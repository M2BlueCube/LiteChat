namespace LiteChat.Model;

public class Message
{
    public string From { get; set; }
    public string To { get; set; }
    public string Text { get; set; }    
    public DateTime Timestamp { get; set; }
}
