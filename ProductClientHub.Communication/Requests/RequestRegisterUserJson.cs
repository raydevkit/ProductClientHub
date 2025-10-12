namespace ProductClientHub.Communication.Requests
{
    public class RequestRegisterUserJson
    {
        public string Email { get; set; } = string.Empty;
        public string Name  { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}