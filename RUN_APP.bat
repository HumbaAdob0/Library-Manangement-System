@echo off
echo ========================================
echo Library Management System - WPF
echo ========================================
echo.
echo Building application...
cd "LibraryManagementSystem.Wpf"
dotnet build
echo.
echo Starting application...
dotnet run
pause
