using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.App.UseCases.Auth.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserApiClient _userApi;
    public RegisterUserUseCase(IUserApiClient userApi)
    {
        _userApi = userApi;
    }
    public async Task Execute(Models.SignUp user)
    {
        var request = new RequestRegisterUserJson
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password
        };
       var response = await _userApi.Register(request);
    }

}
