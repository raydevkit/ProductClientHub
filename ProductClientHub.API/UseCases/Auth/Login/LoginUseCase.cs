using ProductClientHub.API.Infrastructure;
using ProductClientHub.API.Security;
using ProductClientHub.API.Services;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.API.UseCases.Auth.Login
{
    public class LoginUseCase
    {
        public ResponseTokenJson Execute(RequestLoginJson request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                throw new ErrorOnValidationException(["Email and Password are required."]);

            var db = new ProductClientHubDBContext();
            var user = db.Users.SingleOrDefault(u => u.Email == request.Email);
            if (user is null) throw new ErrorOnValidationException(["Invalid credentials."]);

            var ok = PasswordHasher.Verify(request.Password, user.PasswordSalt, user.PasswordHash);
            if (!ok) throw new ErrorOnValidationException(["Invalid credentials."]);

            var jwt = new JwtTokenService().CreateToken(user.Id, user.Email, user.Name, user.Role);

            return new ResponseTokenJson
            {
                AccessToken = jwt.Token,
                ExpiresAtUtc = jwt.ExpiresAtUtc
            };
        }
    }
}