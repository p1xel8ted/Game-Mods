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
set "SKIPPED=0"

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
        echo ----------------------------------------
        echo  Building %%m...
        echo ----------------------------------------
        dotnet build "%SLNDIR%%%m\%%m.csproj" -c Release-Upload
        if !errorlevel! neq 0 (
            set "FAILED=!FAILED! %%m"
        ) else (
            set /a SUCCESS+=1
        )
        echo.
    )
) else (
    for %%m in (%*) do (
        echo ----------------------------------------
        echo  Building %%m...
        echo ----------------------------------------
        dotnet build "%SLNDIR%%%m\%%m.csproj" -c Release-Upload
        if !errorlevel! neq 0 (
            set "FAILED=!FAILED! %%m"
        ) else (
            set /a SUCCESS+=1
        )
        echo.
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
