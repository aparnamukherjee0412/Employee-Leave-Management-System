\# Employee Leave Management System

A web-based \*\*Employee Leave Management System\*\* built with \*\*ASP.NET Core MVC, Entity Framework Core, SQL Server, and SignalR\*\*.

The system allows administrators to manage employees and leave requests, while employees can apply for leave and track their leave status. It also supports \*\*real-time notifications\*\* when leave requests are approved or rejected.

---
\# Scaffold Statement-

Scaffold-DbContext "server=DESKTOP-DMPU9K3; database=EmpLeaveManagementSystem; integrated security=True; TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir D:\\Proj\\Infrastracture\\DbModels -Force

\# Features

\### Admin

\* Admin Dashboard

\* Add / Edit / Deactivate Employees

\* View and Manage Leave Requests

\* Approve or Reject Leave Applications

\* Generate Leave Reports

\* Export Leave Reports to Excel

\* Real-time notifications for leave status updates



\### Employee



\* Employee Dashboard

\* Apply for Leave

\* View Leave Status (Pending / Approved / Rejected)

\* Receive real-time notifications when leave requests are updated



---



\# Technology Stack



\* ASP.NET Core MVC

\* Entity Framework Core

\* SQL Server

\* SignalR (Real-time notifications)

\* Razor Views

\* Bootstrap (optional UI styling)



---



\# Database Structure



\### Tables



\* Users

\* Employees

\* LeaveRequests



\### Relationships



\* Each \*\*Employee\*\* is linked to a \*\*User\*\*

\* Each \*\*LeaveRequest\*\* belongs to an \*\*Employee\*\*

\* Leave approval can be tracked using the \*\*Admin (User)\*\*



---



\# Setup Instructions



\## 1. Clone the Repository



```bash

git clone <https://github.com/aparnamukherjee0412/Employee-Leave-Management-System>

cd EmpLeaveManagementSystem

```



---



\## 2. Configure Database Connection



Open \*\*appsettings.Development\*\*



Update the connection string:



```json

"ConnectionStrings": {

 "sqlconnection": "Server=YOUR\_SERVER\_NAME;Database=EmpLeaveManagementSystem;Trusted\_Connection=True;TrustServerCertificate=True;"

}

```



---



\## 3. Create Database



Run the SQL script included in the project to create tables:



\* Users

\* Employees

\* LeaveRequests



You can execute the script in \*\*SQL Server Management Studio (SSMS)\*\*.



---



\## 4. Restore NuGet Packages



Open the project in \*\*Visual Studio\*\* and restore packages:



```

Build → Restore NuGet Packages

```



or run:



```bash

dotnet restore

```



---



\## 5. Run the Application



Press \*\*F5\*\* in Visual Studio or run:



```bash

dotnet run

```



The application will start in the browser.



---



\# Default Login Credentials



\## Admin Login



Email:



```

admin@example.com

```



Password:



```

admin123

```



Role:



```

Admin

```



---



\## Employee Login



Email:



```

employee@example.com

```



Password:



```

emp123

```



Role:



```

Employee

```



---



\# How the System Works



\### Leave Application Flow



Employee applies for leave

↓

Leave request status becomes \*\*Pending\*\*

↓

Admin reviews the request

↓

Admin \*\*Approves / Rejects\*\* the leave

↓

Employee receives a \*\*real-time notification\*\*



---



\# Real-Time Notifications



The system uses \*\*SignalR\*\* to notify employees instantly when their leave request is:



\* Approved

\* Rejected



Notifications appear on the employee dashboard.



---



\# Project Structure



```

Controllers

   AdminController

   EmployeeController

   LoginController



Services

   Employees

   LeaveRequests

   Reports

   Users



Repository

   Interfaces



Infrastructure

   DbModels

   DbContext



Views

   Admin

   Employee

   Login



NotificationHub

   SignalR Hub

```



---



\# Future Improvements



\* Role-based authentication using ASP.NET Identity

\* Email notifications

\* Leave balance management

\* Department-wise reporting

\* Calendar view for leave schedules

\* File attachment for leave requests



---



\# Author



Developed as a \*\*sample Employee Leave Management System project\*\* using ASP.NET Core MVC.



---







