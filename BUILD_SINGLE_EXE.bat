@echo off
echo ========================================
echo Building Single-File Executable
echo ========================================
echo.

cd /d "%~dp0"

echo Cleaning previous builds...
dotnet clean "LibraryManagementSystem.Wpf/LibraryManagementSystem.Wpf.csproj" --configuration Release
if exist "LibraryManagementSystem.Wpf\bin\Release\net10.0-windows\win-x64\publish" rmdir /s /q "LibraryManagementSystem.Wpf\bin\Release\net10.0-windows\win-x64\publish"

echo.
echo Publishing application...
dotnet publish "LibraryManagementSystem.Wpf/LibraryManagementSystem.Wpf.csproj" --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true

if errorlevel 1 (
    echo.
    echo BUILD FAILED!
    pause
    exit /b 1
)

echo.
echo ========================================
echo Build Complete!
echo ========================================
echo.
echo The executable is located at:
echo LibraryManagementSystem.Wpf\bin\Release\net10.0-windows\win-x64\publish\LibraryManagementSystem.exe
echo.
echo File size: 
cd LibraryManagementSystem.Wpf\bin\Release\net10.0-windows\win-x64\publish
dir LibraryManagementSystem.exe | find "LibraryManagementSystem.exe"
echo.
echo IMPORTANT: Copy the following files together:
echo   - LibraryManagementSystem.exe
echo   - appsettings.json
echo   - library.db (will be created on first run)
echo.
echo NOTE: Users must TYPE their credentials to enable the sign-in button
echo       Default: admin / Admin@123
echo.
pause
