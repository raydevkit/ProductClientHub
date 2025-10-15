using ProductClientHub.App.Data.Auth;
using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.App.Services;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.App.UseCases.Auth.Login;

public interface ILoginUseCase
{
    Task Execute(string email, string password);
}

public class LoginUseCase(
    IUserApiClient apiClient,
    ITokenStore tokenStore,
    IApiErrorHandler errorHandler) : BaseUseCase(errorHandler), ILoginUseCase
{
    public async Task Execute(string email, string password)
    {
        await ExecuteWithErrorHandlingAsync(async () =>
        {
            var request = new RequestLoginJson { Email = email, Password = password };
            var token = await apiClient.Login(request);
            await tokenStore.SaveAsync(token);
        });
    }
}
