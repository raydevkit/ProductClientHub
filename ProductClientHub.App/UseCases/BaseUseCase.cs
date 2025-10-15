using ProductClientHub.App.Services;

namespace ProductClientHub.App.UseCases;

public abstract class BaseUseCase
{
    protected readonly IApiErrorHandler ErrorHandler;

    protected BaseUseCase(IApiErrorHandler errorHandler)
    {
        ErrorHandler = errorHandler;
    }
    protected async Task<T> ExecuteWithErrorHandlingAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            await ErrorHandler.HandleAndNotifyAsync(ex);
            throw; 
        }
    }

    protected async Task ExecuteWithErrorHandlingAsync(Func<Task> operation)
    {
        try
        {
            await operation();
        }
        catch (Exception ex)
        {
            await ErrorHandler.HandleAndNotifyAsync(ex);
            throw; 
        }
    }
}
