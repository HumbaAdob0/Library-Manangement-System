### Final Requirements Document for C# WPF Application – **Library Management System** - Group 4

## This document outlines the final requirements for the development of a **Library Management System** (LMS) using **C# WPF** (Windows Presentation Foundation) technology. The system will manage library resources, patrons, transactions, and reporting, providing an efficient and user-friendly way to manage a library's operations.

## **Project Overview**

### **Project Name:** Library Management System (LMS)

### **Platform:** Windows Desktop

### **Technology Stack:**

- **Programming Language**: C#
- **Framework**: .NET 5/6/7 (latest stable version) with WPF (Windows Presentation Foundation) for UI
- **Database**: SQLite / SQL Server (depending on the size and scale of deployment)
- **Architecture**: MVVM (Model-View-ViewModel)
- **Libraries & Tools**:
  - **Entity Framework Core** for data access
  - **AutoMapper** for mapping models
  - **Xceed WPF Toolkit** for extended UI controls (optional)
  - **Material Design in XAML Toolkit** for modern UI (optional)

### **Project Objective**

To build a user-friendly application to manage library operations such as book tracking, patron management, book checkouts/returns, overdue fines, and reporting. The system will provide an intuitive interface for librarians, admins, and staff to handle these tasks efficiently.

---

## **Functional Requirements**

### 1. **User Authentication and Roles**

- **Login/Logout**:  
  The system will have a login screen that verifies user credentials. Users will authenticate with a username and password.
- **Roles**:
  - **Admin**: Full access to the system, including the ability to manage users, books, patrons, transactions, and settings.
  - **Librarian**: Limited access to manage books, patrons, and transactions. Cannot manage users or system settings.
- **Password Reset**:  
  Admins will have the ability to reset a user's password.

---

### 2. **Book Management**

- **Add New Book**:
  - Fields: Title, Author, ISBN, Genre, Publisher, Year Published, Number of Copies.
  - The system should allow adding new books, including details like description, publisher, etc.
- **Edit Book Information**:
  - Admin and Librarian can edit book details like title, author, and quantity.
- **Delete Book**:
  - Admins can delete books that are no longer available in the library.
- **View Book List**:
  - A list view should display all books with details like title, author, and availability status.
  - The list should support sorting and searching by title, author, and ISBN.

---

### 3. **Patron (Library Member) Management**

- **Add New Patron**:
  - Fields: Full Name, Membership ID, Email, Phone Number, Address, Date of Birth, Membership Type (Standard/Premium).
- **Edit Patron Information**:
  - Admins and Librarians can update patron information, such as contact details or membership type.
- **Delete Patron**:
  - Admins can remove patrons from the system.
- **View Patron List**:
  - A list view should show patron details, including their borrowing history.

---

### 4. **Transaction Management (Book Checkout and Return)**

- **Checkout Books**:
  - Patrons can check out books by selecting one or more available books.
  - Fields: Patron Name, Book(s), Checkout Date, Due Date.
  - The system will check for book availability and update the status to "Checked Out."
- **Return Books**:
  - Patrons can return books, and the system will update the status to "Available."
  - The system will calculate overdue fines (if applicable).
- **Overdue Fees**:
  - If a patron returns a book late, the system will automatically calculate fines based on the number of overdue days.
  - Admins and Librarians can view overdue fees in a report.

---

### 5. **Search and Filter**

- **Search Books**:
  - The system should allow searching by book title, author, ISBN, or genre.
- **Filter Books by Availability**:
  - Users can filter books by availability: Available, Checked Out, or All.
- **Filter Patrons by Membership**:
  - Users can filter patrons by membership type or status (Active/Inactive).
- **Search Transactions**:
  - Admins and Librarians can search for specific transactions by patron, book, or transaction date.

---

### 6. **Reports and Analytics**

- **Overdue Books Report**:
  - Display a list of books that are overdue, including patron details and overdue days.
- **Checked Out Books Report**:
  - A report showing which books are currently checked out and their due dates.
- **Transaction History Report**:
  - A detailed report of all transactions over a specified period, including checkouts and returns.
