using System.Net;

namespace ProductClientHub.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException(List<string> errorMessages) : ProductClientHubException(string.Empty)
    {
        private readonly List<string> _errors = errorMessages;

        public override List<string> GetErrors() => _errors;

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.BadRequest;
    }
}
