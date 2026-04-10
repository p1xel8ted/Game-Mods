param(
    [string]$GameDomain = 'graveyardkeeper'
)

$ErrorActionPreference = 'Stop'
$scriptDir = $PSScriptRoot
$envProps  = Join-Path $scriptDir 'Environment.props'
$csvPath   = Join-Path $scriptDir '..\NexusGroupIDS.csv'

# --- Read API key from Environment.props (git-ignored) ---
if (-not (Test-Path $envProps)) {
    Write-Host "ERROR: $envProps not found."
    Write-Host "Create it with <NexusModsApiKey>...</NexusModsApiKey> before pulling."
    exit 1
}
[xml]$propsXml = Get-Content $envProps
$apiKey = $propsXml.Project.PropertyGroup.NexusModsApiKey
if (-not $apiKey) {
    Write-Host "ERROR: NexusModsApiKey not found in $envProps"
    exit 1
}

if (-not (Test-Path $csvPath)) {
    Write-Host "ERROR: $csvPath not found."
    exit 1
}

# --- CSV display name -> csproj directory name ---
$nameMap = @{
    'Alchemy Research Redux'       = 'AlchemyResearchRedux'
    'Grave Changes Redux'          = 'GraveChangesRedux'
    'Get Outta Ma Way'             = 'GetOuttaMaWay'
    'Keepers Candles'              = 'KeepersCandles'
    'Show Me Moar'                 = 'ShowMeMoar'
    'Decomp Delight'               = 'DecompDelight'
    "Where's Ma' Veggies"          = 'WheresMaVeggies'
    'Bring Out Yer Dead'           = 'BringOutYerDead'
    'Pray the Day Away'            = 'PrayTheDayAway'
    "Where's Ma' Points"           = 'WheresMaPoints'
    'Give Me Moar'                 = 'GiveMeMoar'
    'Economy Reloaded'             = 'EconomyReloaded'
    'Regeneration Reloaded'        = 'RegenerationReloaded'
    'FasterCraft Reloaded'         = 'FasterCraftReloaded'
    "Gerry's Junk Trunk"           = 'GerrysJunkTrunk'
    'Wheres Ma Storage'            = 'WheresMaStorage'
    'Beam Me Up Gerry'             = 'BeamMeUpGerry'
    'I Build Where I Want'         = 'IBuildWhereIWant'
    'MaxButton Controller Support' = 'MaxButtonControllerSupport'
    'The Seed Equalizer'           = 'TheSeedEqualizer'
    'Queue Everything'             = 'QueueEverything'
    'I Neeeed Sticks'              = 'INeedSticks'
    'Misc. Bits and Bobs'          = 'MiscBitsAndBobs'
    'Apple Trees Enhanced'         = 'AppleTreesEnhanced'
    'Longer Days'                  = 'LongerDays'
    'Thoughtful Reminders'         = 'ThoughtfulReminders'
    'Auto-Loot Heavies'            = 'AutoLootHeavies'
    'Trees No More'                = 'TreesNoMore'
    'Add Straight To Table'        = 'AddStraightToTable'
    'No Intros'                    = 'NoIntros'
    'Fog Be Gone'                  = 'FogBeGone'
    'No Time For Fishing'          = 'NoTimeForFishing'
    'New Game At Bottom'           = 'NewGameAtBottom'
    'Exhaust-less'                 = 'Exhaustless'
    'Save Now'                     = 'SaveNow'
}

$base    = "https://api.nexusmods.com/v1/games/$GameDomain/mods"
$headers = @{ apikey = $apiKey; 'User-Agent' = 'nexus-pull.ps1/1.0' }
$rows    = Import-Csv $csvPath
$stats   = [ordered]@{ Success = 0; Failed = 0 }

