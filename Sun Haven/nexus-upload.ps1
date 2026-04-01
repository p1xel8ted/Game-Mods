param(
    [Parameter(Mandatory)][string]$ApiKey,
    [Parameter(Mandatory)][string]$GroupId,
    [Parameter(Mandatory)][string]$ZipFile,
    [Parameter(Mandatory)][string]$ModName,
    [Parameter(Mandatory)][string]$Version,
    [string]$ChangelogFile = ''
)

$ErrorActionPreference = 'Stop'
$base = 'https://api.nexusmods.com/v3'
$headers = @{ apikey = $ApiKey }

if (-not (Test-Path $ZipFile)) {
    Write-Error "File not found: $ZipFile"
    exit 1
}

# Read changelog for the current version
$description = $null
if ($ChangelogFile -and (Test-Path $ChangelogFile)) {
    $content = Get-Content $ChangelogFile -Raw
    # Match the section for the current version (## X.Y.Z followed by lines until next ## or EOF)
    if ($content -match "(?ms)## $([regex]::Escape($Version))\s*\n(.*?)(?=\n## |\z)") {
        $description = $Matches[1].Trim()
        if ($description.Length -gt 255) {
            $description = $description.Substring(0, 252) + '...'
        }
        Write-Host "Changelog: $description"
    } else {
        Write-Host "No changelog entry found for v$Version in $ChangelogFile"
    }
}

$fileSize = (Get-Item $ZipFile).Length
$fileName = Split-Path $ZipFile -Leaf

# Use multipart upload (same as NexusMods' own GitHub action)
Write-Host "`n[1/6] Creating multipart upload session ($fileName, $fileSize bytes)..."
$body = @{ size_bytes = $fileSize; filename = $fileName } | ConvertTo-Json
$createResult = Invoke-RestMethod -Uri "$base/uploads/multipart" -Method Post -Headers $headers -ContentType 'application/json' -Body $body

$uploadId = $createResult.data.id
$partSize = $createResult.data.parts_size
$partUrls = $createResult.data.parts_presigned_url
$completeUrl = $createResult.data.complete_presigned_url
Write-Host "       Upload ID: $uploadId"
Write-Host "       Parts: $($partUrls.Count), Part size: $partSize bytes"

# Upload each part
Write-Host "[2/6] Uploading file parts..."
$fileStream = [System.IO.File]::OpenRead($ZipFile)
$etags = @()
try {
    for ($i = 0; $i -lt $partUrls.Count; $i++) {
        $offset = $i * $partSize
        $remaining = $fileSize - $offset
        $thisPartSize = [Math]::Min($partSize, $remaining)
        $buffer = New-Object byte[] $thisPartSize
        $fileStream.Position = $offset
        $null = $fileStream.Read($buffer, 0, $thisPartSize)

        Write-Host "       Part $($i + 1)/$($partUrls.Count) ($thisPartSize bytes)..."

        $partResponse = Invoke-WebRequest -Uri $partUrls[$i] -Method Put -Body $buffer -ContentType 'application/octet-stream' -UseBasicParsing
        $etag = $partResponse.Headers['ETag']
        if ($etag -is [array]) { $etag = $etag[0] }
        $etag = $etag -replace '"', ''
        $etags += $etag
    }
} finally {
    $fileStream.Close()
}

# Complete multipart upload
Write-Host "[3/6] Completing multipart upload..."
$xmlParts = ""
for ($i = 0; $i -lt $etags.Count; $i++) {
    $xmlParts += "  <Part>`n    <PartNumber>$($i + 1)</PartNumber>`n    <ETag>$($etags[$i])</ETag>`n  </Part>`n"
}
$completeXml = "<CompleteMultipartUpload>`n$xmlParts</CompleteMultipartUpload>"

Invoke-WebRequest -Uri $completeUrl -Method Post -Body $completeXml -ContentType 'application/xml' -UseBasicParsing | Out-Null

# Finalise
Write-Host "[4/6] Finalising upload..."
Invoke-RestMethod -Uri "$base/uploads/$uploadId/finalise" -Method Post -Headers $headers | Out-Null

# Poll
Write-Host "[5/6] Waiting for processing..."
for ($i = 0; $i -lt 60; $i++) {
    Start-Sleep -Seconds 2
    $status = Invoke-RestMethod -Uri "$base/uploads/$uploadId" -Method Get -Headers $headers
    if ($status.data.state -eq 'available') {
        Write-Host "       Upload ready."
        break
    }
    if ($i % 5 -eq 0) {
        Write-Host "       State: $($status.data.state)... ($($i * 2)s)"
    }
}

if ($status.data.state -ne 'available') {
    Write-Error "Upload did not become available within 2 minutes"
    exit 1
}

Start-Sleep -Seconds 3

# Publish
Write-Host "[6/6] Publishing to NexusMods (group $GroupId)..."
$publishBody = @{
    upload_id = $uploadId
    name = $ModName
    version = $Version
    file_category = 'main'
}
if ($description) {
    $publishBody['description'] = $description
}
$publishBody = $publishBody | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "$base/mod-file-update-groups/$GroupId/versions" -Method Post -Headers $headers -ContentType 'application/json' -Body $publishBody
    Write-Host "`n=== SUCCESS ==="
    Write-Host "$ModName v$Version published to NexusMods"
    Write-Host "Group: $GroupId"
    Write-Host "Upload: $uploadId"
} catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    $responseBody = ''
    try {
        $stream = $_.Exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($stream)
        $responseBody = $reader.ReadToEnd()
    } catch {}
    Write-Error "Publish failed ($statusCode): $responseBody"
    exit 1
}
