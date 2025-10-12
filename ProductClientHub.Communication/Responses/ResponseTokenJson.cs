namespace ProductClientHub.Communication.Responses
{
    public class ResponseTokenJson
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}