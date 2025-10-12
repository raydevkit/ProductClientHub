using System.Net;

namespace ProductClientHub.Exceptions.ExceptionsBase
{
    public class UnauthorizedException : ProductClientHubException
    {
        public UnauthorizedException(string errorMessage = "Unauthorized") : base(errorMessage)
        {
        }

        public override List<string> GetErrors() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
    }
}