# 📚 Library Management System

Course project built with **.NET 8** using **Clean Architecture**.  
It combines a REST API, SOAP client, and Razor Pages frontend into a complete library management system.

---

## 🔹 Features

### REST API (ASP.NET Core Web API)
- Implements **3 related entities**:  
  - **Books** (with availability tracking)  
  - **Members** (library users)  
  - **Borrowings** (borrowing/return with due dates & status)  
- Full **CRUD** support for books and members.  
- Borrow / return operations update availability automatically.  
- **Middleware** adds a `x-correlation-id` header for tracing requests.  
- **Authentication & Authorization**:  
  - JWT-based login for `admin` and `user`.  
  - Role-based access control.  
    - **Admin**: manage books, members, borrowings.  
    - **User**: can only view books and check SOAP.  
- **EF Core InMemory** repository.  
- **Swagger** UI available.  
- **Unit tests** included for core logic.

### SOAP Module
- Simple SOAP service + client.  
- Demonstrates calling and consuming SOAP endpoints from Razor Pages.

### Razor Pages (Frontend)
- Web UI with Bootstrap styling.  
- **Login / Logout** with JWT stored in session + cookies.  
- **Role-based navigation**:  
  - Admins see *Books*, *Members*, and *Borrowings*.  
  - Users only see *Books* and *SOAP*.  
- Features:
  - Books list with badges for availability.  
  - Member management.  
  - Borrow / return operations.  
  - Status badges (Borrowed / Returned / Overdue).  

### Scripted API Tests
- Batch script: **`test-api.bat`**  
- Runs a sequence of **cURL commands** that:  
  - Log in as admin  
  - Test CRUD on books & members  
  - Borrow and return a book  
  - Verify middleware headers (`x-correlation-id`)  
  - Negative test: user cannot create a book (403).  
- Output shows results of all endpoints for easy verification.

---

## 🔹 Roles Summary

- **Admin**  
  - Add / edit / delete books  
  - Add members  
  - Borrow & return books  
  - View full borrowing history  

- **User**  
  - View books list  
  - Access SOAP test page  
  - Cannot modify data  

---

## 🔹 How to Run

1. Start the **Web API** (`Library.Api`).  
2. Start the **Razor Pages frontend** (`Library.Web`).  
3. Visit: [http://localhost:7266](http://localhost:7266).  
4. Credentials:  
   - `admin / admin123`  
   - `user / user123`
5. Run **`Library.Api/test-api.bat`** to verify endpoints with cURL.


