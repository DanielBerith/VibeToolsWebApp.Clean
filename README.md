# VibeTools Web App

**A Modern AI Tool Directory**

VibeTools is a sample web application that demonstrates a clean, maintainable architecture for building a directory of AI tools.  Users can browse, search, submit new tools, and post reviews with star ratings and comments. The directory marks the highest-rated tool as the community favorite and hides poorly rated tools.

---

## 📌 Table of Contents

* [Key Features](#-key-features)
* [Architecture Overview](#-architecture-overview)
* [Tech Stack](#-tech-stack)
* [Getting Started](#-getting-started)

  * [Prerequisites](#prerequisites)
  * [Clone & Setup](#clone--setup)
  * [Database Migrations](#database-migrations)
  * [Run the API](#run-the-api)
* [API Endpoints](#api-endpoints)
* [Frontend Integration](#frontend-integration)
* [Testing](#testing)
* [Project Conventions](#project-conventions)
* [Contributing](#contributing)
* [License](#license)

---

## 🔑 Key Features

* **Browse & Search**: View all tools or filter by name/tags.
* **Tool Details**: Drill down to see descriptions and reviews.
* **Submit Tools**: Add new AI tools with name, description, and category.
* **Post Reviews**: Rate tools (1-5 stars) and leave comments.
* **Community Favorite**: Automatically flags the tool(s) with the highest average rating.
* **Hide Poor Tools**: Tools with five consecutive 1‑star reviews are hidden from listings.
* **Robust Validation**: Both client and server side validation with clear error messages.

---

## 🏗️ Architecture Overview

VibeTools follows **Clean Architecture** augmented by **CQRS** to achieve separation of concerns and high testability. Each layer has explicit responsibilities and only depends on the layer directly below it.

```text
VibeToolsWebApp
├── Domain         # Core business entities & rules
│   └── Entities
│       ├── Tool.cs       # `RecalculateRating`, hide logic
│       └── Review.cs     # Rating, timestamp
│
├── Application    # Use cases: commands, queries, interfaces, mapping
│   ├── Features
│   │   └── Tools
│   │       ├── Commands/CreateTool
│   │       │   ├── CreateToolCommand.cs
│   │       │   └── CreateToolCommandHandler.cs
│   │       └── Queries
│   │           ├── GetToolsQuery.cs
│   │           ├── GetToolsQueryHandler.cs
│   │           ├── GetToolDetailsQuery.cs
│   │           └── GetToolDetailsQueryHandler.cs
│   ├── Interfaces
│   │   ├── IToolRepository.cs
│   │   └── IReviewRepository.cs
│   ├── MappingProfile
│   │   ├── ToolsProfile.cs
│   │   └── ReviewsProfile.cs
│   └── ApplicationServicesRegistration.cs
│
├── Persistence    # EF Core context, entity configurations, repositories
│   ├── DatabaseContext
│   │   └── VibeToolsDatabaseContext.cs   # DbSet<Tool>, DbSet<Review>
│   ├── Repositories
│   │   ├── ToolRepository.cs             # implements IToolRepository
│   │   └── ReviewRepository.cs           # implements IReviewRepository
│   └── PersistenceServicesRegistration.cs
│
└── API            # ASP.NET Core Web API: controllers, middleware, DI
    ├── Controllers
    │   ├── ToolsController.cs            # /api/tools endpoints
    │   └── ReviewsController.cs          # /api/reviews endpoints
    ├── Program.cs                        # composition root: DI, MediatR, AutoMapper, EF Core
    └── appsettings.json                  # connection string, logging
```

### Layer Responsibilities

#### Domain Layer

* **Entities** encapsulate business rules and invariants (e.g., `Tool.RecalculateRating`).
* No external dependencies—fully testable in isolation.

#### Application Layer

* **CQRS** via MediatR: Commands for writes, Queries for reads.
* **Handlers** coordinate work: call repository interfaces, apply business logic, map to/from DTOs, log events.
* **DTOs** (`GetToolsDto`, `GetReviewsDto`, etc.) define the shape of data sent to clients.
* **Interfaces** define abstractions (`IToolRepository`, `IReviewRepository`) to decouple from EF Core.

#### Persistence Layer

* **`VibeToolsDatabaseContext`** configures EF Core mappings and relationships.
* **Repositories** implement application interfaces using EF Core, enabling change persistence.
* **Migrations** track schema evolution; initial migration creates `Tools` and `Reviews` tables.

#### API Layer

* **Controllers** bind HTTP requests to MediatR commands/queries and return HTTP responses.
* **Error Handling**: Business exceptions mapped to `400 Bad Request` with structured JSON errors.
* **Dependency Injection**: Registers DbContext, repositories, MediatR, AutoMapper, and controllers in `Program.cs`.
* **Swagger/OpenAPI**: Exposes interactive API documentation for testing.

By strictly enforcing dependencies (outer → inner), VibeTools ensures that changes in the presentation or persistence layers won’t ripple into core business logic, and vice versa.

---

## 🧰 Tech Stack

| Layer       | Technology                                    |
| ----------- | --------------------------------------------- |
| Language    | C# (.NET 8+)                                  |
| API         | ASP.NET Core Web API, MediatR                 |
| Mapping     | AutoMapper                                    |
| Data Access | Entity Framework Core (SQL Server/In-Memory)  |
| Frontend    | React, React Router, React Query (TypeScript) |
| Testing     | xUnit, Moq, EF Core In-Memory Provider        |
| Container   | Docker, docker-compose (optional)             |

---

## 🚀 Getting Started

### Prerequisites

* [.NET 6 SDK or later](https://dotnet.microsoft.com/download)
* SQL Server LocalDB or full SQL Server (or use In-Memory for tests)
* Node.js & npm (for frontend)

### Clone & Setup

```bash
git clone https://github.com/your-org/vibetools-webapp.git
cd vibetools-webapp
```

1. **Configure Connection String**

   * Edit `VibeToolsWebApp.API/appsettings.json`

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=VibeToolsDb;Trusted_Connection=True;"
     }
   }
   ```

2. **Restore & Build**

   ```bash
   dotnet restore
   dotnet build
   ```

### Database Migrations

Use EF Core tools to create and apply migrations:

```bash
cd VibeToolsWebApp.Persistence
dotnet ef migrations add Initial --startup-project ../VibeToolsWebApp.API
dotnet ef database update   --startup-project ../VibeToolsWebApp.API
```

### Run the API

```bash
cd VibeToolsWebApp.API
dotnet run
```

The API listens on `https://localhost:5001` and `http://localhost:5000`.

---

## 📡 API Endpoints

### Tools

* **GET** `/api/tools?search={term}`

  * Returns list of `GetToolsDto` with `Id`, `Name`, `Category`, `AverageRating`, `IsCommunityFavorite`.
* **GET** `/api/tools/{id}`

  * Returns `GetToolDetailsDto` including all reviews.
* **POST** `/api/tools`

  * Accepts `CreateToolCommand { Name, Description, Category }`
  * **400** on validation or duplicate name: `{ error: "..." }`
  * **201** returns `{ id: "new-tool-id" }`

### Reviews

* **GET** `/api/reviews?toolId={toolId}`

  * Returns list of `GetReviewsDto { Id, Comment, Rating, CreatedAt }`.
* **POST** `/api/reviews`

  * Accepts `CreateReviewCommand { ToolId, Author, Comment, Rating }`
  * **400** on invalid rating or missing tool: `{ error: "..." }`
  * **201** returns `{ id: "new-review-id" }`

---

## 🖥️ Frontend Integration

The React frontend consumes these endpoints via a `toolsApi` client using Axios and React Query:

```ts
// Example: fetch tools
useQuery([ 'tools', search ], () => toolsApi.getTools(search).then(r => r.data));
```

See the `pages/` and `components/` folders for examples of listing, searching, and submitting forms.

---

## 🧪 Testing

Run unit and integration tests against an in-memory database:

```bash
cd VibeToolsWebApp.Tests
dotnet test
```

* **Unit Tests**: Validate handlers, services, and mapping.
* **Integration Tests**: Verify EF Core repositories and full request pipelines.

---

## 🔧 Project Conventions

* **Solution structure** matches Clean Architecture layers.
* **Features** grouped under `/Commands` and `/Queries`.
* **Mapping** handled centrally via AutoMapper profiles.
* **Logging** injected into handlers for observability.
* **Error handling**: domain exceptions mapped to HTTP 400.

---

## 🤝 Contributing

1. Fork the repo and create a new branch.
2. Implement your feature or bug fix.
3. Add tests for new behavior.
4. Open a Pull Request against `main`.

Please follow the existing style: keep handlers slim, write clear commit messages, and document new endpoints.

---

## 📜 License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

---
