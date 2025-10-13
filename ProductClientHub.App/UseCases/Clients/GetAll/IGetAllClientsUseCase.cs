using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Clients.GetAll;

public interface IGetAllClientsUseCase
{
    Task<IReadOnlyList<ResponseShortClientJson>> Execute();
}
