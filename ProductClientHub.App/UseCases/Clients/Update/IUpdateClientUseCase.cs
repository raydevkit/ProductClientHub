using ProductClientHub.Communication.Requests;

namespace ProductClientHub.App.UseCases.Clients.Update;

public interface IUpdateClientUseCase
{
    Task Execute(Guid id, RequestClientJson request);
}
