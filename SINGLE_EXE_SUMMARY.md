# Creating a Single Executable - Quick Guide

## TL;DR - Just Run This

```cmd
CREATE_DISTRIBUTION.bat
```

This creates a `Distribution` folder with everything ready to share.

---

## What You Get

Three build scripts are now available:

### 1. `CREATE_DISTRIBUTION.bat` ⭐ RECOMMENDED
- Creates a complete distribution package
- Includes .exe, config, and README
- Ready to zip and share
- **Output:** `Distribution/` folder

### 2. `BUILD_SINGLE_EXE.bat`
- Self-contained executable (~150-200 MB)
- Includes .NET runtime
- No installation required on target PC
- **Output:** `LibraryManagementSystem.Wpf\bin\Release\net10.0-windows\win-x64\publish\`

### 3. `BUILD_SMALL_EXE.bat`
- Smaller executable (~5-10 MB)
- Requires .NET 10.0 Runtime on target PC
- **Output:** `LibraryManagementSystem.Wpf\bin\Release\net10.0-windows\win-x64\publish\`

---

## Quick Comparison

| Feature | Self-Contained | Framework-Dependent |
|---------|---------------|---------------------|
| **File Size** | ~150-200 MB | ~5-10 MB |
| **Requires .NET** | ❌ No | ✅ Yes |
| **Build Script** | BUILD_SINGLE_EXE.bat | BUILD_SMALL_EXE.bat |
| **Best For** | Distribution to users | Internal use |

---

## Distribution Checklist

- [ ] Run `CREATE_DISTRIBUTION.bat`
- [ ] Test the .exe in the `Distribution` folder
- [ ] Verify login works (admin/Admin@123)
- [ ] Check that database is created
- [ ] Zip the `Distribution` folder
- [ ] Share with users

---

## What's Included in Distribution

```
Distribution/
├── LibraryManagementSystem.exe  (150-200 MB)
├── appsettings.json             (<1 KB)
└── README.txt                   (User instructions)
```

The `library.db` file is created automatically on first run.

---

## User Instructions (Include in Email/Documentation)

1. Extract all files from the ZIP
2. Double-click `LibraryManagementSystem.exe`
3. Login with:
   - Username: `admin`
   - Password: `Admin@123`

**Important:** Keep all files in the same folder!

---

## Technical Details

The project file (`LibraryManagementSystem.Wpf.csproj`) has been configured with:

```xml
<PublishSingleFile>true</PublishSingleFile>
<SelfContained>true</SelfContained>
<RuntimeIdentifier>win-x64</RuntimeIdentifier>
<PublishReadyToRun>true</PublishReadyToRun>
<IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
<EnableCompressionInSingleFile>true</EnableCompressionInSingleFile>
```

This ensures:
- Single-file output
- Embedded .NET runtime
- Optimized for Windows 64-bit
- Compressed for smaller size
- Fast startup with ReadyToRun

---

## Troubleshooting

### Build Fails
- Ensure .NET 10.0 SDK is installed
- Run `dotnet --version` to verify
- Try `dotnet clean` first

### .exe Won't Run
- Check Windows Event Viewer for errors
- Ensure all files are together
- Try running as Administrator

### Large File Size
- This is normal for self-contained builds
- Use `BUILD_SMALL_EXE.bat` for smaller size (requires .NET Runtime)

---

## Next Steps

1. **Test locally:** Run the .exe from the Distribution folder
2. **Create installer (optional):** Use tools like Inno Setup or WiX
3. **Code signing (optional):** Sign the .exe to avoid Windows warnings
4. **Auto-updates (optional):** Implement update checking mechanism

---

For more details, see `BUILD_INSTRUCTIONS.md`
