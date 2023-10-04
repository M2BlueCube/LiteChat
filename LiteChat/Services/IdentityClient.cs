using Newtonsoft.Json;
using System.Text;

namespace LiteChat.Services;

internal class IdentityClient : IIdentityClient
{
    private readonly string _baseUrl;
    private readonly IRsaService rsa;
    public IdentityClient()
    {
        _baseUrl = "https://localhost:44310/api/Identity/";
        rsa = new RsaService();
    }
    private static CancellationToken GetDefulatCancellationToken()
    {
        CancellationTokenSource cts = new();
        cts.CancelAfter(TimeSpan.FromSeconds(3));
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
        var response = await httpClient.PostAsync(_baseUrl + "Register", content, ct);
        var responseString = await response.Content.ReadAsStringAsync(ct);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"request was failed : {responseString}");
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
        var response = await httpClient.PostAsync(_baseUrl + "Login", content, ct);
        var responseString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception($"request was failed : {responseString}");
        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);
        var encryptToken = loginResponse.Token;
        var token = rsa.Decrypt(encryptToken, privateKey);
        return token;
    }

    public async Task<UserDto> GetUserNameAsync(string token, CancellationToken? cancellationToken = null)
    {
        var ct = cancellationToken ?? GetDefulatCancellationToken();
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        var response = await httpClient.GetAsync(_baseUrl + "MyUser", ct);
        var responseString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception($"request was failed : {responseString}");
        var user = JsonConvert.DeserializeObject<UserDto>(responseString);
        return user;
    }

    private static StringContent GetContent(object request)
    {
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }
}
