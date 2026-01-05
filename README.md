# Backend Auth Template

This project is a robust and feature-rich backend template built with .NET 8, designed to serve as a comprehensive starting point for modern web applications. It showcases a clean, maintainable, and scalable architecture, incorporating a wide range of best practices and advanced features. This template is ideal for developers looking to build secure, high-performance applications while adhering to industry-standard design patterns.

## Key Features

*   **Clean Architecture**: The project follows the principles of Clean Architecture, separating concerns into distinct layers (Domain, Application, Infrastructure, and API). This results in a codebase that is easy to understand, maintain, and extend.
*   **Authentication and Authorization**: A complete authentication system is implemented using JWTs (JSON Web Tokens), with secure refresh tokens to ensure a seamless and secure user experience.
*   **Comprehensive Testing**: The template includes a full suite of tests, including unit, integration, and functional tests, to ensure the reliability and correctness of the codebase.
*   **Advanced Data Handling**:
    *   **Include System**: A flexible system for including related data in API responses, allowing clients to request the exact data they need.
    *   **Sorting System**: A generic sorting system that supports sorting on nested properties.
    *   **Pagination**: A paginated list system for efficiently handling large datasets.
*   **Email System with Blazor Rendering**: A powerful email system that uses Blazor for rendering email templates, enabling the creation of dynamic and visually rich emails.
*   **API Versioning**: The API is versioned to allow for future enhancements without breaking existing client integrations.
*   **Real-time Communication**: SignalR hubs are integrated for real-time communication between the server and clients.
*   **CI/CD Pipeline**: A complete CI/CD pipeline is set up to automate the build, testing, and deployment processes.
*   **Robust Error Handling**: A centralized exception handling system provides consistent and informative error responses.

## Templates

This project includes a set of templates to accelerate development by scaffolding common features.

### Feature Template

The Feature Template is used to create a new feature, including the entity, API controller, commands, queries, and tests.

**Usage:**

1.  **Copy the Template**: Copy the `templates/FeatureTemplate` directory and rename it to match your new feature (e.g., `Products`).
2.  **Rename Placeholders**:
    *   Rename `FeatureName` to your feature's name (e.g., `Products`).
    *   Rename `EntityName` to your entity's name (e.g., `Product`).
3.  **Update `AppDbContext`**:
    *   Open `src/Infrastructure/Data/AppDbContext.cs` and add a `DbSet` for your new entity:
        ```csharp
        public DbSet<Product> Products { get; set; }
        ```
    *   Open `src/Application/Common/Interfaces/IAppDbContext.cs` and add the `DbSet` to the interface:
        ```csharp
        DbSet<Product> Products { get; }
        ```
4.  **Run Migrations**:
    *   Run the following commands to create and apply the database migration:
        ```bash
        dotnet ef migrations add "Add Product Entity"
        dotnet ef database update
        ```

### CQRS-Command Template

The CQRS-Command Template is used to add a new command to an existing feature.

**Usage:**

1.  **Copy the Template**: Copy the `templates/CommandTemplate` directory into your feature's directory (e.g., `src/Application/Features/Products`).
2.  **Rename Placeholders**:
    *   Rename `CommandName` to your command's name (e.g., `ArchiveProduct`).
3.  **Update Test Helper**:
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

1.  **Copy the Template**: Copy the `templates/QueryTemplate` directory into your feature's directory (e.g., `src/Application/Features/Products`).
2.  **Rename Placeholders**:
    *   Rename `QueryName` to your query's name (e.g., `GetProductBySku`).
3.  **Update Test Helper**:
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
