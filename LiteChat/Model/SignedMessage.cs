namespace LiteChat.Model;

public class SignedMessage : Message
{
    public SignedMessage( Message message, string signature)
    {
        To=message.To; 
        From=message.From;
        Text = message.Text;
        Timestamp = message.Timestamp;
        Signature = signature;
    }
    public string Signature { get; set; }

}
