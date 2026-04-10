@echo off
setlocal EnableDelayedExpansion

echo.
echo ========================================
echo  Graveyard Keeper Mod Publisher
echo ========================================
echo.

set "SLNDIR=%~dp0"
set "FAILED="
set "SUCCESS=0"

REM Read API key from git-ignored Environment.props
set "ENVPROPS=%SLNDIR%Environment.props"
if not exist "%ENVPROPS%" (
    echo ERROR: %ENVPROPS% not found.
    echo Create it with ^<NexusModsApiKey^>...^</NexusModsApiKey^> before publishing.
    pause
    exit /b 1
)
for /f "tokens=*" %%k in ('findstr /r "NexusModsApiKey" "%ENVPROPS%"') do set "KEYLINE=%%k"
set "APIKEY=!KEYLINE:*<NexusModsApiKey>=!"
set "APIKEY=!APIKEY:</NexusModsApiKey>=!"
set "APIKEY=!APIKEY: =!"
if "!APIKEY!"=="" (
    echo ERROR: NexusModsApiKey not found in %ENVPROPS%
    pause
    exit /b 1
)

if "%~1"=="" (
    echo Usage:
    echo   publish.bat                            - Upload ALL mods with NexusGroupId
    echo   publish.bat BeamMeUpGerry              - Upload single mod
    echo   publish.bat BeamMeUpGerry SaveNow      - Upload multiple mods
    echo.

    REM Auto-discover mods by scanning csproj files for NexusGroupId
    set "DISCOVERED="
    for /d %%d in ("%SLNDIR%*") do (
        if exist "%%d\%%~nxd.csproj" (
            findstr /r "NexusGroupId" "%%d\%%~nxd.csproj" >nul 2>&1
            if not errorlevel 1 (
                set "DISCOVERED=!DISCOVERED! %%~nxd"
            )
        )
    )
    if "!DISCOVERED!"=="" (
        echo No mods with NexusGroupId found in %SLNDIR%
        pause
        exit /b 0
    )
    echo Discovered mods:!DISCOVERED!
    echo.
    set /p "CONFIRM=Upload all discovered mods? (y/n): "
    if /i not "!CONFIRM!"=="y" (
        echo Cancelled.
        pause
        exit /b 0
    )
    echo.

    for %%m in (!DISCOVERED!) do call :buildmod %%m
) else (
    for %%m in (%*) do (
        call :buildmod %%m
    )
)

echo.
echo ========================================
echo  Results: !SUCCESS! succeeded
if defined FAILED (
    echo  FAILED:!FAILED!
)
echo ========================================
pause
exit /b 0

:buildmod
set "MOD=%~1"
set "CSPROJ=%SLNDIR%%MOD%\%MOD%.csproj"

if not exist "%CSPROJ%" (
    echo  ERROR: %CSPROJ% not found
    set "FAILED=!FAILED! %MOD%"
    exit /b 1
)

echo ----------------------------------------
echo  Building %MOD%...
echo ----------------------------------------

REM Build and zip only (Release config)
dotnet build "%CSPROJ%" -c Release --nologo -v q 2>&1 | findstr /i "error warning succeeded failed"

REM Read NexusGroupId from csproj
for /f "tokens=*" %%g in ('findstr /r "NexusGroupId" "%CSPROJ%"') do set "GROUPLINE=%%g"
set "GROUPID=!GROUPLINE:*<NexusGroupId>=!"
set "GROUPID=!GROUPID:</NexusGroupId>=!"
set "GROUPID=!GROUPID: =!"

if "!GROUPID!"=="" (
    echo  No NexusGroupId — skipping upload for %MOD%
    set /a SUCCESS+=1
    exit /b 0
)

REM Read Version from csproj
for /f "tokens=*" %%v in ('findstr /r "<Version>" "%CSPROJ%" ^| findstr /v "Assembly File"') do set "VERLINE=%%v"
set "VERSION=!VERLINE:*<Version>=!"
set "VERSION=!VERSION:</Version>=!"
set "VERSION=!VERSION: =!"

set "ZIPFILE=%SLNDIR%AAA_Releases\%MOD%_v!VERSION!_BepInEx.zip"

if not exist "!ZIPFILE!" (
    echo  ERROR: Zip not found: !ZIPFILE!
    set "FAILED=!FAILED! %MOD%"
    exit /b 1
)

echo.
echo  Uploading %MOD% v!VERSION! (group !GROUPID!)...
echo.

pwsh -NoProfile -ExecutionPolicy Bypass -File "%SLNDIR%nexus-upload.ps1" -ApiKey "!APIKEY!" -GroupId "!GROUPID!" -ZipFile "!ZIPFILE!" -ModName "%MOD%" -Version "!VERSION!"

if !errorlevel! neq 0 (
    set "FAILED=!FAILED! %MOD%"
) else (
    set /a SUCCESS+=1
)

echo.
echo  Waiting 45 seconds...
timeout /t 45 /nobreak >nul
exit /b 0
