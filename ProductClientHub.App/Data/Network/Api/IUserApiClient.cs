using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using Refit;

namespace ProductClientHub.App.Data.Network.Api;

public interface IUserApiClient
{
    // Auth
    [Post("/api/Auth/register")]
    Task<ResponseRegisteredUserJson> Register([Body] RequestRegisterUserJson request);

    [Post("/api/Auth/login")]
    Task<ResponseTokenJson> Login([Body] RequestLoginJson request);

    // 204 No Content
    [Post("/api/Auth/logout")]
    Task Logout();

    // Clients (requires Authorization)
    [Post("/api/Clients")]
    Task<ResponseShortClientJson> RegisterClient([Body] RequestClientJson request);

    // 202 Accepted (empty body)
    [Put("/api/Clients/{id}")]
    Task UpdateClient(Guid id, [Body] RequestClientJson request);

    // 200 OK with body or 204 No Content
    [Get("/api/Clients")]
    Task<ApiResponse<ResponseAllClientsJson>> GetAllClients();

    [Get("/api/Clients/{id}")]
    Task<ResponseClientJson> GetClientById(Guid id);

    // 200 OK (empty body)
    [Delete("/api/Clients/{id}")]
    Task DeleteClient(Guid id);

    // Products (requires Authorization)
    [Post("/api/Products/{clientId}")]
    Task<ResponseShortProductJson> RegisterProduct(Guid clientId, [Body] RequestProductJson request);

    // 200 OK (empty body)
    [Delete("/api/Products/{id}")]
    Task DeleteProduct(Guid id);
}
