using ProductClientHub.App.Data.Network.Api;

namespace ProductClientHub.App.UseCases.Clients.Delete;

public class DeleteClientUseCase : IDeleteClientUseCase
{
    private readonly IUserApiClient _api;

    public DeleteClientUseCase(IUserApiClient api)
    {
        _api = api;
    }

    public async Task Execute(Guid id)
    {
        await _api.DeleteClient(id);
    }
}
