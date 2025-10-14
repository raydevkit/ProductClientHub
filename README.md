# ProductClientHub

A full-stack sample solution for managing clients and products, composed of:
- ProductClientHub.API: ASP.NET Core Web API with JWT authentication, EF Core (Sqlite), FluentValidation, and Swagger.
- ProductClientHub.App: .NET MAUI app (Android, iOS, Windows, MacCatalyst) consuming the API via Refit with secure token handling.
- ProductClientHub.Communication: Shared request/response DTOs.
- ProductClientHub.Exceptions: Custom domain exceptions with HTTP mapping.

## Solution structure
- `ProductClientHub.API` (Target: .NET 10)
  - `Controllers`: `AuthController`, `ClientsController`, `ProductsController`
  - `UseCases`:
    - Auth: `RegisterUserUseCase`, `LoginUseCase`, `LogoutUseCase`
    - Clients: `RegisterClientUseCase`, `UpdateClientUseCase`, `GetAllClientsUseCase`, `GetClientByIdUseCase`, `DeleteClientUseCase`
    - Products: `RegisterProductUseCase`, `DeleteProductUseCase`
    - Validation: `RequestClientValidator`, `RequestProductValidator` (FluentValidation)
  - `Entities`: `Client`, `Product`, `User`, `EntityBase`
  - `Infrastructure`: `ProductClientHubDBContext` (Sqlite)
  - `Security`: `PasswordHasher` (PBKDF2)
  - `Services`: `JwtTokenService`
  - `Filters`: `ExceptionFilter` (maps domain exceptions to HTTP with `ResponseErrorMessagesJson`)
  - `Program.cs`: JSON converter, MVC + global `ExceptionFilter`, Swagger (Bearer), JWT validation

- `ProductClientHub.App` (Target: .NET 10 multi-target for Android, iOS, Windows, MacCatalyst)
  - `MauiProgram`: registers pages, navigation service, loads embedded `appsettings.json`, configures Refit clients, and use cases
  - Views/Pages: `OnboardingPage`, `LoginPage`, `SignUpPage`, `DashboardPage`
  - ViewModels: `OnboardingViewModel`, `LoginViewModel`, `SignUpViewModel`, `DashboardViewModel`, `ViewModelBase`
  - Navigation: `AppShell`, `DashboardShell`, `INavigationService`
  - Shells: `AppShell` (initial onboarding), `DashboardShell` (authenticated user with flyout menu and logout)
  - Data/Network: `IUserApiClient` (Refit), `AuthHeaderHandler`, `ITokenStore`/`SecureTokenStore`
  - UseCases (App-side):
    - Auth: `IRegisterUserUseCase`, `RegisterUserUseCase`, `ILoginUseCase`, `LoginUseCase`
    - Clients: `IGetAllClientsUseCase`, `GetAllClientsUseCase`, `IGetClientByIdUseCase`, `GetClientByIdUseCase`, `ICreateClientUseCase`, `CreateClientUseCase`, `IUpdateClientUseCase`, `UpdateClientUseCase`, `IDeleteClientUseCase`, `DeleteClientUseCase`
    - Products: `ICreateProductUseCase`, `CreateProductUseCase`, `IDeleteProductUseCase`, `DeleteProductUseCase`
  - Resources/Styles (including `Icons.xaml`) and platform-specific folders

- `ProductClientHub.Communication` (Target: .NET 8)
  - Requests: `RequestRegisterUserJson`, `RequestLoginJson`, `RequestClientJson`, `RequestProductJson`
  - Responses: `ResponseTokenJson`, `ResponseRegisteredUserJson`, `ResponseAllClientsJson`, `ResponseClientJson`, `ResponseShortClientJson`, `ResponseShortProductJson`, `ResponseErrorMessagesJson`

- `ProductClientHub.Exceptions` (Target: .NET 8)
  - Base: `ProductClientHubException`
  - Derived: `ErrorOnValidationException`, `NotFoundException`, `UnauthorizedException`

