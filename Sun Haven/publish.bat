@echo off
setlocal EnableDelayedExpansion

echo.
echo ========================================
echo  Sun Haven Mod Publisher
echo ========================================
echo.

set "SLNDIR=%~dp0"
set "FAILED="
set "SUCCESS=0"

if "%~1"=="" (
    echo Usage:
    echo   publish.bat                        - Upload ALL mods
    echo   publish.bat Seedify                - Upload single mod
    echo   publish.bat Seedify EasyLiving     - Upload multiple mods
    echo.
    set /p "CONFIRM=Upload ALL mods? (y/n): "
    if /i not "!CONFIRM!"=="y" (
        echo Cancelled.
        pause
        exit /b 0
    )
    echo.

    for %%m in (KeepAlive UIScales CheatEnabler EasyLiving AutoTools NoTimeForFishing NoTimeToStopAndEat MoreScythesRedux MuseumSellPriceRedux MoreJewelry CharacterEditRedux Seedify MoreTheMerrier) do (
        call :buildmod %%m
    )
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

pwsh -NoProfile -ExecutionPolicy Bypass -File "%SLNDIR%nexus-upload.ps1" -ApiKey "OU9lYTM3cmVZM2c4aURlcHhldDRMZSs0NElMZlVTOTZaMHpyZlFVQWU3R2ZwTjgxUnpMcWtGQkxMSXV5aldYRy0tVUxvVXBwS0Z3dURxNGR0Y1ZCMGN5UT09--07e79ee1a9d300ed7473c300784ea7384fc8459e" -GroupId "!GROUPID!" -ZipFile "!ZIPFILE!" -ModName "%MOD%" -Version "!VERSION!"

if !errorlevel! neq 0 (
    set "FAILED=!FAILED! %MOD%"
) else (
    set /a SUCCESS+=1
)

echo.
echo  Waiting 45 seconds...
timeout /t 45 /nobreak >nul
exit /b 0
