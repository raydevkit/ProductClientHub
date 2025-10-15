using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.App.Services;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Clients.GetById;

public interface IGetClientByIdUseCase
{
    Task<ResponseClientJson> Execute(Guid id);
}

public class GetClientByIdUseCase(
    IUserApiClient api,
    IApiErrorHandler errorHandler) : BaseUseCase(errorHandler), IGetClientByIdUseCase
{
    public async Task<ResponseClientJson> Execute(Guid id)
    {
        return await ExecuteWithErrorHandlingAsync(async () => await api.GetClientById(id));
    }
}
