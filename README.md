# Backend Auth Template

This project is a robust and feature-rich backend template built with .NET 8, designed to serve as a comprehensive starting point for modern web applications. It showcases a clean, maintainable, and scalable architecture, incorporating a wide range of best practices and advanced features. This template is ideal for developers looking to build secure, high-performance applications while adhering to industry-standard design patterns.

This template is designed to work with the [Frontend Auth Intl Template](https://github.com/titanfrigel/frontend-auth-intl-template), but it can also be used as a standalone backend.

## Key Features

*   **Clean Architecture**: The project follows the principles of Clean Architecture, separating concerns into distinct layers (Domain, Application, Infrastructure, and API). This results in a codebase that is easy to understand, maintain, and extend.
*   **CQRS Pattern**: Implements the Command Query Responsibility Segregation (CQRS) pattern to separate read and write operations, leading to a more organized and scalable application logic.
*   **Authentication and Authorization**: A complete authentication system is implemented using JWTs (JSON Web Tokens), with secure refresh tokens to ensure a seamless and secure user experience.
*   **Comprehensive Testing**: The template includes a full suite of tests, including unit, integration, and functional tests, to ensure the reliability and correctness of the codebase.
*   **Advanced Data Handling**:
    *   **Include System**: A flexible system for including related data in API responses, allowing clients to request the exact data they need.
    *   **Sorting System**: A generic sorting system that supports sorting on nested properties.
    *   **Pagination**: A paginated list system for efficiently handling large datasets.
*   **Database Management**: Uses Entity Framework Core for data access and provides a streamlined migration process.
*   **Email System with Blazor Rendering**: A powerful email system that uses Blazor for rendering email templates, enabling the creation of dynamic and visually rich emails.
*   **API Versioning**: The API is versioned to allow for future enhancements without breaking existing client integrations.
*   **Real-time Communication**: SignalR hubs are integrated for real-time communication between the server and clients.
*   **CI/CD Pipeline**: A complete CI/CD pipeline is set up to automate the build, testing, and deployment processes.
*   **Robust Error Handling**: A centralized exception handling system provides consistent and informative error responses.
*   **Code Generation**: Scaffolding templates for features, commands, and queries to accelerate development and ensure consistency.
*   **Containerization**: Docker support for easy deployment and a consistent development environment.

## Getting Started

To create a new project from this template, you'll first need to install the template from the root directory:

```bash
dotnet new install .
```

Once installed, you can create a new project with the following command:

```bash
dotnet new backend-auth -n YourProjectName
```

## Templates

This project includes a set of templates to accelerate development by scaffolding common features.

### Feature Template

The Feature Template is used to create a new feature, including the entity, API controller, commands, queries, and tests.

**Usage:**

```bash
dotnet new feature --FeatureName Products --EntityName Product
```

After creating the feature, you'll need to:

1.  **Update `AppDbContext`**:
    *   Open `src/Infrastructure/Data/AppDbContext.cs` and add a `DbSet` for your new entity:
        ```csharp
        public DbSet<Product> Products { get; set; }
        ```
    *   Open `src/Application/Common/Interfaces/IAppDbContext.cs` and add the `DbSet` to the interface:
        ```csharp
        DbSet<Product> Products { get; }
        ```
2.  **Run Migrations**:
    *   Run the following commands to create and apply the database migration:
        ```bash
        dotnet ef migrations add "Add Product Entity"
        dotnet ef database update
        ```

### CQRS-Command Template

The CQRS-Command Template is used to add a new command to an existing feature.

**Usage:**

```bash
dotnet new cqrs-command --FeatureName Products --EntityName Product --CommandName ArchiveProduct
```

After creating the command, you'll need to:

1.  **Update Test Helper**:
    *   Open the corresponding `CommandsTestHelper.cs` file (e.g., `tests/Tests.Common/Products/ProductsCommandsTestHelper.cs`).
    *   Add a new static method to create an instance of your new command:
        ```csharp
        public static ArchiveProductCommand ArchiveProductCommand(Guid? productId = null)
        {
            return new()
            {
                ProductId = productId ?? Guid.NewGuid()
            };
        }
        ```

### CQRS-Query Template

The CQRS-Query Template is used to add a new query to an existing feature.

**Usage:**

```bash
dotnet new cqrs-query --FeatureName Products --EntityName Product --QueryName GetProductBySku
```

After creating the query, you'll need to:

1.  **Update Test Helper**:
    *   Open the corresponding `QueriesTestHelper.cs` file (e.g., `tests/Tests.Common/Products/ProductsQueriesTestHelper.cs`).
    *   Add a new static method to create an instance of your new query:
        ```csharp
        public static GetProductBySkuQuery GetProductBySkuQuery(string sku = "DEFAULT-SKU")
        {
            return new()
            {
                Sku = sku
            };
        }
        ```

## Env Variables
All required environment variables can be found in `docker-compose.yml` or in `src/API/appsettings.Testing.json`.
