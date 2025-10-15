using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.App.Services;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Clients.GetAll;

public interface IGetAllClientsUseCase
{
    Task<IReadOnlyList<ResponseShortClientJson>> Execute();
}

public class GetAllClientsUseCase(
    IUserApiClient api,
    IApiErrorHandler errorHandler) : BaseUseCase(errorHandler), IGetAllClientsUseCase
{
    public async Task<IReadOnlyList<ResponseShortClientJson>> Execute()
    {
        return await ExecuteWithErrorHandlingAsync(async () =>
        {
            var resp = await api.GetAllClients();
            if (resp.StatusCode == System.Net.HttpStatusCode.NoContent || resp.Content is null)
                return Array.Empty<ResponseShortClientJson>();

            return (IReadOnlyList<ResponseShortClientJson>)resp.Content.Clients;
        });
    }
}
