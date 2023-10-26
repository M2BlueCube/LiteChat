using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LiteChat.Services;

[Serializable]
internal class ApiException : Exception
{
    private new readonly string Message;
    private readonly int StatusCode;
    private readonly string ResponseText;
    private readonly IReadOnlyDictionary<string, IEnumerable<string>> Headers;
    private readonly JsonException Exception;

    public ApiException() { }

    public ApiException(string message) : base(message) { }

    public ApiException(string message, Exception innerException) : base(message, innerException) { }

    public ApiException(string message, int statusCode, string responseText, IReadOnlyDictionary<string, IEnumerable<string>> headers, JsonException exception)
    {
        Message = message;
        StatusCode = statusCode;
        ResponseText = responseText;
        Headers = headers;
        Exception = exception;
    }

    protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context) { }

}