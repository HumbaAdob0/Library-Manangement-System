@echo off
echo ========================================
echo Building Framework-Dependent Executable
echo (Requires .NET 10.0 Runtime on target PC)
echo ========================================
echo.

cd /d "%~dp0LibraryManagementSystem.Wpf"

echo Cleaning previous builds...
dotnet clean --configuration Release

echo.
echo Publishing application...
dotnet publish --configuration Release --runtime win-x64 --self-contained false -p:PublishSingleFile=true

echo.
echo ========================================
echo Build Complete!
echo ========================================
echo.
echo The executable is located at:
echo LibraryManagementSystem.Wpf\bin\Release\net10.0-windows\win-x64\publish\LibraryManagementSystem.exe
echo.
echo File size: 
cd bin\Release\net10.0-windows\win-x64\publish
dir LibraryManagementSystem.exe | find "LibraryManagementSystem.exe"
echo.
echo NOTE: This build requires .NET 10.0 Runtime to be installed on the target PC.
echo Download from: https://dotnet.microsoft.com/download/dotnet/10.0
echo.
echo IMPORTANT: Copy the following files together:
echo   - LibraryManagementSystem.exe
echo   - appsettings.json
echo   - library.db (will be created on first run)
echo.
pause
