# Compare local WMS mod versions against live Nexus MAIN versions.
# Reads NexusGroupIDS.csv + CHANGELOG.md files + Environment.props for API key.

param([string]$Domain = "graveyardkeeper")

$ErrorActionPreference = "Stop"
$root  = Split-Path -Parent $PSScriptRoot  # repo "Graveyard Keeper" dir
$csv   = Join-Path $root  "NexusGroupIDS.csv"
$env   = Join-Path $root  "src\Environment.props"

if (-not (Test-Path $csv))  { Write-Error "CSV not found: $csv" }
if (-not (Test-Path $env))  { Write-Error "Environment.props not found" }

# Parse API key
[xml]$xml = Get-Content $env
$apiKey = ([string]($xml.Project.PropertyGroup.NexusModsApiKey)).Trim()
if (-not $apiKey) { Write-Error "NexusModsApiKey missing in Environment.props" }

# Build PluginGuid → local directory map
$dirMap = @{}
foreach ($proj in (Get-ChildItem -Path (Join-Path $root "src") -Filter "*.csproj" -Recurse)) {
    $content = Get-Content $proj.FullName -Raw
    if ($content -match '<BepInExPluginGuid>([^<]+)</BepInExPluginGuid>') {
        $dirMap[$matches[1]] = $proj.Directory.FullName
    }
}

# Parse CSV (skip header)
$rows = Get-Content $csv | Select-Object -Skip 1 | ForEach-Object {
    $parts = $_.Split(',')
    [pscustomobject]@{
        Name      = $parts[0]
        NexusId   = $parts[1]
        PluginGuid = $parts[3].Trim()
    }
}

$headers = @{ apikey = $apiKey; 'User-Agent' = 'check-versions.ps1/1.0' }

$results = foreach ($row in $rows) {
    # Find local changelog
    $localVer = "(missing)"
    if ($dirMap.ContainsKey($row.PluginGuid)) {
        $cl = Join-Path $dirMap[$row.PluginGuid] "CHANGELOG.md"
        if (Test-Path $cl) {
            $first = (Get-Content $cl) | Where-Object { $_ -match '^## (\d+\.\d+\.\d+)' } | Select-Object -First 1
            if ($first -match '## (\d+\.\d+\.\d+)') { $localVer = $matches[1] }
        }
    }

    # Query Nexus
    $nexusVer = "?"
    $uploaded = ""
    try {
        $uri = "https://api.nexusmods.com/v1/games/$Domain/mods/$($row.NexusId)/files.json"
        $f = Invoke-RestMethod -Uri $uri -Headers $headers
        $latest = $f.files | Where-Object { $_.category_name -eq 'MAIN' } |
                  Sort-Object uploaded_timestamp -Descending | Select-Object -First 1
        if ($latest) {
            $nexusVer = $latest.version
            $uploaded = [DateTimeOffset]::FromUnixTimeSeconds($latest.uploaded_timestamp).ToString('yyyy-MM-dd')
        } else {
            $nexusVer = "(no MAIN)"
        }
    } catch {
        $nexusVer = "ERR: $($_.Exception.Message.Substring(0, [Math]::Min(40, $_.Exception.Message.Length)))"
    }

    # Compute expected nexus = local - 1 patch
    $status = "?"
    if ($localVer -match '^\d+\.\d+\.\d+$' -and $nexusVer -match '^\d+\.\d+\.\d+$') {
        $lp = [Version]$localVer
        $np = [Version]$nexusVer
        $expected = "$($lp.Major).$($lp.Minor).$($lp.Build - 1)"
        if ($localVer -eq $nexusVer)       { $status = "SAME"    }
        elseif ($nexusVer -eq $expected)   { $status = "OK +1"   }
        elseif ($lp -gt $np)               { $status = "AHEAD $([Math]::Abs($lp.Build - $np.Build - 1) + 1)" }
        elseif ($lp -lt $np)               { $status = "BEHIND" }
    }

    [pscustomobject]@{
        Mod       = $row.Name
        Local     = $localVer
        Nexus     = $nexusVer
        Uploaded  = $uploaded
        Status    = $status
    }

    Start-Sleep -Milliseconds 150
}

$results | Format-Table -AutoSize
