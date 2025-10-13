using System.Net.Http.Headers;
using ProductClientHub.App.Data.Auth;

namespace ProductClientHub.App.Data.Network;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly ITokenStore _tokenStore;

    public AuthHeaderHandler(ITokenStore tokenStore)
    {
        _tokenStore = tokenStore;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        
        var token = await _tokenStore.GetAccessTokenAsync();
        var expiresAt = await _tokenStore.GetExpiresAtUtcAsync();

        if (!string.IsNullOrWhiteSpace(token) && (expiresAt == null || expiresAt > DateTime.UtcNow))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
