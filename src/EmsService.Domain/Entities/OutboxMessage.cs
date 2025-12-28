using Amazon.DynamoDBv2.DataModel;

namespace EmsService.Domain.Entities;

[DynamoDBTable("OutboxMessages")]
public class OutboxMessage
{
    [DynamoDBHashKey]
    public string Id { get; set; }
    
    public string EventType { get; set; }
    public string Payload { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public bool IsProcessed { get; set; }

    // Public parameterless constructor for DynamoDB
    public OutboxMessage()
    {
        Id = string.Empty;
        EventType = string.Empty;
        Payload = string.Empty;
    }

    public OutboxMessage(
        string id,
        string eventType,
        string payload)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Id cannot be null or empty", nameof(id));
        if (string.IsNullOrWhiteSpace(eventType))
            throw new ArgumentException("EventType cannot be null or empty", nameof(eventType));
        if (string.IsNullOrWhiteSpace(payload))
            throw new ArgumentException("Payload cannot be null or empty", nameof(payload));

        Id = id;
        EventType = eventType;
        Payload = payload;
        CreatedAt = DateTime.UtcNow;
        IsProcessed = false;
    }

    public void MarkAsProcessed()
    {
        IsProcessed = true;
        ProcessedAt = DateTime.UtcNow;
    }
}
