using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Products.Create;

public class CreateProductUseCase : ICreateProductUseCase
{
    private readonly IUserApiClient _api;

    public CreateProductUseCase(IUserApiClient api)
    {
        _api = api;
    }

    public async Task<ResponseShortProductJson> Execute(Guid clientId, RequestProductJson request)
    {
        return await _api.RegisterProduct(clientId, request);
    }
}