- **Patron Activity Report**:
  - A report showing patron activity, including the number of books checked out, overdue books, and fines.
- **Export to PDF/Excel**:
  - All reports should be exportable to PDF, Excel, or CSV formats for easy distribution and analysis.

---

## **Non-Functional Requirements**

### 1. **Performance**

- The application should be responsive with minimal latency when performing actions like searching, checking out books, or generating reports.
- The system should be able to handle a minimum of 500 concurrent transactions (book checkouts/returns) efficiently.
- Reports should be generated within 30 seconds even with large data sets.

### 2. **Security**

- **Authentication**: Use a secure login process with hashed passwords.
- **Role-Based Access Control (RBAC)**: Implement user roles (Admin, Librarian) to ensure proper access control.
- **Data Encryption**: Use encryption for sensitive data, especially patron information and transaction data.
- **Data Integrity**: Ensure no data corruption during database transactions, especially in case of unexpected shutdowns.

### 3. **Usability**

- **Intuitive UI**: The user interface should be easy to navigate, with clearly labeled buttons, forms, and notifications.
- **Error Handling**: The application should handle errors gracefully and provide clear feedback to users.
- **Accessibility**: The system should support keyboard navigation, and color contrast should be suitable for users with visual impairments.

### 4. **Scalability**

- The system should support adding new modules (e.g., fine payment, inventory management) without major changes to the core structure.
- It should be scalable to support large libraries with up to **10,000 books** and **5,000 patrons** without performance degradation.

### 5. **Maintainability**

- The application should be modular, with clear separation between the UI, business logic, and data access layers (using the MVVM pattern).
- Use **dependency injection** to manage services and improve testability.

### 6. **Localization**

- Support for multiple languages, starting with **English** and **Spanish**.
- The system should adjust for different time zones and formats (e.g., date formats) depending on the user's region.

---

## **Technical Requirements**

### 1. **Development Tools**

- **IDE**: Visual Studio 2022 or later.
- **Language**: C# (latest stable version).
- **Framework**: .NET 5/6 with WPF for the UI.
- **Database**: SQLite for local deployment or SQL Server for large-scale use.
- **MVVM Pattern**: Implement the MVVM pattern to separate concerns and make the application more maintainable.

### 2. **Database Design**

- **Books Table**: ID, Title, Author, ISBN, Genre, Publisher, Published Year, Quantity.
- **Patrons Table**: ID, Name, Membership ID, Email, Phone, Address, Membership Type, Status.
- **Transactions Table**: Transaction ID, Book ID, Patron ID, Checkout Date, Due Date, Return Date, Fine Amount.
- **Fines Table**: Fine ID, Patron ID, Amount, Date Applied.

**Relationships**:

- Books are linked to transactions (many-to-many).
- Patrons are linked to transactions (one-to-many).
- Fines are linked to patrons (one-to-many).

### 3. **Testing**

- **Unit Tests**: Implement unit tests for core business logic (e.g., calculating fines, checking availability).
- **UI Tests**: Use **Selenium** or **Appium** for automated UI testing.
- **Integration Tests**: Ensure the application works end-to-end, with proper interactions between the UI, business logic, and database.

### 4. **Deployment**

- **Installer**: Use **ClickOnce** or **WiX Toolset** for application deployment.
- **Database Backup and Restore**: Admins should be able to back up and restore the database through the UI.

---

## **Project Timeline**

- **Phase 1: Requirements Gathering & Design** (2 weeks)
  - Finalize project scope and design UI/UX.
  - Set up the database schema and architecture.
- **Phase 2: Core Development** (6 weeks)
  - Develop key features: Book management, patron management, transactions, and reports.
  - Implement the UI using WPF.
- **Phase 3: Testing & Bug Fixing** (2 weeks)
  - Unit, integration, and UI testing.
  - Address any identified issues.
- \*\*Phase 4:

Deployment & Documentation\*\* (1 week)

- Finalize application deployment.
- Provide user and technical documentation.
- Hand over the application to the client.
