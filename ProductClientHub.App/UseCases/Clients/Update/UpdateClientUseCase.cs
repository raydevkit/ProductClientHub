using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.App.UseCases.Clients.Update;

public class UpdateClientUseCase : IUpdateClientUseCase
{
    private readonly IUserApiClient _api;

    public UpdateClientUseCase(IUserApiClient api)
    {
        _api = api;
    }

    public async Task Execute(Guid id, RequestClientJson request)
    {
        await _api.UpdateClient(id, request);
    }
}
