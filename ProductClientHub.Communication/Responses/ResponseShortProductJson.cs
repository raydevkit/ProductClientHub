namespace ProductClientHub.Communication.Responses
{
    public class ResponseShortProductJson
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
