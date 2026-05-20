# Building a Single-File Executable

This guide explains how to create a standalone executable for the Library Management System that can run on any Windows machine without requiring .NET SDK installation.

## Prerequisites

- .NET 10.0 SDK installed on your development machine
- Windows operating system

## Build Options

### Option 1: Self-Contained Single-File (Recommended)

Creates a single `.exe` file (~150-200 MB) that includes the .NET runtime. Users don't need to install anything.

**Steps:**

1. Run the build script:
   ```cmd
   BUILD_SINGLE_EXE.bat
   ```

2. The executable will be created at:
   ```
   LibraryManagementSystem.Wpf\bin\Release\net10.0-windows\win-x64\publish\LibraryManagementSystem.exe
   ```

3. **Important:** Copy these files together to distribute:
   - `LibraryManagementSystem.exe` (the main executable)
   - `appsettings.json` (configuration file)
   - `library.db` (database - will be created on first run if not present)

### Option 2: Framework-Dependent (Smaller Size)

Creates a smaller executable (~5-10 MB) but requires .NET 10.0 Runtime to be installed on the target machine.

**Command:**
```cmd
cd LibraryManagementSystem.Wpf
dotnet publish --configuration Release --runtime win-x64 --self-contained false -p:PublishSingleFile=true
```

### Option 3: Manual Publish Command

If you want to customize the build, use this command:

```cmd
cd LibraryManagementSystem.Wpf
dotnet publish --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:PublishReadyToRun=true
```

## Distribution

### What to Include

Create a folder with these files:

```
LibraryManagementSystem/
├── LibraryManagementSystem.exe  (the main executable)
├── appsettings.json              (configuration)
└── README.txt                    (optional: user instructions)
```

The `library.db` database file will be created automatically on first run.

### Creating a ZIP Package

1. Navigate to the publish folder:
   ```cmd
   cd LibraryManagementSystem.Wpf\bin\Release\net10.0-windows\win-x64\publish
   ```

2. Copy the required files to a new folder:
   ```cmd
   mkdir ..\..\..\..\..\..\..\Distribution
   copy LibraryManagementSystem.exe ..\..\..\..\..\..\..\Distribution\
   copy appsettings.json ..\..\..\..\..\..\..\Distribution\
   ```

3. Zip the Distribution folder and share it with users.

## File Sizes

- **Self-Contained Single-File:** ~150-200 MB (includes .NET runtime)
- **Framework-Dependent:** ~5-10 MB (requires .NET 10.0 Runtime)
- **appsettings.json:** <1 KB
- **library.db:** ~100 KB (grows with data)

## Reducing File Size

If the 150-200 MB size is too large, consider these options:

### 1. Use Framework-Dependent Build
Requires users to install .NET 10.0 Runtime (free download from Microsoft).

### 2. Enable Trimming (Advanced)
Add to `.csproj`:
```xml
<PublishTrimmed>true</PublishTrimmed>
<TrimMode>link</TrimMode>
```

**Warning:** Trimming can cause runtime errors with reflection-heavy code (like Entity Framework). Test thoroughly!

### 3. Use ReadyToRun Compilation
Already enabled in the project. Improves startup time but increases file size slightly.

## Troubleshooting

### "The application requires .NET Runtime"
- You're using a framework-dependent build. Either:
  - Install .NET 10.0 Runtime on the target machine, OR
  - Rebuild with `--self-contained true`

### "Cannot find appsettings.json"
- Ensure `appsettings.json` is in the same folder as the `.exe`

### "Database error on startup"
- The app will create `library.db` automatically
- Ensure the folder has write permissions

### Large File Size
- Self-contained builds include the entire .NET runtime (~150 MB)
- This is normal and allows the app to run without .NET installation
- Consider framework-dependent build if size is critical

## Default Credentials

After first run, use these credentials to log in:

- **Admin Account:**
  - Username: `admin`
  - Password: `Admin@123`

- **Librarian Account:**
  - Username: `librarian`
  - Password: `Librarian@123`

## System Requirements

- **Operating System:** Windows 10 or later (64-bit)
- **RAM:** 512 MB minimum, 1 GB recommended
- **Disk Space:** 300 MB for self-contained build
- **Display:** 1280x720 minimum resolution

## Support

For issues or questions, refer to the project documentation or contact the development team.
