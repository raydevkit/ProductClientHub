using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Clients.GetById;

public interface IGetClientByIdUseCase
{
    Task<ResponseClientJson> Execute(Guid id);
}
