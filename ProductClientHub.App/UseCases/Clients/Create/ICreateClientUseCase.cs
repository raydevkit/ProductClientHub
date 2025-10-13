using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Clients.Create;

public interface ICreateClientUseCase
{
    Task<ResponseShortClientJson> Execute(RequestClientJson request);
}
