using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Clients.GetAll;

public class GetAllClientsUseCase : IGetAllClientsUseCase
{
    private readonly IUserApiClient _api;

    public GetAllClientsUseCase(IUserApiClient api)
    {
        _api = api;
    }

    public async Task<IReadOnlyList<ResponseShortClientJson>> Execute()
    {
        var resp = await _api.GetAllClients();
        if (resp.StatusCode == System.Net.HttpStatusCode.NoContent || resp.Content is null)
            return Array.Empty<ResponseShortClientJson>();

        return resp.Content.Clients;
    }
}
