param(
    [string]$GameDomain = 'graveyardkeeper',
    [switch]$ShowDiff
)

$ErrorActionPreference = 'Stop'
$scriptDir = $PSScriptRoot
$envProps  = Join-Path $scriptDir 'Environment.props'
$csvPath   = Join-Path $scriptDir '..\NexusGroupIDS.csv'
$tempDir   = Join-Path $env:TEMP 'gyk-nexus-compare'

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

# --- CSV display name -> csproj directory name (mirrors nexus-pull.ps1) ---
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
    'Max Buttons Redux'            = 'MaxButtonsRedux'
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
    'Rest In Patches'              = 'RestInPatches'
}

$base    = "https://api.nexusmods.com/v1/games/$GameDomain/mods"
$headers = @{ apikey = $apiKey; 'User-Agent' = 'nexus-compare.ps1/1.0' }
$rows    = Import-Csv $csvPath

# Ensure temp dir exists and is clean
if (Test-Path $tempDir) { Remove-Item $tempDir -Recurse -Force }
New-Item -ItemType Directory -Path $tempDir | Out-Null

function Invoke-NexusGet {
    param([string]$Url, [hashtable]$Headers)
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

function Normalise-Bbcode {
    # Normalise line endings to LF, strip Nexus' <br /> line-break tags
    # (the API returns the HTML-rendered form), trim trailing whitespace
    # per line, and trim leading/trailing blank lines.
    param([string]$Text)
    if ($null -eq $Text) { return '' }
    $s = $Text -replace "`r`n", "`n" -replace "`r", "`n"
    $s = $s -replace '<br\s*/?>', ''
    $lines = $s -split "`n" | ForEach-Object { $_ -replace '\s+$', '' }
    $joined = ($lines -join "`n").Trim("`n")
    return $joined
}

Write-Host ""
Write-Host "========================================"
Write-Host " Nexus Compare ($GameDomain)"
Write-Host "========================================"
Write-Host ""

$results = New-Object System.Collections.Generic.List[object]

foreach ($row in $rows) {
    $modName = $row.Mod
    $modId   = $row.NexusID

    if (-not $nameMap.ContainsKey($modName)) {
        Write-Host "SKIP (unmapped): $modName"
        continue
    }

    $dir    = $nameMap[$modName]
    $modDir = Join-Path $scriptDir $dir
    $localPath = Join-Path $modDir 'NEXUS-DESC.bbcode'

    if (-not (Test-Path $modDir)) {
        Write-Host "SKIP (dir not found): $modDir"
        continue
    }

    # --- Fetch description from Nexus ---
    $remote = $null
    try {
        $info = Invoke-NexusGet -Url "$base/$modId.json" -Headers $headers
        $remote = $info.description
        if (-not $remote) { $remote = '' }
    } catch {
        Write-Host "FAIL ($modName): $($_.Exception.Message)"
        $results.Add([pscustomobject]@{
            Mod = $modName; Dir = $dir; NexusId = $modId; Status = 'fetch-failed'; LocalChars = 0; RemoteChars = 0
        })
        Start-Sleep -Seconds 1
        continue
    }

    # --- Load local ---
    $hasLocal = Test-Path $localPath
    $local = if ($hasLocal) { [System.IO.File]::ReadAllText($localPath) } else { '' }

    # --- Write remote copy to temp for manual inspection ---
    $remoteTempPath = Join-Path $tempDir ("{0}.bbcode" -f $dir)
    [System.IO.File]::WriteAllText($remoteTempPath, $remote, [System.Text.UTF8Encoding]::new($false))

    # --- Normalise and compare ---
    $localN  = Normalise-Bbcode $local
    $remoteN = Normalise-Bbcode $remote

    $status = if (-not $hasLocal)              { 'no-local' }
              elseif ($localN -eq $remoteN)    { 'match' }
              else                             { 'differ' }

    $line = "{0,-28} {1,-8} local={2,6} nexus={3,6}" -f $modName, $status, $local.Length, $remote.Length
    Write-Host $line

    if ($ShowDiff -and $status -eq 'differ') {
        Write-Host "       remote copy saved: $remoteTempPath"
    }

    $results.Add([pscustomobject]@{
        Mod = $modName; Dir = $dir; NexusId = $modId; Status = $status;
        LocalChars = $local.Length; RemoteChars = $remote.Length;
        RemotePath = $remoteTempPath
    })

    Start-Sleep -Seconds 1
}

Write-Host ""
Write-Host "========================================"
Write-Host " Summary"
Write-Host "========================================"

$byStatus = $results | Group-Object Status | Sort-Object Name
foreach ($g in $byStatus) {
    Write-Host ("{0,-14} {1}" -f $g.Name, $g.Count)
}

$needsUpdate = $results | Where-Object { $_.Status -eq 'differ' -or $_.Status -eq 'no-local' }

Write-Host ""
Write-Host "========================================"
Write-Host " Mods needing Nexus description update"
Write-Host "========================================"
if ($needsUpdate.Count -eq 0) {
    Write-Host "(none — all descriptions match)"
} else {
    foreach ($r in $needsUpdate) {
        Write-Host ("- {0}  (mod {1}, {2})" -f $r.Mod, $r.NexusId, $r.Status)
    }
    Write-Host ""
    Write-Host "Remote copies for diff inspection: $tempDir"
    Write-Host "Tip: run with -ShowDiff to print the temp path inline."
}
