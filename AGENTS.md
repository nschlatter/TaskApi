# AGENTS.md

Purpose: Quick onboarding instructions for AI coding agents working on this repository.

Contents
- **Project type**: .NET 8 Web API (minimal API / controller-based hybrid)
- **Build**: `dotnet build`
- **Run**: `dotnet run` (runs at http://localhost:5000 with Swagger in Development)
- **Restore**: `dotnet restore`
- **Test**: `dotnet test` (no test projects present)

Key files
- Program: [Program.cs](Program.cs#L1) — app configuration, DI, EF Core, middleware, Swagger
- Controllers: [Controllers/TasksController.cs](Controllers/TasksController.cs#L1) — API endpoints
- Services: [Services/TaskService.cs](Services/TaskService.cs#L1) — business logic and DTO mapping
- Data: [Data/AppDbContext.cs](Data/AppDbContext.cs#L1) — EF Core DbContext, seed data
- Models: [Models/TaskItem.cs](Models/TaskItem.cs#L1) — domain entity and enums
- DTOs: [DTOs/TaskDtos.cs](DTOs/TaskDtos.cs#L1) — request/response DTOs and `PagedResponse<T>`
- Middleware: [Middleware/ErrorHandlingMiddleware.cs](Middleware/ErrorHandlingMiddleware.cs#L1) — global error handling
- Config: [appsettings.json](appsettings.json#L1) — SQLite connection string and logging

Conventions & notes for agents
- Follow existing layering: Controllers → Services → Data → Models.
- Use `PagedResponse<T>` when adding paginated endpoints or responses.
- Prefer adding unit tests in a new `TaskApi.Tests` project using xUnit and mock `AppDbContext` with `UseInMemoryDatabase`.
- Do not change the database provider without updating `appsettings.json` and seed logic.
- Keep enum JSON representation as strings (project config uses `JsonStringEnumConverter`).

Missing items an agent may add
- `TaskApi.Tests` project (xUnit) with sample tests for `TaskService`.
- CI workflow: `.github/workflows/dotnet.yml` to build and test on push.
- Developer docs: `docs/ARCHITECTURE.md` linking to `Program.cs` and `AppDbContext` for startup behavior.

If you want, I can add a minimal `TaskApi.Tests` project and a GitHub Actions workflow next.
