@echo off
echo ========================================
echo Creating Distribution Package
echo ========================================
echo.

REM Build the application
echo Step 1: Building application...
cd /d "%~dp0"
dotnet publish "LibraryManagementSystem.Wpf/LibraryManagementSystem.Wpf.csproj" --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true

if errorlevel 1 (
    echo.
    echo ========================================
    echo BUILD FAILED!
    echo ========================================
    pause
    exit /b 1
)

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
echo 2. Type username: admin and password: Admin@123
echo 3. Zip the Distribution folder to share with users
echo.
echo IMPORTANT: Users must TYPE their credentials (button enables after typing)
echo.
pause
