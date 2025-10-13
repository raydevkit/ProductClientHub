namespace ProductClientHub.App.UseCases.Clients.Delete;

public interface IDeleteClientUseCase
{
    Task Execute(Guid id);
}
