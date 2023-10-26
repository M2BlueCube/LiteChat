using LiteChat.Dto;
using Newtonsoft.Json;
using System.Text;

namespace LiteChat.Services;

internal class Client : IClient
{
    private readonly string _baseUrl;
    private readonly IRsaService rsa;
    private string Token { get; set; } = null;
    public Client()
    {
        //_baseUrl = "https://auth.shieldedinbox.com/api/Identity/";
        _baseUrl = "https://localhost:44310/api/";
        rsa = new RsaService();
    }
    private static CancellationToken GetDefulatCancellationToken()
    {
        CancellationTokenSource cts = new();
        cts.CancelAfter(TimeSpan.FromSeconds(30));
        return cts.Token;
    }

    public async Task RegisterAsync(string privateKey, CancellationToken? cancellationToken = null)
    {
        var ct = cancellationToken ?? GetDefulatCancellationToken();
        var publicKey = rsa.GetPublicKeyFromPrivateKey(privateKey);
        var request = new LoginRequest()
        {
            PublicKey = publicKey,
            Signature = rsa.Sign(publicKey, privateKey),
        };
        var content = GetContent(request);
        using HttpClient httpClient = new();
        var response = await httpClient.PostAsync(_baseUrl + "Identity/Register", content, ct);
        var responseString = await response.Content.ReadAsStringAsync(ct);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Register was failed : {responseString}");
    }

    public async Task<string> LoginAsync(string privateKey, CancellationToken? cancellationToken = null)
    {
        var ct = cancellationToken ?? GetDefulatCancellationToken();

        var publicKey = rsa.GetPublicKeyFromPrivateKey(privateKey);
        var request = new LoginRequest()
        {
            PublicKey = publicKey,
            Signature = rsa.Sign(publicKey, privateKey),
        };

        var content = GetContent(request);
        using HttpClient httpClient = new();
        var response = await httpClient.PostAsync(_baseUrl + "Identity/Login", content, ct);
        var responseString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Login was failed : {responseString}");
        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);
        var encryptToken = loginResponse.Token;
        Token = rsa.Decrypt(encryptToken, privateKey);

        return Token;


        
    }

    public async Task<UserDto> GetUserAsync(CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrEmpty(Token))
            throw new ArgumentNullException(nameof(Token));
        var ct = cancellationToken ?? GetDefulatCancellationToken();
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
        var response = await httpClient.GetAsync(_baseUrl + "Identity/MyUser", ct);
        var responseString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception($"GetUser was failed : {responseString}");
        var user = JsonConvert.DeserializeObject<UserDto>(responseString);
        return user;
    }
    
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrEmpty(Token))
            throw new ArgumentNullException(nameof(Token));
        var ct = cancellationToken ?? GetDefulatCancellationToken();
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
        var response = await httpClient.GetAsync(_baseUrl + "Identity/GetAllUsers", ct);
        var responseString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception($"GetUser was failed : {responseString}");
        var user = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(responseString);
        return user;
    }

    private static StringContent GetContent(object request)
    {
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    public async Task<ICollection<ChatMessageEventDto>> GetMessagesAsync(GetChatQuery body, CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrEmpty(Token))
            throw new ArgumentNullException(nameof(Token));
        var ct = cancellationToken ?? GetDefulatCancellationToken();
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
        var response = await httpClient.PostAsync(_baseUrl + "LiteChat/GetMessages",GetContent(body), ct);
        var responseString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception($"GetMessagesAsync was failed : {responseString}");
        var chatMessages = JsonConvert.DeserializeObject<ICollection<ChatMessageEventDto>>(responseString);
        return chatMessages;
    }

    public async Task AppendMessageAsync(AppendChatMessage body, CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrEmpty(Token))
            throw new ArgumentNullException(nameof(Token));
        var ct = cancellationToken ?? GetDefulatCancellationToken();
        var content = GetContent(body);
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
        var response = await httpClient.PostAsync(_baseUrl + "LiteChat/AppendMessage", content, ct);
        var responseString = await response.Content.ReadAsStringAsync(ct);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"AppendMessageAsync was failed : {responseString}");
    }
}
