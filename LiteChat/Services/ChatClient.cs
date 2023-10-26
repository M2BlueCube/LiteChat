using LiteChat.Dto;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace LiteChat.Services;

public class ChatClient 
{
    private readonly string BaseUrl;
    private readonly HttpClient _client;
    private readonly JsonSerializerSettings JsonSerializerSettings;
    private readonly bool ReadResponseAsString;


    public async Task<ICollection<ChatMessageEventDto>> GetMessagesAsync(GetChatQuery body, CancellationToken? cancellationToken = null)
    {
        return new List<ChatMessageEventDto>()
        {
            new ChatMessageEventDto(){ Message = "Hi"},
            new ChatMessageEventDto(){ Message = "How are you?"},
            new ChatMessageEventDto(){ Message = "bye!"},
        };

        StringBuilder urlBuilder = new();
        urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/LiteChat/GetMessages");
        try
        {
            using HttpRequestMessage request = new();
            StringContent content = new(JsonConvert.SerializeObject(body));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request.Content = content;
            request.Method = new("POST");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));

            var url = urlBuilder.ToString();
            request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, (CancellationToken)cancellationToken).ConfigureAwait(false);
            try
            {
                var headers = Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                if (response.Content != null && response.Content.Headers != null)
                {
                    foreach (var item in response.Content.Headers)
                        headers[item.Key] = item.Value;
                }
                var status = ((int)response.StatusCode).ToString();
                if (status == "200")
                {
                    var objectResponse = await ReadObjectResponseAsync<ICollection<ChatMessageEventDto>>(response, headers).ConfigureAwait(false);
                    return objectResponse.Object;
                }
                else
                if (status != "200" && status != "204")
                {
                    var responseData_ = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new ApiException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", (int)response.StatusCode, responseData_, headers, null);
                }

                return default;
            }
            finally { response?.Dispose(); }
        }
        finally { }
    }
    public async Task AppendMessageAsync(AppendChatMessage body, CancellationToken? cancellationToken = null)
    {

        StringBuilder urlBuilder = new();
        urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/LiteChat/AppendMessage");

        try
        {
            using HttpRequestMessage request = new();
            StringContent content = new(JsonConvert.SerializeObject(body));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request.Content = content;
            request.Method = new HttpMethod("POST");

            var url_ = urlBuilder.ToString();
            request.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, (CancellationToken)cancellationToken).ConfigureAwait(false);
            try
            {
                var headers = Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                if (response.Content != null && response.Content.Headers != null)
                    foreach (var item in response.Content.Headers)
                        headers[item.Key] = item.Value;

                var status = ((int)response.StatusCode).ToString();
                if (status == "200")
                    return;
                else if (status != "200" && status != "204")
                {
                    var responseData_ = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new ApiException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", (int)response.StatusCode, responseData_, headers, null);
                }
            }
            catch (Exception ex)
            {
                var s = ex.Message;
            }
            finally { response?.Dispose(); }
        }
        catch (Exception ex)
        {
            var s = ex.Message;
        }
        finally { }
    }



    public async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers)
    {
        if (response == null || response.Content == null)
        {
            return new ObjectResponseResult<T>(default, string.Empty);
        }

        if (ReadResponseAsString)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            try
            {
                var typedBody = JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
                return new ObjectResponseResult<T>(typedBody, responseText);
            }
            catch (JsonException exception)
            {
                var message = $"Could not deserialize the response body string as {typeof(T).FullName}.";
                throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
            }
        }
        else
            try
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using StreamReader streamReader = new(responseStream);
                using JsonTextReader jsonTextReader = new(streamReader);
                var serializer = JsonSerializer.Create(JsonSerializerSettings);
                var typedBody = serializer.Deserialize<T>(jsonTextReader);
                return new(typedBody, string.Empty);
            }
            catch (JsonException exception)
            {
                var message = $"Could not deserialize the response body stream as {typeof(T).FullName}.";
                throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
            }
    }
    public readonly struct ObjectResponseResult<T>
    {
        public ObjectResponseResult(T responseObject, string responseText)
        {
            Object = responseObject;
            Text = responseText;
        }

        public T Object { get; }

        public string Text { get; }
    }

}