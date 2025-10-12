using ProductClientHub.App.Models;

namespace ProductClientHub.App.UseCases.Auth.Register;

public interface IRegisterUserUseCase
{
    Task Execute(Models.SignUp user);
}
