@echo off
echo ========================================
echo Creating Distribution Package
echo ========================================
echo.

REM Build the application
echo Step 1: Building application...
cd /d "%~dp0LibraryManagementSystem.Wpf"
dotnet publish --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true
cd ..

REM Create distribution folder
echo.
echo Step 2: Creating distribution folder...
if exist "Distribution" rmdir /s /q "Distribution"
mkdir "Distribution"

REM Copy files
echo.
echo Step 3: Copying files...
copy "LibraryManagementSystem.Wpf\bin\Release\net10.0-windows\win-x64\publish\LibraryManagementSystem.exe" "Distribution\"
copy "LibraryManagementSystem.Wpf\bin\Release\net10.0-windows\win-x64\publish\appsettings.json" "Distribution\"
copy "DISTRIBUTION_README.txt" "Distribution\README.txt"

REM Show results
echo.
echo ========================================
echo Distribution Package Created!
echo ========================================
echo.
echo Location: Distribution\
echo.
echo Contents:
dir "Distribution" /b
echo.
echo File sizes:
dir "Distribution\LibraryManagementSystem.exe" | find "LibraryManagementSystem.exe"
echo.
echo Next steps:
echo 1. Test the application by running Distribution\LibraryManagementSystem.exe
echo 2. Zip the Distribution folder to share with users
echo 3. Users should extract all files and run LibraryManagementSystem.exe
echo.
pause
