================================================================================
                    LIBRARY MANAGEMENT SYSTEM v1.0.0
================================================================================

INSTALLATION
------------
1. Extract all files to a folder of your choice
2. Double-click LibraryManagementSystem.exe to run

IMPORTANT: Keep these files together in the same folder:
  - LibraryManagementSystem.exe
  - appsettings.json
  - library.db (created automatically on first run)


FIRST TIME LOGIN
----------------
Use these default credentials to log in:

Admin Account:
  Username: admin
  Password: Admin@123

Librarian Account:
  Username: librarian
  Password: Librarian@123

IMPORTANT: You must TYPE your credentials!
- The sign-in button is disabled until you type both username and password
- This is normal security behavior
- The button will enable automatically as you type


FEATURES
--------
- Manage Books: Add, edit, delete, and search books
- Manage Patrons: Track library members and their information
- Transactions: Check out and return books
- Fines: Automatic calculation for overdue books ($0.50/day)
- Reports: View statistics and analytics
- User Management: Admin can manage user accounts


SYSTEM REQUIREMENTS
-------------------
- Windows 10 or later (64-bit)
- 512 MB RAM (1 GB recommended)
- 300 MB disk space
- 1280x720 screen resolution or higher


TROUBLESHOOTING
---------------
Q: The application won't start
A: Make sure all files are in the same folder. If using the small version,
   install .NET 10.0 Runtime from https://dotnet.microsoft.com/download

Q: I forgot my password
A: Delete the library.db file to reset to default credentials
   (WARNING: This will delete all data)

Q: The database is corrupted
A: Delete library.db and restart the application to create a fresh database

Q: How do I backup my data?
A: Copy the library.db file to a safe location


SUPPORT
-------
For questions or issues, contact your system administrator.


================================================================================
                    © 2026 Library Management System
================================================================================
