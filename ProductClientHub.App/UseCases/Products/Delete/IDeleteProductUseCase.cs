namespace ProductClientHub.App.UseCases.Products.Delete;

public interface IDeleteProductUseCase
{
    Task Execute(Guid id);
}
