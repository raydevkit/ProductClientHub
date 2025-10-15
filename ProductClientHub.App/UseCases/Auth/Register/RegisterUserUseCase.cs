using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.App.Services;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.App.UseCases.Auth.Register;

public interface IRegisterUserUseCase
{
    Task Execute(Models.SignUp user);
}

public class RegisterUserUseCase(
    IUserApiClient userApi,
    IApiErrorHandler errorHandler) : BaseUseCase(errorHandler), IRegisterUserUseCase
{
    public async Task Execute(Models.SignUp user)
    {
        await ExecuteWithErrorHandlingAsync(async () =>
        {
            var request = new RequestRegisterUserJson
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };
            await userApi.Register(request);
        });
    }
}
