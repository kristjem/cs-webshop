# CS Webshop

A full-stack e-commerce application built with **ASP.NET Core, C#, Next.js, TypeScript, and SQL Server**.

The project demonstrates the development of a modern web application with a REST API, relational database, authentication, product management, shopping cart, and order processing.

Development is incremental, with an emphasis on clean architecture, maintainability, security, and practical use of the frameworks.

**Project status:** Early development. The ASP.NET Core API and containerized development environment are operational. Additional features are planned.

## 1. Technology Stack

| Component | Technology |
|---|---|
| Backend | C# / ASP.NET Core Web API |
| Runtime | .NET 9 |
| Frontend | Next.js / React |
| Frontend language | TypeScript |
| Database | SQL Server 2022 Developer |
| ORM | Entity Framework Core 9 (planned) |
| Authentication | ASP.NET Core Identity (planned) |
| API | REST / JSON |
| Development environment | VS Code / Dev Containers |
| Containerization | Docker / Docker Compose |
| Version control | Git |

The project currently targets .NET 9. Framework support and upgrade requirements should be reviewed before production deployment.

## 2. Architecture

The application is organized in a single repository containing a separate frontend and backend.

```text
                  Browser
                     |
                     v
              Next.js / React
                  Frontend
                     |
                  HTTP/JSON
                     |
                     v
               ASP.NET Core
                  Web API
                     |
                     v
             Entity Framework Core
                     |
                     v
                 SQL Server
```

### Backend

The backend is an ASP.NET Core Web API application using controller-based routing.

Its responsibilities will include business logic, request validation, authentication, authorization, and database access.

### Frontend

The frontend will be developed with Next.js, React, and TypeScript.

It will communicate with the backend through the API rather than accessing the database directly.

### Database

SQL Server is used as the relational database.

Entity Framework Core will provide object-relational mapping, database migrations, and model configuration.

### Application design

The backend is designed as a monolithic application with a single database. Frontend and backend run as separate processes during development.

The architecture favors straightforward components and clear responsibilities over unnecessary abstraction or microservices.

## 3. Repository Structure

```text
cs-webshop/
├── .devcontainer/
│   ├── Dockerfile
│   ├── compose.yaml
│   ├── devcontainer.json
│   └── .env                  # Local, not committed
│
├── back-end/
│   ├── Controllers/
│   ├── Properties/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── Webshop.Api.csproj
│
├── front-end/                # Planned Next.js application
├── tests/                    # Planned automated tests
│
├── .gitignore
├── Webshop.sln
└── README.md
```

Additional backend directories, including models, DTOs, services, data access, and migrations, will be introduced as the application develops.

## 4. Development Environment

The development environment uses VS Code, Dev Containers, and Docker Compose.

Docker Compose defines two services:

| Service | Description |
|---|---|
| `app` | Development container with .NET SDK and Node.js |
| `sqlserver` | SQL Server 2022 Developer database |

The repository is mounted inside the development container at `/workspace`.

SQL Server uses a named Docker volume to preserve database files across container restarts.

### Prerequisites

- Git
- Visual Studio Code
- Dev Containers extension
- Docker Engine with Docker Compose
- WSL2 with a Linux distribution when developing on Windows

The .NET SDK and Node.js are provided by the development container.

### Getting Started

1. Clone the repository.
2. Create `.devcontainer/.env` with a strong database password.
3. Open the repository in VS Code.
4. Build the development image if necessary.
5. Run **Dev Containers: Reopen in Container**.

Example `.devcontainer/.env`:

```dotenv
MSSQL_SA_PASSWORD=<your-strong-password>
```

Never commit this file or real credentials to version control.

### Development image

The current Compose configuration references a locally built development image:

```yaml
services:
  app:
    image: devcontainer-app:latest
```

Build the image from the repository root:

```bash
docker build \
  -t devcontainer-app:latest \
  -f .devcontainer/Dockerfile \
  .devcontainer
```

This is a temporary workaround for a compatibility issue encountered with the Dev Containers build process. The setup may be simplified later.

## 5. Running the Backend

The ASP.NET Core project is located in `back-end/` and registered in `Webshop.sln`.

### Verify the SDK

```bash
dotnet --version
```

### Build the solution

From the repository root:

```bash
dotnet build Webshop.sln
```

### Start the API

```bash
cd back-end
dotnet run --urls http://0.0.0.0:5000
```

The API will be available at:

`http://localhost:5000`

### Example endpoint

The initial ASP.NET Core template includes:

`GET http://localhost:5000/weatherforecast`

This endpoint returns sample JSON data and confirms that the API is running correctly.

It will be replaced by application-specific endpoints.

## 6. Database Configuration

SQL Server 2022 Developer runs as a separate Docker container.

For local development, the database is exposed on port `1433`, bound to the host loopback interface.

### Connecting with SQL Server Management Studio

