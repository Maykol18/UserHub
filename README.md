# UserHub

UserHub is a learning project for building a user management REST API with **ASP.NET Core and .NET 10**. The project is being developed incrementally, following a story-based workflow with an emphasis on clean architecture, maintainability, testing, and secure configuration.

## Project goals

The long-term goal is to build an API that supports:

- User creation, retrieval, updating, and deletion.
- Authentication and authorization with ASP.NET Core Identity and JWT.
- User roles and account status management.
- Search, filtering, sorting, and pagination.
- Validation, consistent error responses, logging, and automated tests.
- Containerization and CI/CD.

Features will be implemented in separate development stories. Not every feature listed above is implemented yet.

## Technology stack

- .NET 10
- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity and JWT (planned)
- FluentValidation (planned)
- Serilog (planned)
- xUnit (planned/for automated tests)
- Docker and GitHub Actions (planned)

## Solution structure

The solution is organized into the following projects:

```text
UserHub/
├── UserHub.API/             # HTTP endpoints and application entry point
├── UserHub.Application/     # Application use cases and contracts
├── UserHub.Domain/          # Domain models and business rules
├── UserHub.Infrastructure/  # Database and external infrastructure
├── UserHub.Tests/           # Automated tests
├── UserHub.slnx
└── README.md
```

The intended dependency direction keeps business and application logic independent of infrastructure details. The API project acts as the composition root where dependencies are registered.

## Prerequisites

Before running the project locally, install:

1. [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
2. [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Developer or Express edition is suitable for local development), or another compatible SQL Server instance.
3. [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/ssms/install/install) or another SQL client (optional, but useful for inspecting your database).
4. [Git](https://git-scm.com/downloads)

Verify the .NET SDK installation:

```bash
dotnet --version
```

The output should show an installed .NET 10 SDK version.

## Getting started

### 1. Clone the repository

```bash
git clone https://github.com/Maykol18/UserHub.git
cd UserHub
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Configure the local SQL Server connection

UserHub reads the database connection string using the configuration key:

```text
ConnectionStrings:DefaultConnection
```

For local development, store the connection string in **ASP.NET Core User Secrets** rather than committing machine-specific connection details or credentials to Git.

The API project already has a `UserSecretsId` configured in its project file. From the repository root, run:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=UserHubDb;Trusted_Connection=True;TrustServerCertificate=True;" --project UserHub.API
```

Adjust the `Server` value to match your SQL Server installation. For example, a SQL Server Express instance may use `localhost\\SQLEXPRESS`, while LocalDB commonly uses `(localdb)\\MSSQLLocalDB`.

To confirm that the setting was saved:

```bash
dotnet user-secrets list --project UserHub.API
```

Do not commit actual passwords, access tokens, or production connection strings. If your SQL Server setup uses SQL authentication, store the appropriate connection string in User Secrets and never publish its password.

### 4. Keep development settings free of local connection strings

The tracked file `UserHub.API/appsettings.Development.json` should not contain your personal connection string. It can remain an empty JSON object:

```json
{}
```

ASP.NET Core loads configuration from multiple providers. In the Development environment, User Secrets are loaded by the default application builder and override values from appsettings files when the same key is present.

The application can retrieve the connection string with:

```csharp
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");
```

The database context and its registration will be introduced in a later development story.

## Build and test

Run these commands from the repository root:

```bash
dotnet restore
dotnet build
dotnet test
```

All commands should complete successfully before considering a change ready for review.

## Development workflow

UserHub is developed in small, reviewable stories. Each story should:

1. Have a clear objective and acceptance criteria.
2. Be implemented without prematurely adding work from later stories.
3. Be built and tested locally.
4. Be submitted for code review with relevant command output and a short explanation of the implementation.

## Current development status

The project is being built incrementally. The initial focus is solution structure, dependency configuration, secure local database configuration, and Entity Framework Core infrastructure. Check the repository's latest commits and development notes for the current implementation status.

## License

No license has been specified yet. Unless a license is added to this repository, the source code is not granted an explicit open-source license.
