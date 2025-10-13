using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Clients.GetById;

public class GetClientByIdUseCase : IGetClientByIdUseCase
{
    private readonly IUserApiClient _api;

    public GetClientByIdUseCase(IUserApiClient api)
    {
        _api = api;
    }

    public async Task<ResponseClientJson> Execute(Guid id)
    {
        return await _api.GetClientById(id);
    }
}
