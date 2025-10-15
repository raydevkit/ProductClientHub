using ProductClientHub.Communication.Responses;
using Refit;
using System.Net;
using System.Text.Json;

namespace ProductClientHub.App.Services;

public interface IApiErrorHandler
{
    Task<IEnumerable<string>> ExtractErrorMessagesAsync(Exception exception);
    Task HandleAndNotifyAsync(Exception exception);
}

public class ApiErrorHandler(IErrorNotifier errorNotifier) : IApiErrorHandler
{
    private readonly IErrorNotifier _errorNotifier = errorNotifier;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<IEnumerable<string>> ExtractErrorMessagesAsync(Exception exception)
    {
        return exception switch
        {
            ApiException apiException => ParseApiException(apiException),
            HttpRequestException httpException => HandleHttpException(httpException),
            TaskCanceledException => ["Request timed out. Please try again."],
            _ => ["An unexpected error occurred. Please try again."]
        };
    }

    public async Task HandleAndNotifyAsync(Exception exception)
    {
        var errors = await ExtractErrorMessagesAsync(exception);
        await _errorNotifier.ShowErrors(errors);
    }

    private static IEnumerable<string> ParseApiException(ApiException apiException)
    {
        try
        {
            var content = apiException.Content;
            if (string.IsNullOrWhiteSpace(content))
            {
                return GetDefaultErrorMessage(apiException.StatusCode);
            }

            var errorResponse = JsonSerializer.Deserialize<ResponseErrorMessagesJson>(content, JsonOptions);

            if (errorResponse?.Errors is { Count: > 0 })
            {
                return errorResponse.Errors;
            }

            return GetDefaultErrorMessage(apiException.StatusCode);
        }
        catch
        {
            return GetDefaultErrorMessage(apiException.StatusCode);
        }
    }

    private static IEnumerable<string> HandleHttpException(HttpRequestException httpException)
    {
        if (httpException.Message.Contains("No such host", StringComparison.OrdinalIgnoreCase) ||
            httpException.Message.Contains("network", StringComparison.OrdinalIgnoreCase))
        {
            return ["Unable to connect to the server. Please check your internet connection."];
        }

        return ["A network error occurred. Please try again."];
    }

    private static IEnumerable<string> GetDefaultErrorMessage(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.BadRequest => ["Invalid request. Please check your input."],
            HttpStatusCode.Unauthorized => ["You are not authorized. Please login again."],
            HttpStatusCode.Forbidden => ["You don't have permission to perform this action."],
            HttpStatusCode.NotFound => ["The requested resource was not found."],
            HttpStatusCode.InternalServerError => ["A server error occurred. Please try again later."],
            HttpStatusCode.ServiceUnavailable => ["The service is temporarily unavailable. Please try again later."],
            _ => ["An error occurred. Please try again."]
        };
    }
}
