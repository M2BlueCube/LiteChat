using LiteChat.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteChat.Services;

internal class MessageService
{
    private readonly IRsaService rsa;
    private readonly string privateKey;
    public MessageService()
    {
        //_baseUrl = "https://localhost:44310/api/Identity/";
        rsa = new RsaService();

    }
    public SignedMessage SendMessage(Message message)
    {
        var msg = JsonConvert.SerializeObject(message);
        var msgSignature = rsa.Sign(msg, privateKey);
        return new SignedMessage(message, msgSignature);

    }
}
