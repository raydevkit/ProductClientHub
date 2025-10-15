using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.App.Services;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.App.UseCases.Clients.Update;

public interface IUpdateClientUseCase
{
    Task Execute(Guid id, RequestClientJson request);
}

public class UpdateClientUseCase(
    IUserApiClient api,
    IApiErrorHandler errorHandler) : BaseUseCase(errorHandler), IUpdateClientUseCase
{
    public async Task Execute(Guid id, RequestClientJson request)
    {
        await ExecuteWithErrorHandlingAsync(async () => await api.UpdateClient(id, request));
    }
}
