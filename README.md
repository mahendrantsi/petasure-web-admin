# Petasure Web & API — README

Petasure is a lost-and-found pet platform for dogs and cats that combines a conventional pet registry with AI-based biometric recognition (dog nose prints and cat face matching). This repository, `petasure-web-admin`, holds the complete .NET 8 back-end solution: a JWT-secured REST API (`Project.WebAPI`) consumed by the mobile apps and integrations, and a cookie-authenticated ASP.NET Core MVC admin portal (`Project.Web`) used by staff to manage pets, missing-pet reports, users, content, and subscriptions. Both host applications share a single layered library stack and the same SQL Server database via EF Core.

This README is the entry point for new developers. For deeper topics, see the sibling docs linked at the bottom.

---

## Tech Stack

| Area | Technology |
| --- | --- |
| Runtime / language | .NET 8 (`net8.0`), C# |
| API host | ASP.NET Core 8 Web API (`Project.WebAPI`) — classic `Startup`/`IHostBuilder` pattern |
| Web host | ASP.NET Core 8 MVC + Razor (`Project.Web`) — Areas, runtime view compilation |
| Authentication | JWT Bearer (HS256) for the API; cookie-based ASP.NET Core Identity for the web portal |
| Identity | ASP.NET Core Identity with custom `DerivedIdentityUser` and `IdentityRole<Guid>` |
| Data access | EF Core 8 (SQL Server provider), repository + Unit of Work |
| Database | SQL Server |
| Mapping | AutoMapper 13 |
| API docs | Swagger / Swashbuckle 6.8 |
| Logging | NLog (via `Project.Logger`) |
| UI extras (web) | NToastNotify, X.PagedList, jQuery DataTables, Highcharts |
| External services | Dog AI / Cat AI recognition APIs, Shopify + Recharge (subscriptions), SendGrid + SMTP (email), Azure Blob Storage, Firebase Cloud Messaging |
| Code quality | StyleCop.Analyzers 1.1.118 (enforced solution-wide via `Directory.Build.props`) |

---

## Solution Structure

The solution file is `Project.sln`. It contains nine C# projects organised into solution folders (App, Data, DomainLogic, Logger).

| Project | Role |
| --- | --- |
| `Project.WebAPI` | REST API host — JWT auth, Swagger, runs EF migrations at startup, wraps `/api` responses in a standard envelope. |
| `Project.Web` | MVC/Razor admin portal — cookie Identity auth, role-gated Areas (Admin, User), server-rendered HTML. |
| `Project.Services` | Business-logic service layer; every service returns `ServiceResponse<T>`. Hosts the Dog/Cat AI HTTP integration and email dispatch. |
| `Project.Persistence` | Repository + Unit of Work over EF Core (`IUnitOfWork`, generic and typed repositories). |
| `Project.Data` | EF Core entities, `ProjectDbContext` (IdentityDbContext), Fluent API config, and migrations. |
| `Project.Models` | Shared DTOs / ViewModels and API request-response contracts. No business logic. |
| `Project.Core` | Cross-cutting utilities: enums, validation attributes, crypto/hashing helpers, date/string extensions, message constants. |
| `Project.Middleware` | API middleware: common response wrapping (`CommonResponseMiddleware`), request-duration header, validation filters. |
| `Project.Logger` | NLog-backed logging abstraction (`ILoggerManager`), request logging, and an EF Core SQL command interceptor. |

> Note: A few orphaned/backup `.csproj` files (e.g. `KnightPay - Backup.*`, `Project.Library`, `Project.BCAccounts`) exist on disk from a prior template and are **not** referenced in `Project.sln`. Ignore them.

### Layering

```
Project.Web  /  Project.WebAPI   (host apps)
        |
   Project.Services              (business logic, ServiceResponse<T>)
        |
   Project.Persistence           (UnitOfWork + repositories)
        |
   Project.Data                  (EF Core DbContext + entities)

   Project.Models, Project.Core, Project.Logger, Project.Middleware  -> shared by all layers
```

---

## Prerequisites

- **.NET 8 SDK** (`dotnet --version` should report 8.x)
- **SQL Server** (e.g. SQL Server Express or LocalDB) reachable from your machine
- **EF Core tools** for migrations: `dotnet tool install --global dotnet-ef` (or the Package Manager Console `Update-Database` cmdlets)
- **Visual Studio 2022** (17.7+) or **JetBrains Rider** — optional but recommended; the CLI works too
- Access credentials for any external integrations you intend to exercise (Dog/Cat AI, Recharge, SendGrid, Azure Blob, FCM). The app runs without them, but the related features will fail.

---

## Quick Start

### 1. Clone and restore

```bash
git clone <repo-url>
cd petasure-web-admin
dotnet restore Project.sln
```

### 2. Configure `appsettings.json`

Both host apps read configuration from their own `appsettings.json` (`Project.WebAPI/appsettings.json` and `Project.Web/appsettings.json`). At minimum, set the database connection string. The connection-string key is **`ConnectionStrings:ProjectDbConnection`** and both apps share the same `ProjectDbContext` / database.

