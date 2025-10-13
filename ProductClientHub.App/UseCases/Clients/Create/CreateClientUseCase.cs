using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Clients.Create;

public class CreateClientUseCase : ICreateClientUseCase
{
    private readonly IUserApiClient _api;

    public CreateClientUseCase(IUserApiClient api)
    {
        _api = api;
    }

    public async Task<ResponseShortClientJson> Execute(RequestClientJson request)
    {
        return await _api.RegisterClient(request);
    }
}
