# SakenyApp

**SakenyApp** is a comprehensive property management and rental platform built with .NET Core, following Onion Architecture principles. It provides a scalable, maintainable, and testable backend API for managing real estate listings, user interactions, and notifications.

---

## Table of Contents

* [Overview](#overview)
* [Key Features](#key-features)
* [Architecture](#architecture)
* [Tech Stack](#tech-stack)
* [Project Structure](#project-structure)
* [Prerequisites](#prerequisites)
* [Installation & Setup](#installation--setup)
* [Running the Application](#running-the-application)
* [API Endpoints](#api-endpoints)
* [Testing](#testing)
* [Contributing](#contributing)
* [License](#license)
* [Contact](#contact)

---

## Overview

SakenyApp is designed to streamline the process of listing, searching, and managing rental properties. Administrators can add new properties and units, while tenants can search, favorite, and request notifications about changes or availability. The system employs clean architecture to separate concerns and ensure long-term maintainability.

## Key Features

* **User Management**: Registration, authentication (JWT), and role-based authorization.
* **Property Listings**: CRUD operations for properties and units (apartments, rooms).
* **Search & Filtering**: Query by location, price range, availability, and other attributes.
* **Favorites**: Users can mark properties or units as favorites.
* **Notifications**: Email/SMS notifications for listing updates or availability.
* **Audit Logging**: Track changes and user actions for transparency and debugging.
* **Paging & Sorting**: Efficient data retrieval with pagination and sorting options.

## Architecture

This project follows the **Onion Architecture** pattern, dividing the codebase into concentric layers with strict dependency rules:

1. **Core (Domain)**

   * Entities, Value Objects, and Domain Interfaces
2. **Application**

   * DTOs, Use Cases/Services, and Application Interfaces
3. **Infrastructure**

   * EF Core DbContext implementation, Repositories, External Services (Email/SMS)
4. **API**

   * RESTful controllers, request/response models, and middleware

## Tech Stack

* **Backend**: .NET Core 6.0 (or latest LTS)
* **ORM**: Entity Framework Core
* **Database**: SQL Server (configurable)
* **Authentication**: JWT Bearer Tokens
* **Logging**: Microsoft.Extensions.Logging
* **Notifications**: SendGrid (email), Twilio (SMS)
* **Testing**: xUnit, Moq

## Project Structure

```text
SakennyProject/
├── src/
│   ├── SakennyProject.Core/
│   ├── SakennyProject.Application/
│   ├── SakennyProject.Infrastructure/
│   └── SakennyProject.API/
├── tests/
│   ├── SakennyProject.UnitTests/
│   └── SakennyProject.IntegrationTests/
├── .gitignore
└── README.md
```

* **src/**: Main application source code divided into four layers.
* **tests/**: Unit and integration test projects.

## Prerequisites

* [.NET Core SDK 6.0+](https://dotnet.microsoft.com/download)
* [SQL Server](https://www.microsoft.com/sql-server) or compatible database
* [SendGrid account](https://sendgrid.com/) (for email notifications)
* [Twilio account](https://www.twilio.com/) (for SMS notifications)

## Installation & Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/OmarAnalog/SakenyApp.git
   cd SakenyApp/src/SakennyProject.API
   ```

2. **Configure Connection Strings**

   * In `appsettings.json`, update the `ConnectionStrings:DefaultConnection` to point to your SQL Server instance.

3. **Configure Notification Settings**

   * Set your SendGrid API key and Twilio credentials in the `appsettings.json` under `NotificationSettings`.

4. **Apply Migrations & Seed Data**

   ```bash
   cd ../SakennyProject.Infrastructure
   dotnet ef database update
   ```

## Running the Application

1. Navigate to the API project:

   ```bash
   cd src/SakennyProject.API
   ```
2. Run the project:

   ```bash
   dotnet run
   ```
3. The API will be available at `https://localhost:5001` (or configured URL).
4. Swagger UI is available at `https://localhost:5001/swagger` for exploring endpoints.

## API Endpoints

> **Note**: Replace `{API_URL}` with your running address (e.g., `https://localhost:5001`).

| Module        | Endpoint                  | Method | Description                   |
| ------------- | ------------------------- | ------ | ----------------------------- |
| Auth          | `/api/auth/register`      | POST   | Register a new user           |
| Auth          | `/api/auth/login`         | POST   | Authenticate and retrieve JWT |
| Properties    | `/api/properties`         | GET    | List all properties           |
| Properties    | `/api/properties/{id}`    | GET    | Get property details by ID    |
| Properties    | `/api/properties`         | POST   | Create a new property         |
| Properties    | `/api/properties/{id}`    | PUT    | Update existing property      |
| Properties    | `/api/properties/{id}`    | DELETE | Delete a property             |
| Favorites     | `/api/users/{userId}/fav` | GET    | List user favorites           |
| Favorites     | `/api/users/{userId}/fav` | POST   | Add item to favorites         |
| Notifications | `/api/notifications`      | POST   | Send a custom notification    |

## Testing

* **Unit Tests**: Run from the `tests/SakennyProject.UnitTests` folder:

  ```bash
  dotnet test tests/SakennyProject.UnitTests
  ```

* **Integration Tests**: Run from the `tests/SakennyProject.IntegrationTests` folder:

  ```bash
  dotnet test tests/SakennyProject.IntegrationTests
  ```

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/YourFeature`.
3. Commit your changes: `git commit -m "Add your feature"`.
4. Push to the branch: `git push origin feature/YourFeature`.
5. Open a Pull Request.

Please ensure that your code follows existing style conventions and includes tests where applicable.

## License

This project is licensed under the [MIT License](LICENSE).

## Contact

Created by Omar Analog. For questions or feedback, please reach out at \[[the.eoa96@gmail.com](mailto:the.eoa96@gmail.com)].
