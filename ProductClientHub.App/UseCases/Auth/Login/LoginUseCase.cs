using ProductClientHub.App.Data.Auth;
using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.App.UseCases.Auth.Login;

public interface ILoginUseCase
{
    Task Execute(string email, string password);
}

public class LoginUseCase : ILoginUseCase
{
    private readonly IUserApiClient _api;
    private readonly ITokenStore _tokenStore;

    public LoginUseCase(IUserApiClient apiClient, ITokenStore tokenStore)
    {
        _api = apiClient;
        _tokenStore = tokenStore;
    }

    public async Task Execute(string email, string password)
    {
        var request = new RequestLoginJson { Email = email, Password = password };
        var token = await _api.Login(request);
        await _tokenStore.SaveAsync(token);
    }
}