```jsonc
{
  "ConnectionStrings": {
    "ProjectDbConnection": "Server=<DB_SERVER>;Database=Petasure;Integrated Security=True;TrustServerCertificate=True;"
  },
  "jwtTokenConfig": {
    "secret": "<JWT_SECRET>",
    "issuer": "https://mywebapi.com",
    "audience": "https://mywebapi.com",
    "accessTokenExpiration": 30000,
    "refreshTokenExpiration": 43200
  },
  "Email": {
    "SmtpUser": "<SMTP_USER>",
    "SmtpHost": "<SMTP_HOST>",
    "SmtpPort": "587",
    "SmtpPassword": "<SMTP_PASSWORD>",
    "EnableSsl": "true"
  },
  "CustomKeys": {
    "pathBase":"",
    "BaseUrl": "<BASE_URL>",
    "DogRequestUrl": "<DOG_AI_URL>",
    "DogRequestApiKey": "<DOG_AI_KEY>",
    "CatRequestUrl": "<CAT_AI_URL>",
    "CatRequestApiKey": "<CAT_AI_KEY>",
    "PetaSupportEmail": "<SUPPORT_EMAIL>",
    "RechargeUrl": "https://api.rechargeapps.com/",
    "RechargeApiKey": "<RECHARGE_API_KEY>",
    "RechargeApiVersion": "2021-11"
  },
  "StorageSetting": {
    "StorageURL": "<AZURE_BLOB_URL>",
    "Container": "rootcontainer"
  },
  "NotificationKeys": {
    "ServerKey": "<FCM_SERVER_KEY>",
    "SenderID": "<FCM_SENDER_ID>"
  }
}
```

> **Never commit real secrets.** Use placeholders in committed files and supply real values via environment variables, ASP.NET Core User Secrets, or your deployment platform's secret store. The API project has a `UserSecretsId` configured for local development. See [SECURITY.md](SECURITY.md) for the full configuration-and-secrets policy.
>
> Reference key groups (full list in [SECURITY.md](SECURITY.md)): `ConnectionStrings:ProjectDbConnection`, `jwtTokenConfig.*`, `Email.*`, `CustomKeys.*`, `StorageSetting.*`, `NotificationKeys.*`, `DBSettings.DBInterceptor`, `WebProjectRootPath`.

### 3. Apply database migrations

`Project.WebAPI` calls `db.Database.Migrate()` automatically at startup (inside a try/catch that only logs failures), so running the API against an empty database will create the schema. `Project.Web` does **not** auto-migrate. To apply migrations manually:

```bash
# from the repo root
dotnet ef database update --project Project.Data --startup-project Project.WebAPI
```

Or, in the Visual Studio Package Manager Console (Default project = `Project.Data`):

```powershell
Update-Database
```

### 4. Run the API (`Project.WebAPI`)

```bash
dotnet run --project Project.WebAPI
```

Swagger UI is served at the **application root** (`RoutePrefix` is empty), so once the app is listening, open the base URL in a browser (for example `https://localhost:<port>/`) and you'll land directly on the Swagger page. All API routes use the `api/[controller]` pattern; controllers live under `Project.WebAPI/Controllers/V1/` (folder-based versioning — there is no `/v1/` URL segment).

To authorise calls in Swagger: register/login via `api/Account`, copy the returned JWT access token, click **Authorize**, and paste the token.

### 5. Run the admin portal (`Project.Web`)

```bash
dotnet run --project Project.Web
```

The default route lands unauthenticated visitors on **`Account/Login`**. After login, users are routed by role: Admin/SubAdmin to `/Admin/Dashboard/Index`, AnonymousUser (e.g. a vet/scanner) to `/Admin/AnonymousUser/Index`. The admin screens live in the `Admin` area under the route pattern `{area:exists}/{controller=Home}/{action=Index}/{id?}`.

> The two hosts are independent processes. Run them separately (or set multiple startup projects in your IDE). For end-to-end work, run the API and the web portal at the same time.

---

## Documentation Map

| Document | Purpose |
| --- | --- |
| [ARCHITECTURE.md](ARCHITECTURE.md) | Layered design, project dependencies, middleware pipeline, auth flows. |
| [DEVELOPMENT.md](DEVELOPMENT.md) | Local setup, building, running, migrations, conventions, gotchas. |
| [API.md](API.md) | REST endpoints, route conventions, request/response envelope, controllers. |
| [DATABASE.md](DATABASE.md) | Schema, entities, relationships, migration history. |
| [DEPLOYMENT.md](DEPLOYMENT.md) | Build, publish, and environment configuration for staging/production. |
| [SECURITY.md](SECURITY.md) | Auth model, secrets handling, configuration keys, known security risks. |
| [TROUBLESHOOTING.md](TROUBLESHOOTING.md) | Common errors and fixes (migrations, config, integrations). |
| [CONTRIBUTING.md](CONTRIBUTING.md) | Branching, coding standards, StyleCop, PR process. |
| [CHANGELOG.md](CHANGELOG.md) | Notable changes per release/branch. |