## Technologies
- .NET 10 (API, MAUI app), .NET 8 (class libraries)
- ASP.NET Core Web API, Swagger (OpenAPI) - Swashbuckle.AspNetCore 9.0.6
- JWT auth (`Microsoft.AspNetCore.Authentication.JwtBearer` 9.0.9, `System.IdentityModel.Tokens.Jwt` 8.14.0)
- EF Core 9.0.9 with Sqlite
- FluentValidation 12.0.0
- .NET MAUI + CommunityToolkit.Maui 12.2.0 + CommunityToolkit.Mvvm 8.4.0
- Refit 8.0.0 HTTP client with HttpClientFactory

## Architecture
- API follows a thin-controller, use-case oriented approach. Controllers call use cases directly.
- Validation is handled by FluentValidation in dedicated validators.
- Errors are converted to consistent JSON via `ExceptionFilter` returning `ResponseErrorMessagesJson`.
- Authentication uses JWT Bearer tokens; `JwtTokenService` issues tokens, `Program.cs` configures validation.
- The MAUI app follows MVVM pattern with use-case oriented business logic:
  - Each feature has dedicated use cases (interfaces + implementations)
  - ViewModels coordinate between views and use cases
  - Use cases consume the API via Refit
  - `AuthHeaderHandler` injects the bearer token from `SecureTokenStore`
  - Navigation is handled via `INavigationService` and Shell-based routing
  - Two shells: `AppShell` for unauthenticated flow, `DashboardShell` for authenticated users with flyout navigation

## Data model
- `Client`: `Id`, `Name`, `LastName`, `CodiceFiscale`, `Email`, `Products`
- `Product`: `Id`, `Name`, `Brand`, `Price`, `ClientId`
- `User`: `Id`, `Email`, `Name`, `PasswordSalt`, `PasswordHash`, `Role`

## API endpoints (summary)
- Auth
  - POST `/api/auth/register` → 201 Created with `ResponseRegisteredUserJson`
  - POST `/api/auth/login` → 200 OK with `ResponseTokenJson`
  - POST `/api/auth/logout` → 204 No Content
- Clients
  - POST `/api/clients` → 201 Created with `ResponseShortClientJson`
  - PUT `/api/clients/{id}` → 202 Accepted
  - GET `/api/clients` → 200 OK with `ResponseAllClientsJson` or 204 No Content (empty)
  - GET `/api/clients/{id}` → 200 OK with `ResponseClientJson`
  - DELETE `/api/clients/{id}` → 200 OK
- Products
  - POST `/api/products/{clientId}` → 201 Created with `ResponseShortProductJson`
  - DELETE `/api/products/{id}` → 200 OK

All endpoints (except `register` and `login`) require Bearer authentication (`[Authorize]` attribute).

## Running locally
### Prerequisites
- .NET SDKs for the targeted frameworks
  - .NET 10 SDK for API and MAUI app
  - .NET 8 SDK for class libraries
- .NET MAUI workloads installed for your platform targets

### Configuration
- API
  - JWT settings are read from configuration: `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`, optional `Jwt:AccessTokenMinutes`.
  - Database: `ProductClientHubDBContext` uses Sqlite with a hardcoded path: `F:\Workspace\ProductClientHub.db`. Adjust as needed.
- MAUI App
  - `ProductClientHub.App/appsettings.json` is embedded and loaded via `AddJsonStream`. Set `ApiUrl` to your API base URL.

### Start the API
- Set `ProductClientHub.API` as startup project and run.
- Swagger UI: `https://localhost:<port>/swagger`
  - Use the Authorize button to paste the JWT token acquired from `/api/auth/login`.

### Start the MAUI app
- Set `ProductClientHub.App` as startup project.
- Select target platform (Android, iOS, Windows, or MacCatalyst) and run.

## Error model
- On validation, not found, or authorization issues, the API returns:
  - `ResponseErrorMessagesJson` with an `Errors` array and appropriate HTTP status code.

## Notes and limitations
- The DB path is hardcoded in `ProductClientHubDBContext`.
- Use cases instantiate `ProductClientHubDBContext` directly (no DI). Consider refactoring to use dependency injection and `AddDbContext`.
- Migrations/seeding are not included.
- `LogoutUseCase` is a no-op (stateless JWT). Consider token blacklisting if needed.

## License
This project is licensed under the MIT License. See `LICENSE.txt` for details.