using System.Security.Cryptography;
using System.Text;

namespace LiteChat.Services;

public class RsaService : IRsaService
{
    private static readonly RSAEncryptionPadding EncryptionPadding = RSAEncryptionPadding.Pkcs1;
    private static readonly RSASignaturePadding SignaturePadding = RSASignaturePadding.Pkcs1;
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;
    private const int MaxDataSize = 245;
    private const int EncryptedBlockSize = 256;

    public (string publicKey, string privateKey) CreateKeys()
    {
        var rsa = RSA.Create();
        return (rsa.ToXmlString(false), rsa.ToXmlString(true));
    }
    public string? Encrypt(string text, string publicKey)
    {
        var rsa = CreateFromKey(publicKey);
        var textBytes = GetBytes(text);
        byte[] encryptedBytes;
        if (textBytes.Length > MaxDataSize)
        {
            var encryptedBytesList = new List<byte[]>();
            var listOfTextBytes = Split(textBytes, MaxDataSize);
            for (var i = 0; i < listOfTextBytes.Count; i++)
            {
                var item = listOfTextBytes[i];
                var encryptedItem = rsa.Encrypt(item, EncryptionPadding);
                encryptedBytesList.Add(encryptedItem);
            }
            encryptedBytes = Merge(encryptedBytesList);
        }
        else
        {
            encryptedBytes = rsa.Encrypt(textBytes, EncryptionPadding);
        }
        var encryptedText = Convert.ToBase64String(encryptedBytes);
        return encryptedText;
    }
    public string? Decrypt(string encryptedText, string privateKey)
    {
        var rsa = CreateFromKey(privateKey);
        var encryptedBytes = Convert.FromBase64String(encryptedText);
        byte[] bytes;
        if (encryptedBytes.Length > EncryptedBlockSize)
        {
            var decryptedBytesList = new List<byte[]>();
            var listOfEncryptedBytes = Split(encryptedBytes, EncryptedBlockSize);
            for (var i = 0; i < listOfEncryptedBytes.Count; i++)
            {
                var item = listOfEncryptedBytes[i];
                var decryptedBytes = rsa.Decrypt(item, EncryptionPadding);
                decryptedBytesList.Add(decryptedBytes);
            }
            bytes = Merge(decryptedBytesList);
        }
        else
        {
            bytes = rsa.Decrypt(encryptedBytes, EncryptionPadding);
        }
        var decryptedText = GetString(bytes);
        return decryptedText;
    }
    public string? Sign(string text, string privateKey)
    {
        var rsa = CreateFromKey(privateKey);
        var textBytes = GetBytes(text);
        var signatureBytes = rsa.SignData(textBytes, HashAlgorithm, SignaturePadding);
        var signature = Convert.ToBase64String(signatureBytes);
        return signature;
    }
    public bool Verify(string text, string publicKey, string signature)
    {
        try
        {
            var rsa = CreateFromKey(publicKey);
            var textBytes = GetBytes(text);
            var signatureBytes = Convert.FromBase64String(signature);
            var result = rsa.VerifyData(textBytes, signatureBytes, HashAlgorithm, SignaturePadding);
            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public string GetPublicKeyFromPrivateKey(string privateKey)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(privateKey);
        var publicKey = rsa.ToXmlString(false);
        return publicKey;
    }
    public string GetNormalizedPublicKey(string publicKey)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(publicKey);
        var normalizedPublicKey = rsa.ToXmlString(false);
        return normalizedPublicKey;
    }

    private static RSA CreateFromKey(string key)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(key);
        return rsa;
    }
    private static byte[] GetBytes(string text) => Encoding.Unicode.GetBytes(text);
    private static string GetString(byte[] textBytes) => Encoding.Unicode.GetString(textBytes);
    private static IList<byte[]> Split(byte[] input, int blockSize)
    {
        var result = new List<byte[]>();
        var numberOfBlock = (input.Count() / blockSize) + (input.Count() % blockSize == 0 ? 0 : 1);
        for (var i = 0; i < numberOfBlock; i++)
        {
            result.Add(input.Skip(i * blockSize).Take(blockSize).ToArray());
        }
        return result;
    }
    private static byte[] Merge(IList<byte[]> input)
    {
        var totalSize = input.Sum(item => item.Length);
        var result = new byte[totalSize];
        var currentSize = 0;
        for (var i = 0; i < input.Count; i++)
        {
            Array.Copy(input[i], 0, result, currentSize, input[i].Length);
            currentSize += input[i].Length;
        }
        return result;
    }
}
