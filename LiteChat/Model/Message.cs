namespace LiteChat.Model;

public class Message
{
    public int Id { get; set; }
    public bool IsMine { get; set; }
    public string Text { get; set; }    
    public DateTime Timestamp { get; set; }
}
