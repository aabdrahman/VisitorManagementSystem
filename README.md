# 🏢 The Visitor Management System  
[![.NET](https://img.shields.io/badge/.NET%208.0-blueviolet?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor WebAssembly](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![Entity Framework Core](https://img.shields.io/badge/ORM-EF%20Core-green?logo=nuget&logoColor=white)](https://learn.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-red?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![Bootstrap](https://img.shields.io/badge/UI-Bootstrap-563d7c?logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
[![JWT Auth](https://img.shields.io/badge/Auth-JWT-orange?logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![NLog](https://img.shields.io/badge/Logging-NLog-darkblue?logo=nlog&logoColor=white)](https://nlog-project.org/)
[![Blazored.SessionStorage](https://img.shields.io/badge/Session-Blazored.SessionStorage-2b8a3e?logo=blazor&logoColor=white)](https://github.com/Blazored/SessionStorage)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

---

## 📖 Overview

**The Visitor Management System (VMS)** is a comprehensive visitor tracking solution built with **.NET Core** and **Blazor WebAssembly**.  
It simplifies the process of managing visitors in an organization — allowing self-service walk-ins, scheduled visits, administrative oversight, and real-time monitoring.

Designed for security, transparency, and user convenience, The VMS ensures every visitor interaction is logged, validated, and authorized with minimal friction.

---

## 🧩 Architecture

The system is divided into **two major front-end modules**:

1. **👤 Self-Service Portal** — Enables walk-in visitors to register themselves quickly upon arrival.
2. **👩‍💼 Management Portal** — For registered users, admins, and receptionists to:
   - Schedule upcoming visits  
   - Approve or deny requests  
   - Track visitor activities in real time  
   - View analytics and summaries  

Both front-ends communicate with a **.NET Core Web API** backend secured by **JWT authentication** and powered by **Entity Framework Core** with a **SQL Server** database.

---

### Prerequisites
- .NET 8 SDK or later
- SQL Server instance

### Setup
Clone the repository and open it in Visual Studio or VS Code.

```bash
git clone https://github.com/yourusername/PrLexVisitorManagementSystem.git
```

## ⚙️ Tech Stack

| Layer | Technology | Description |
|-------|-------------|-------------|
| **Frontend** | [Blazor WebAssembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) | Dynamic SPA using C# and Razor components |
| **User Session Management** | [Blazored Storage](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) | Session Management in browser storage |
| **Backend** | [.NET 8 Web API](https://dotnet.microsoft.com/) | RESTful API serving all modules |
| **Database** | [SQL Server](https://www.microsoft.com/en-us/sql-server) | Persistent storage using EF Core |
| **ORM** | [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) | Code-first ORM with migrations |
| **Logging** | [NLog](https://nlog-project.org/) | Structured and file-based logging |
| **Authentication** | [JWT (JSON Web Token)](https://jwt.io/) | Role-based authentication and authorization |
| **Styling** | [Bootstrap 5](https://getbootstrap.com/) | Responsive UI components |
| **License** | [MIT](LICENSE) | Free to use and modify |

---
## 📋 Key Features  

The **The Visitor Management System** is designed to streamline visitor registration, scheduling, and management through a secure, role-based workflow.  

### 👥 Visitor Management  
- 📝 **Self-Service Registration** — Walk-in visitors can easily register themselves via the Blazor WebAssembly frontend.  
- 🔔 **Visit Scheduling** — Registered users can pre-schedule visits and invite guests.  
- 🕵️ **Visit Tracking** — Admins and receptionists can view, filter, and manage active and completed visits.  
- ⏱️ **Real-Time Status Updates** — Automatic state updates for scheduled, ongoing, or completed visits.  

### 🔐 Authentication & Authorization  
- 🔑 **JWT-based Authentication** for secure access to the API.  
- 🧩 **Role-based Authorization** across both frontend and backend:
  - **Admin** — Full system control, user management, and visitor logs.  
  - **Receptionist** — Handles check-ins, check-outs, and on-site validations.  
  - **Registered User** — Can schedule and manage personal visitor appointments.  
  - **Visitor** — Can self-register for visits.  

### ⚙️ System Architecture & Functionality  
- 🏗️ **Modular Layered Design** — Clear separation between UI, business logic, and data layers.  
- 📁 **NLog Integration** — File-based structured logging for diagnostics and monitoring.  
- 🧮 **EF Core ORM** — Simplifies database operations using strongly typed models.  
- 💾 **SQL Server** — Reliable and scalable data persistence.  
- 🧠 **Session Management with Blazored.SessionStorage** — Maintains user sessions seamlessly on the frontend.  

### 🎨 User Experience  
- 💻 **Responsive Bootstrap UI** — Optimized for both desktop and tablet devices.  
- 🔄 **Smooth Transitions** between user roles and pages.  
- 🧭 **Intuitive Navigation** and clear data presentation.  

---


---

## 🧱 Project Structure
## 🧩 Architecture

The **The Visitor Management System (VMS)** follows a **repository pattern with onion architecture**, designed for scalability, maintainability, and clear separation of concerns.

### 🏗️ System Overview

The solution consists of the following major components:

| Layer | Component | Description |
|-------|------------|-------------|
| **Frontend - Self Service (Blazor WebAssembly)** | `VisitorManagementSystem.SelfServiceUI` | A rich single-page application (SPA) built with Blazor WebAssembly. Handles all user-facing interactions. |
| **Frontend - Frontend (Blazor WebAssembly)** | `VisitorManagementSystem.SelfServiceUI` | A rich single-page application (SPA) built with Blazor WebAssembly. Handles all management operations for admin, receptionist and users. |
| **Backend (API Layer)** | `VisitorManagementSystem` | A .NET Core Web API project that exposes endpoints for all visitor operations. Handles authentication, authorization, and business logic. |
| **Shared Library** | `Shared` | Contains DTOs, enums, and shared models that are used across the API and Frontend projects to maintain strong typing and consistency. |
| **Data Layer** | `VisitorManagementSystem.Repository` | Houses Entity Framework Core context, configurations, and repository implementations that communicate directly with the SQL Server database. |
| **Logging Layer** | `VisitorManagementSystem.LoggerManager` | Centralized NLog configuration for structured logging across all layers of the application. |

---

### 🔄 Data Flow

1. **Frontend (Blazor)**  
   - Users interact via the browser (WebAssembly).  
   - Actions such as registration, scheduling, and approval trigger HTTP requests to the backend API.  
   - JWT tokens are used to authenticate each request.

2. **Backend (API)**  
   - Receives requests, validates tokens, and applies business logic.  
   - Interacts with the EF Core data context to read or write data.  
   - Logs operations and exceptions using **NLog**.  
   - Returns standardized API responses to the frontend.

3. **Database (SQL Server)**  
   - Stores visitor records, user profiles, roles, appointments, and logs.  
   - Uses EF Core migrations to evolve schema safely and version-control database changes.

4. **Logging (NLog)**  
   - All requests, errors, and background processes are logged to the `/log` directory.  
   - Each log file includes timestamps, severity levels, and source context.

---

### 👥 User Roles & Responsibilities

| Role | Description | Permissions |
|------|--------------|-------------|
| **Visitor (Walk-in)** | Registers themselves using the self-service Blazor interface. | Create visit requests only. |
| **Registered User / Host** | Schedules and manages visits for guests. | Schedule, update, or cancel visits. |
| **Receptionist** | Checks in and checks out visitors upon arrival or departure. | Manage on-site visitors, verify check-ins, record departures. |
| **Administrator** | Oversees all system operations. | Full access — manage users, roles, configurations, and analytics. |

---

### ⚙️ Security & Authentication

- **JWT-based authentication** ensures each request is securely validated.  
- **Role-based access control (RBAC)** restricts features to specific roles both on the frontend and backend.  
- Passwords are securely hashed before storage.  
- Tokens include expiry durations and issuer/audience validation for integrity.

---

### 🧰 Background Services

- The system includes **background jobs** (hosted services) that perform:
  - Automatic cleanup of soft-deleted records after a configured period.
  - Log maintenance and archival.
- These services run silently and do not block user operations.

---

### 📊 Analytics & Reporting

- Admin users can view visitor statistics such as:
  - Total visits (daily, weekly, monthly)
  - Check-in / check-out trends
  - Cancelled or pending visit counts
- Reports are filterable by time range using an analytics boundary system (`ReportAnalyticsBoundaryDto`).

---

### 🧩 Design Principles

- **Separation of Concerns** – Each layer has a distinct responsibility.  
- **Dependency Injection** – Services and repositories are injected via DI container for loose coupling.  
- **DTO Mapping** – Entities are mapped to Data Transfer Objects to control what data is exposed externally.  
- **Error Handling** – Global exception middleware translates exceptions into consistent HTTP responses.  
- **Logging First** – Every operation and failure is traceable through the NLog system.

---

### 🧱 Architecture Overview  

The **The Visitor Management System** follows a modern layered architecture that separates concerns between the user interface, backend API, and data layer.  
This design ensures **scalability**, **maintainability**, and **robust security** across the entire stack.

```text
 ┌───────────────────────┐
 │     Blazor WASM UI    │
 │ (Self-Service & Admin)│
 └───────────┬───────────┘
             │ HTTPS + JWT
             ▼
 ┌───────────────────────┐
 │     .NET Core API     │
 │ Authentication + Logic│
 └───────────┬───────────┘
             │ EF Core ORM
             ▼
 ┌───────────────────────┐
 │     SQL Server DB     │
 │ Visitor Data Storage  │
 └───────────┬───────────┘
             │
             ▼
 ┌───────────────────────┐
 │       NLog Logs       │
 │ (File-based Logging)  │
 └───────────────────────┘

```
### Upcoming Features
- Email notifications integration
- Real-time visitor tracking
- QR-based check-in/out
- Group Visit Schedule and Creation

## 📫 Contact  

Have questions, suggestions, or want to contribute to the project **Visitor Management System**?  
Feel free to reach out — collaboration and feedback are always welcome!  

**👤 Author:** Abdrahman Akande  
**📧 Email:** [abdrahman.akande@gmail.com](mailto:abdrahman.akande@gmail.com)  
**💻 GitHub:** [@abdrahman-akande](https://github.com/aabdrahman)  
**🌍 Location:** Nigeria  

If you find this project helpful, please consider giving it a ⭐ on [GitHub](https://github.com/aabdrahman/VisitorManagementSystem)!  

---

> _“Building secure, elegant, and efficient systems — using .net stack”_


