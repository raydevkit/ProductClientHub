using Microsoft.Maui.Storage;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.Data.Auth;

public class SecureTokenStore : ITokenStore
{
    private const string AccessTokenKey = "auth_access_token";
    private const string ExpiresAtKey = "auth_expires_at";

    public async Task SaveAsync(ResponseTokenJson token)
    {
        if (!string.IsNullOrWhiteSpace(token.AccessToken))
        {
            await SecureStorage.SetAsync(AccessTokenKey, token.AccessToken);
        }
        await SecureStorage.SetAsync(ExpiresAtKey, token.ExpiresAtUtc.ToUniversalTime().Ticks.ToString());
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        try
        {
            return await SecureStorage.GetAsync(AccessTokenKey);
        }
        catch
        {
            return null;
        }
    }

    public async Task<DateTime?> GetExpiresAtUtcAsync()
    {
        try
        {
            var ticks = await SecureStorage.GetAsync(ExpiresAtKey);
            if (long.TryParse(ticks, out var t))
            {
                return new DateTime(t, DateTimeKind.Utc);
            }
        }
        catch { }
        return null;
    }

    public async Task ClearAsync()
    {
        SecureStorage.Remove(AccessTokenKey);
        SecureStorage.Remove(ExpiresAtKey);
        await Task.CompletedTask;
    }
}
