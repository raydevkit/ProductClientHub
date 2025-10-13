using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.Data.Auth;

public interface ITokenStore
{
    Task SaveAsync(ResponseTokenJson token);
    Task<string?> GetAccessTokenAsync();
    Task<DateTime?> GetExpiresAtUtcAsync();
    Task ClearAsync();
}
