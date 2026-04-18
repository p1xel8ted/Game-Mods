# Touch the date on each CHANGELOG's top entry IF that entry's version is ahead of Nexus.
# Run with -WhatIf to preview without writing.

param(
    [string]$Domain = "graveyardkeeper",
    [string]$NewDate = (Get-Date -Format "d MMMM yyyy"),
    [switch]$WhatIf
)

$ErrorActionPreference = "Stop"
$root  = Split-Path -Parent $PSScriptRoot
$csv   = Join-Path $root  "NexusGroupIDS.csv"
$env   = Join-Path $root  "src\Environment.props"

[xml]$xml = Get-Content $env
$apiKey = ([string]($xml.Project.PropertyGroup.NexusModsApiKey)).Trim()

$dirMap = @{}
foreach ($proj in (Get-ChildItem -Path (Join-Path $root "src") -Filter "*.csproj" -Recurse)) {
    $content = Get-Content $proj.FullName -Raw
    if ($content -match '<BepInExPluginGuid>([^<]+)</BepInExPluginGuid>') {
        $dirMap[$matches[1]] = $proj.Directory.FullName
    }
}

$headers = @{ apikey = $apiKey; 'User-Agent' = 'update-unreleased-dates.ps1/1.0' }

$rows = Get-Content $csv | Select-Object -Skip 1 | ForEach-Object {
    $parts = $_.Split(',')
    [pscustomobject]@{ Name = $parts[0]; NexusId = $parts[1]; Guid = $parts[3].Trim() }
}

foreach ($row in $rows) {
    if (-not $dirMap.ContainsKey($row.Guid)) { continue }
    $cl = Join-Path $dirMap[$row.Guid] "CHANGELOG.md"
    if (-not (Test-Path $cl)) { continue }

    $lines = Get-Content $cl
    $topIdx = $null
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match '^## (\d+\.\d+\.\d+)') {
            $topIdx = $i
            $localVer = $matches[1]
            break
        }
    }
    if ($null -eq $topIdx) { continue }

    # Query Nexus
    try {
        $f = Invoke-RestMethod -Uri "https://api.nexusmods.com/v1/games/$Domain/mods/$($row.NexusId)/files.json" -Headers $headers
        $latest = $f.files | Where-Object { $_.category_name -eq 'MAIN' } |
                  Sort-Object uploaded_timestamp -Descending | Select-Object -First 1
        $nexusVer = if ($latest) { $latest.version } else { "(none)" }
    } catch {
        Write-Host "SKIP $($row.Name): Nexus API error"
        continue
    }

    if ($localVer -eq $nexusVer) {
        Write-Host "SAME $($row.Name) v$localVer — skipping"
        continue
    }

    $old = $lines[$topIdx]
    $new = "## $localVer | $NewDate"
    if ($old -eq $new) {
        Write-Host "KEEP $($row.Name) v$localVer already dated $NewDate"
        continue
    }

    Write-Host "TOUCH $($row.Name): '$old' → '$new'"
    if (-not $WhatIf) {
        $lines[$topIdx] = $new
        $lines | Set-Content $cl
    }

    Start-Sleep -Milliseconds 150
}
