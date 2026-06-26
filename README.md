# Task API — ASP.NET Core 8 REST API

A clean, production-ready REST API for task management built with ASP.NET Core 8, Entity Framework Core, and SQLite.

## Project Structure

```
TaskApi/
├── Controllers/
│   └── TasksController.cs     # HTTP endpoints
├── Data/
│   └── AppDbContext.cs        # EF Core DbContext + seed data
├── DTOs/
│   └── TaskDtos.cs            # Request/response shapes
├── Middleware/
│   └── ErrorHandlingMiddleware.cs  # Global error handling
├── Models/
│   └── TaskItem.cs            # Domain entity
├── Services/
│   └── TaskService.cs         # Business logic (interface + impl)
├── Program.cs                 # App setup & DI registration
├── appsettings.json
└── TaskApi.csproj
```

## Quick Start

**Prerequisites:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
cd TaskApi
dotnet run
```

Open **http://localhost:5000** — Swagger UI loads automatically.

## API Endpoints

| Method   | Endpoint                    | Description                        |
|----------|-----------------------------|------------------------------------|
| `GET`    | `/api/tasks`                | List tasks (paginated + filtered)  |
| `GET`    | `/api/tasks/{id}`           | Get task by ID                     |
| `POST`   | `/api/tasks`                | Create a new task                  |
| `PUT`    | `/api/tasks/{id}`           | Replace a task                     |
| `DELETE` | `/api/tasks/{id}`           | Delete a task                      |
| `PATCH`  | `/api/tasks/{id}/toggle`    | Toggle completion status           |

### Query Parameters (GET /api/tasks)

| Param         | Type    | Default | Description                        |
|---------------|---------|---------|------------------------------------|
| `page`        | int     | 1       | Page number                        |
| `pageSize`    | int     | 10      | Items per page (max 100)           |
| `isCompleted` | bool?   | null    | Filter by completion status        |
| `priority`    | enum?   | null    | Filter by priority (Low/Medium/High)|

### Example Requests

```bash
# Create a task
curl -X POST http://localhost:5000/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title":"Buy groceries","priority":"High","dueDate":"2026-07-01T00:00:00Z"}'

# List incomplete high-priority tasks
curl "http://localhost:5000/api/tasks?isCompleted=false&priority=High"

# Toggle complete
curl -X PATCH http://localhost:5000/api/tasks/1/toggle
```

## Switching to SQL Server or PostgreSQL

1. Add the NuGet package:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   # or
   dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
   ```

2. Update `Program.cs`:
   ```csharp
   options.UseSqlServer(connectionString);
   // or
   options.UseNpgsql(connectionString);
   ```

3. Update `appsettings.json` with your connection string.

## Key Design Decisions

- **Service layer** (`ITaskService`) keeps business logic out of controllers and makes unit testing easy
- **DTOs** decouple the API contract from the domain model
- **Global error middleware** catches all unhandled exceptions and returns consistent JSON errors
- **Enum serialization** as strings (e.g. `"High"` not `2`) for readable API responses
- **Seeded data** so the API works immediately on first run
