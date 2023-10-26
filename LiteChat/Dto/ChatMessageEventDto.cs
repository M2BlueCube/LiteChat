namespace LiteChat.Dto;

public class ChatMessageEventDto
{
    public int Id { get; set; }

    public DateTimeOffset OccurredAt { get; set; }

    public DateTimeOffset Day { get; set; }

    public Guid From { get; set; }

    public Guid To { get; set; }

    public Guid Chat { get; set; }

    public string Message { get; set; }
}
