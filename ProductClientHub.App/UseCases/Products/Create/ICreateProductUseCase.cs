using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Products.Create;

public interface ICreateProductUseCase
{
    Task<ResponseShortProductJson> Execute(Guid clientId, RequestProductJson request);
}
