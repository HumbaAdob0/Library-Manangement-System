@echo off
echo ========================================
echo Building Single-File Executable
echo ========================================
echo.

cd /d "%~dp0LibraryManagementSystem.Wpf"

echo Cleaning previous builds...
dotnet clean --configuration Release
if exist "bin\Release\net10.0-windows\win-x64\publish" rmdir /s /q "bin\Release\net10.0-windows\win-x64\publish"

echo.
echo Publishing application...
dotnet publish --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true

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
echo IMPORTANT: Copy the following files together:
echo   - LibraryManagementSystem.exe
echo   - appsettings.json
echo   - library.db (will be created on first run)
echo.
pause
