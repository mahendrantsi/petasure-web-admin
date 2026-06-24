# Petasure Admin Web Project

This repository contains the admin web backend and supporting .NET projects for the Petasure platform.

## Structure

- `Project.Web/` — ASP.NET Core web application for the admin interface
- `Project.WebAPI/` — API backend services for admin and integration scenarios
- `Project.Core/` — Core business logic and shared utilities
- `Project.Services/` — Service layer implementations
- `Project.Persistence/` — Database access and persistence logic
- `Project.Models/` — Shared data models and contracts
- `petasureAdmin.sln` — Visual Studio solution file

## Getting Started

1. Open `petasureAdmin.sln` in Visual Studio or Rider.
2. Restore NuGet packages.
3. Build the solution.
4. Set `Project.Web` as the startup project.
5. Run the web app.

## Development

- Build output is generated under `bin/` and `obj/` directories.
- The project uses `appsettings.json` for configuration.
- Use `libman.json` for static library management if needed.

## Notes

- Ignore files and build artifacts are managed in `.gitignore`.
- Keep any user-specific IDE files out of source control.
- For Azure or publish steps, configure publish profiles in Visual Studio and avoid committing secret configuration values.
