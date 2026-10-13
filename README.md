# Digital Library Management System

A full-stack web application for managing books, students, book issuing, returns, and reports.

## Features
- JWT-based authentication
- Dashboard with book and issue summaries
- Add, view, and delete books
- Add and view students
- Issue and return books
- Reports with issue and return history
- Input validation and protected API endpoints

## Tech Stack
- **Frontend:** Angular, TypeScript, HTML, CSS
- **Backend:** ASP.NET Core Web API, C#
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Authentication:** JWT Bearer

## Project Structure
```text
Digital Library/
├── Frontend/digital-library/
├── Backend/DigitalLibrary.API/
├── .gitignore
└── README.md
```

## Setup

**1. Database**

Configure your SQL Server connection in the backend settings, then run the existing EF Core migrations:

```bash
dotnet ef database update
```

**2. Backend**

From `Backend/DigitalLibrary.API`, configure these environment variables:

- `Jwt__Key` — development signing key of at least 32 bytes
- `DIGITAL_LIBRARY_ADMIN_USERNAME`
- `DIGITAL_LIBRARY_ADMIN_PASSWORD`

Then run:

```bash
dotnet restore
dotnet run --urls http://localhost:5018
```



**3. Frontend**

From `Frontend/digital-library`, run:

```bash
npm install
ng serve
```

Open `http://localhost:4200`.

## Future Improvements
- Deployment
- Automated testing
- Search and pagination
- Responsive UI improvements

**Project Status:** Core features implemented; production builds and core API workflows verified. Full browser-based login verification remains outstanding.

Digital Library Management System — an project built with Angular, ASP.NET Core Web API, Entity Framework Core, and SQL Server.