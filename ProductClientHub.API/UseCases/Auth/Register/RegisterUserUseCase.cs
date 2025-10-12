using ProductClientHub.API.Entities;
using ProductClientHub.API.Infrastructure;
using ProductClientHub.API.Security;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.API.UseCases.Auth.Register
{
    public class RegisterUserUseCase
    {
        public ResponseRegisteredUserJson Execute(RequestRegisterUserJson request)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.Email)) errors.Add("Email is required.");
            if (string.IsNullOrWhiteSpace(request.Name)) errors.Add("Name is required.");
            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6) errors.Add("Password must be at least 6 characters.");
            if (errors.Count > 0) throw new ErrorOnValidationException(errors);

            var db = new ProductClientHubDBContext();
            var exists = db.Users.Any(u => u.Email == request.Email);
            if (exists) throw new ErrorOnValidationException(["A user with this email already exists."]);

            var (salt, hash) = PasswordHasher.Hash(request.Password);

            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                PasswordSalt = salt,
                PasswordHash = hash,
                Role = "User"
            };

            db.Users.Add(user);
            db.SaveChanges();

            return new ResponseRegisteredUserJson
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}