| Setting | Value |
|---|---|
| Server | `localhost,1433` |
| Authentication | SQL Server Authentication |
| Username | `sa` |
| Password | Value configured in `.devcontainer/.env` |

Trusting the server certificate may be necessary in the local development environment.

### Backend database connection

Within the Docker Compose network, the backend will use the hostname `sqlserver`.

Database credentials must be supplied through configuration or environment variables rather than hardcoded in source files.

### Entity Framework Core

Planned database functionality includes:

- A dedicated `DbContext`.
- Entity classes and relationship configuration.
- Database constraints and indexes.
- Schema management through EF Core migrations.
- Automatic migration application in the local development environment.
- Repeatable initialization of sample data.

Database integration has not yet been implemented in the API.

## 7. Domain Model

The application will support the following core entities:

| Entity | Responsibility |
|---|---|
| User | Accounts, profile information, and authentication |
| Role | Authorization and access levels |
| Membership | Membership tiers and discounts |
| Product | Product details, pricing, and inventory |
| Brand | Product brands |
| Category | Product classification |
| Order | Shopping carts and placed orders |
| OrderItem | Products, quantities, and pricing within orders |

### Domain Design Considerations

**Shopping carts:** A shopping cart may be represented as an order with a draft or cart status, reducing duplication between cart and order models.

**Historical pricing:** Order items should retain relevant price and discount snapshots so historical orders remain accurate after product prices change.

**Inventory:** Checkout operations must validate stock availability and handle concurrent updates appropriately.

**Product deletion:** Soft deletion may be used to hide products while preserving references from historical orders.

**Data integrity:** Unique constraints, foreign keys, and appropriate database constraints will protect relational consistency.

The final entity model and relationships will be established during implementation.

## 8. Authentication and Authorization

ASP.NET Core Identity is planned for account and role management.

The authentication system will support:

- Registration using a unique email address.
- Secure password hashing.
- User login and logout.
- Role-based authorization.
- Administrative access to protected functionality.

The authentication mechanism between Next.js and ASP.NET Core will be designed to avoid exposing sensitive credentials or tokens unnecessarily.

Authentication has not yet been implemented.

## 9. Planned API Features

The API is expected to provide functionality for:

- User registration and authentication.
- User and role administration.
- Product management.
- Brands and categories.
- Product search and filtering.
- Membership tiers and discounts.
- Shopping cart management.
- Checkout and order processing.
- Order history.
- Inventory management.
- Development data initialization.

Routes, DTOs, validation rules, and response formats will be defined as each feature is implemented.

OpenAPI documentation is also planned.

## 10. Sample Data Initialization

A repeatable initialization mechanism will be developed to populate the database with sample records.

It should:

- Create predefined roles and membership tiers.
- Populate sample categories, brands, and products.
- Avoid duplicate records when executed repeatedly.
- Be restricted to the development environment or otherwise appropriately secured.

The initialization mechanism is not yet implemented.

## 11. Testing

Automated tests will be added under `tests/`.

Planned test coverage includes:

- API request validation.
- Product and category operations.
- Authentication and authorization.
- Shopping cart operations.
- Order processing.
- Stock management.
- Discount calculations.
- Database integration.

The test framework and project structure will be selected during implementation.

## 12. Development Principles

The project follows several guiding principles.

**Incremental development:** Implement and validate features in small, manageable steps.

**Framework-first approach:** Prefer established ASP.NET Core and Entity Framework Core functionality over unnecessary custom implementations.

**Separation of concerns:** Keep HTTP handling, business rules, and persistence responsibilities clearly defined.

**Maintainability:** Favor readable code and understandable abstractions.

**Security:** Apply appropriate validation, authorization, secret management, and password-handling practices.

**Data integrity:** Use database constraints and transactions where necessary.

**Consistent naming:** Domain-specific C# identifiers may use Norwegian terminology, while framework conventions and standardized technical identifiers retain their established names.

**Testability:** Design components so important business rules can be verified through automated tests.

## 13. Development Roadmap

| Phase | Description | Status |
|---|---|---|
| 1 | Git repository, Docker, and Dev Container | Complete |
| 2 | ASP.NET Core Web API initialization | Complete |
| 3 | Next.js and TypeScript initialization | Planned |
| 4 | SQL Server integration and EF Core setup | Planned |
| 5 | Domain models and migrations | Planned |
| 6 | Sample data and product API | Planned |
| 7 | Authentication and authorization | Planned |
| 8 | Cart, discounts, and orders | Planned |
| 9 | Frontend integration and UI | Planned |
| 10 | Automated testing and documentation | Planned |

## 14. Current Status

The initial development environment is operational, and the ASP.NET Core API builds and runs successfully.

The frontend, database integration, domain entities, authentication, and e-commerce functionality are still under development.

---

**CS Webshop** — Full-stack e-commerce application under active development.