function Invoke-NexusGet {
    param([string]$Url, [hashtable]$Headers)
    # Retry once on 429
    try {
        return Invoke-RestMethod -Uri $Url -Headers $Headers -Method Get
    } catch {
        $code = $null
        if ($_.Exception.Response) { $code = $_.Exception.Response.StatusCode.value__ }
        if ($code -eq 429) {
            Write-Host "       Rate limited (429). Waiting 60s and retrying once..."
            Start-Sleep -Seconds 60
            return Invoke-RestMethod -Uri $Url -Headers $Headers -Method Get
        }
        if ($code -eq 401) {
            Write-Host "ERROR: Authentication failed (401). Check NexusModsApiKey in Environment.props."
            exit 1
        }
        throw
    }
}

function Format-Changelog {
    param($Changelogs)

    $sb = [System.Text.StringBuilder]::new()
    [void]$sb.AppendLine('# Changelog')
    [void]$sb.AppendLine()

    if (-not $Changelogs -or -not $Changelogs.PSObject.Properties.Count) {
        return $sb.ToString()
    }

    # Sort versions descending — try [version] first, fall back to string sort if any key fails to parse
    $props = @($Changelogs.PSObject.Properties)
    $sorted = $null
    try {
        $sorted = $props | Sort-Object { [version]($_.Name) } -Descending
    } catch {
        $sorted = $props | Sort-Object Name -Descending
    }

    foreach ($p in $sorted) {
        [void]$sb.AppendLine("## $($p.Name)")
        [void]$sb.AppendLine()
        foreach ($line in @($p.Value)) {
            [void]$sb.AppendLine("- $line")
        }
        [void]$sb.AppendLine()
    }

    return $sb.ToString()
}

Write-Host ""
Write-Host "========================================"
Write-Host " Nexus Pull ($GameDomain)"
Write-Host "========================================"
Write-Host ""

foreach ($row in $rows) {
    $modName = $row.Mod
    $modId   = $row.NexusID

    if (-not $nameMap.ContainsKey($modName)) {
        Write-Host "SKIP (unmapped): $modName"
        $stats.Failed++
        continue
    }

    $dir    = $nameMap[$modName]
    $modDir = Join-Path $scriptDir $dir

    if (-not (Test-Path $modDir)) {
        Write-Host "SKIP (dir not found): $modDir"
        $stats.Failed++
        continue
    }

    Write-Host "[$modName] ID $modId -> $dir"

    # --- Fetch mod info (description) ---
    try {
        $info = Invoke-NexusGet -Url "$base/$modId.json" -Headers $headers
    } catch {
        Write-Host "       FAIL (description fetch): $($_.Exception.Message)"
        $stats.Failed++
        continue
    }

    $description = $info.description
    if (-not $description) { $description = '(No description provided)' }

    $descPath = Join-Path $modDir 'NEXUS-DESC.md'
    [System.IO.File]::WriteAllText($descPath, $description, [System.Text.UTF8Encoding]::new($false))
    Write-Host "       NEXUS-DESC.md ($($description.Length) chars)"

    Start-Sleep -Seconds 1

    # --- Fetch changelogs ---
    try {
        $changelogs = Invoke-NexusGet -Url "$base/$modId/changelogs.json" -Headers $headers
    } catch {
        $code = $null
        if ($_.Exception.Response) { $code = $_.Exception.Response.StatusCode.value__ }
        if ($code -eq 404) {
            # No changelogs published yet — write empty file
            $changelogs = $null
        } else {
            Write-Host "       FAIL (changelog fetch): $($_.Exception.Message)"
            $stats.Failed++
            Start-Sleep -Seconds 1
            continue
        }
    }

    $changelogMd = Format-Changelog -Changelogs $changelogs
    $versionCount = 0
    if ($changelogs) { $versionCount = @($changelogs.PSObject.Properties).Count }

    $clPath = Join-Path $modDir 'CHANGELOG.md'
    [System.IO.File]::WriteAllText($clPath, $changelogMd, [System.Text.UTF8Encoding]::new($false))
    Write-Host "       CHANGELOG.md ($versionCount versions)"

    $stats.Success++
    Start-Sleep -Seconds 1
}

Write-Host ""
Write-Host "========================================"
Write-Host " Summary"
Write-Host "========================================"
Write-Host ("Success: {0}" -f $stats.Success)
Write-Host ("Failed:  {0}" -f $stats.Failed)
