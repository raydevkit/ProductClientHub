using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using Refit;

namespace ProductClientHub.App.Data.Network.Api;

public interface IUserApiClient
{
    [Post("/api/Auth/register")]
    Task<ResponseRegisteredUserJson> Register([Body]RequestRegisterUserJson request);
}
