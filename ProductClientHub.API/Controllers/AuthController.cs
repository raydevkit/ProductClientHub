using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductClientHub.API.UseCases.Auth.Login;
using ProductClientHub.API.UseCases.Auth.Logout;
using ProductClientHub.API.UseCases.Auth.Register;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RequestRegisterUserJson request)
        {
            var useCase = new RegisterUserUseCase();
            var response = useCase.Execute(request);
            return Created(string.Empty, response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseTokenJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] RequestLoginJson request)
        {
            var useCase = new LoginUseCase();
            var response = useCase.Execute(request);
            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status401Unauthorized)]
        public IActionResult Logout()
        {
            var useCase = new LogoutUseCase();
            useCase.Execute();
            return NoContent();
        }
    }
}