param(
    [Parameter(Mandatory)][string]$ApiKey,
    [Parameter(Mandatory)][string]$GroupId,
    [Parameter(Mandatory)][string]$ZipFile,
    [Parameter(Mandatory)][string]$ModName,
    [Parameter(Mandatory)][string]$Version
)

$ErrorActionPreference = 'Stop'
$base = 'https://api.nexusmods.com/v3'
$headers = @{ apikey = $ApiKey }
$description = 'Please see bundled CHANGELOG.md file for changes.'

if (-not (Test-Path $ZipFile)) {
    Write-Host "ERROR: File not found: $ZipFile"
    exit 1
}

$fileSize = (Get-Item $ZipFile).Length
$fileName = Split-Path $ZipFile -Leaf

# Step 1: Create multipart upload session (with retry)
Write-Host ""
Write-Host "[1/6] Creating upload session ($fileName, $fileSize bytes)..."
$body = @{ size_bytes = $fileSize; filename = $fileName } | ConvertTo-Json

$createResult = $null
for ($attempt = 1; $attempt -le 5; $attempt++) {
    try {
        $createResult = Invoke-RestMethod -Uri "$base/uploads/multipart" -Method Post -Headers $headers -ContentType 'application/json' -Body $body
        break
    } catch {
        $wait = [Math]::Min(30 * [Math]::Pow(1.5, $attempt), 120)
        Write-Host "       Failed (attempt $attempt/5): $($_.Exception.Message)"
        Write-Host "       Waiting $([int]$wait)s..."
        Start-Sleep -Seconds ([int]$wait)
    }
}
if (-not $createResult) {
    Write-Host "ERROR: Failed to create upload session after 5 attempts"
    exit 1
}

$uploadId = $createResult.data.id
$partSize = $createResult.data.parts_size
$partUrls = $createResult.data.parts_presigned_url
$completeUrl = $createResult.data.complete_presigned_url
Write-Host "       Upload ID: $uploadId"

# Step 2: Upload file parts to S3 (with retry + Content-Length)
Write-Host "[2/6] Uploading file..."
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
        $partResponse = $null
        for ($retry = 1; $retry -le 3; $retry++) {
            try {
                $partResponse = Invoke-WebRequest -Uri $partUrls[$i] -Method Put -Body $buffer -ContentType 'application/octet-stream' -Headers @{ 'Content-Length' = $thisPartSize.ToString() } -UseBasicParsing
                break
            } catch {
                $wait = 15 * $retry
                Write-Host "       Part upload failed (attempt $retry/3): $($_.Exception.Message)"
                Write-Host "       Waiting ${wait}s..."
                Start-Sleep -Seconds $wait
            }
        }
        if (-not $partResponse) {
            Write-Host "ERROR: Failed to upload part $($i + 1) after 3 attempts"
            exit 1
        }
        $etag = $partResponse.Headers['ETag']
        if ($etag -is [array]) { $etag = $etag[0] }
        $etag = $etag -replace '"', ''
        $etags += $etag
    }
} finally {
    $fileStream.Close()
}

# Step 3: Complete multipart upload
Write-Host "[3/6] Completing multipart upload..."
$xmlParts = ""
for ($i = 0; $i -lt $etags.Count; $i++) {
    $xmlParts += "  <Part>`n    <PartNumber>$($i + 1)</PartNumber>`n    <ETag>$($etags[$i])</ETag>`n  </Part>`n"
}
$completeXml = "<CompleteMultipartUpload>`n$xmlParts</CompleteMultipartUpload>"
Invoke-WebRequest -Uri $completeUrl -Method Post -Body $completeXml -ContentType 'application/xml' -UseBasicParsing | Out-Null

# Step 4: Finalise upload
Write-Host "[4/6] Finalising upload..."
Invoke-RestMethod -Uri "$base/uploads/$uploadId/finalise" -Method Post -Headers $headers | Out-Null

# Step 5: Poll with exponential backoff (matching NexusMods' own GitHub action)
Write-Host "[5/6] Waiting for processing..."
$delay = 2
for ($i = 0; $i -lt 20; $i++) {
    Start-Sleep -Seconds ([int]$delay)
    $status = Invoke-RestMethod -Uri "$base/uploads/$uploadId" -Method Get -Headers $headers
    Write-Host "       State: $($status.data.state) (after $([int]$delay)s)"
    if ($status.data.state -eq 'available') {
        break
    }
    $delay = [Math]::Min($delay * 1.5, 30)
}

if ($status.data.state -ne 'available') {
    Write-Host "ERROR: Upload did not become available"
    exit 1
}

# Step 6: Publish (with retry for 429)
Write-Host "[6/6] Publishing to NexusMods (group $GroupId)..."
$publishBody = @{
    upload_id = $uploadId
    name = $ModName
    version = $Version
    description = $description
    file_category = 'main'
} | ConvertTo-Json

$published = $false
for ($attempt = 1; $attempt -le 3; $attempt++) {
    try {
        Invoke-RestMethod -Uri "$base/mod-file-update-groups/$GroupId/versions" -Method Post -Headers $headers -ContentType 'application/json' -Body $publishBody | Out-Null
        $published = $true
        break
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        if ($code -eq 429) {
            $wait = 60 * $attempt
            Write-Host "       Rate limited (429). Waiting ${wait}s... (attempt $attempt/3)"
            Start-Sleep -Seconds $wait
        } else {
            $errBody = ''
            try { $s = $_.Exception.Response.GetResponseStream(); $r = [System.IO.StreamReader]::new($s); $errBody = $r.ReadToEnd() } catch {}
            Write-Host "ERROR: Publish failed (HTTP $code): $errBody"
            exit 1
        }
    }
}

if (-not $published) {
    Write-Host "ERROR: Publish failed after 3 attempts"
    exit 1
}

Write-Host ""
Write-Host "=== SUCCESS ==="
Write-Host "$ModName v$Version published to NexusMods (group $GroupId)"
