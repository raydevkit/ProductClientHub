using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.App.Services;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Clients.Create;

public interface ICreateClientUseCase
{
    Task<ResponseShortClientJson> Execute(RequestClientJson request);
}

public class CreateClientUseCase(
    IUserApiClient api,
    IApiErrorHandler errorHandler) : BaseUseCase(errorHandler), ICreateClientUseCase
{
    public async Task<ResponseShortClientJson> Execute(RequestClientJson request)
    {
        return await ExecuteWithErrorHandlingAsync(async () => await api.RegisterClient(request));
    }
}
