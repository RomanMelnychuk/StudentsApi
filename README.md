# StudentsApi

REST API for managing students, courses and enrollments, built with ASP.NET Core and Entity Framework Core.

## Tech stack

- .NET 10, ASP.NET Core Web API
- Entity Framework Core (Code First), SQL Server
- JWT Bearer authentication, BCrypt.Net-Next
- xUnit

## Features

- User registration with BCrypt password hashing
- Login with JWT token generation and validation
- Role claim in the token (User / Admin)
- CRUD for students, courses and enrollments
- Global exception handling middleware with structured logging
- Unit tests for the authentication service

## Architecture

The project is organized in three layers:

- **Controllers** handle HTTP: they read the request, call a service and map the result to a status code. No business logic here.
- **Services** hold the business logic. They validate the case, work with the data and return a result.
- **Repository** isolates data access. `AuthService` depends on `IUserRepository`, not on `DbContext` directly, which is what makes it testable.

Instead of throwing exceptions for expected failures (email already taken, wrong password), `AuthService` and `EnrollmentService` return a `ServiceResult<T>` with `Success`, `Error` and `Data`. The controller checks `Success` and picks the HTTP status. Exceptions are reserved for unexpected errors, which the middleware catches and turns into a 500 response. The student and course services still return values directly — unifying them on `ServiceResult<T>` is on the roadmap.

Login returns the same message for a missing user and a wrong password, so the API does not reveal which emails are registered.

## Project structure

```
StudentsApi/
├── Common/             # ServiceResult<T>
├── Controllers/        # Auth, Students, Courses, Enrollments
├── DTOs/               # Request and response models
├── Middleware/         # Global exception handling
├── Migrations/         # EF Core migrations
├── Models/             # Student, Course, Enrollment, User, UserRole
├── Repositories/       # IUserRepository, UserRepository
├── Services/           # Business logic
├── AppDbContext.cs
└── Program.cs

StudentsApi.Tests/
├── AuthServiceTests.cs
└── FakeUserRepository.cs
```

## Database

Code First with EF Core migrations. Entities:

- **Student** — name, grade, city
- **Course** — title
- **Enrollment** — join entity between Student and Course (many-to-many)
- **User** — email, password hash, role

## Endpoints

| Method | Route | Auth |
|---|---|---|
| POST | `/api/auth/register` | — |
| POST | `/api/auth/login` | — |
| GET | `/api/students` | JWT |
| GET | `/api/students/{id}` | — |
| POST | `/api/students` | — |
| PUT | `/api/students/{id}` | — |
| DELETE | `/api/students/{id}` | — |
| GET | `/api/courses` | — |
| GET | `/api/courses/{id}` | — |
| POST | `/api/courses` | — |
| PUT | `/api/courses/{id}` | — |
| DELETE | `/api/courses/{id}` | — |
| POST | `/api/enrollments` | — |
| GET | `/api/enrollments/student/{studentId}` | — |

## Getting started

```bash
git clone https://github.com/RomanMelnychuk/StudentsApi.git
cd StudentsApi
dotnet restore
dotnet ef database update
dotnet run
```

Connection string and JWT settings are in `appsettings.json`.

## Tests

```bash
dotnet test
```

Five tests cover registration and login in `AuthService`, using a fake repository implementation.

## Roadmap

- Pagination, filtering and sorting
- Role-based authorization on endpoints
- Unify all services on `ServiceResult<T>`
- Swagger documentation
- Move secrets to User Secrets / environment variables
