using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.App.Services;

namespace ProductClientHub.App.UseCases.Clients.Delete;

public interface IDeleteClientUseCase
{
    Task Execute(Guid id);
}

public class DeleteClientUseCase(
    IUserApiClient api,
    IApiErrorHandler errorHandler) : BaseUseCase(errorHandler), IDeleteClientUseCase
{
    public async Task Execute(Guid id)
    {
        await ExecuteWithErrorHandlingAsync(async () => await api.DeleteClient(id));
    }
}
