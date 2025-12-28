using Amazon.DynamoDBv2.DataModel;

namespace EmsService.Domain.Entities;

[DynamoDBTable("EnergyReadings")]
public class EnergyReading
{
    [DynamoDBHashKey]
    public string Id { get; set; }
    
    public DateTime Timestamp { get; set; }
    public decimal EnergyValue { get; set; } // kWh
    public string MeterId { get; set; }
    public string LocationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Public parameterless constructor for DynamoDB
    public EnergyReading()
    {
        Id = string.Empty;
        MeterId = string.Empty;
        LocationId = string.Empty;
    }

    public EnergyReading(
        string id,
        DateTime timestamp,
        decimal energyValue,
        string meterId,
        string locationId)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Id cannot be null or empty", nameof(id));
        if (string.IsNullOrWhiteSpace(meterId))
            throw new ArgumentException("MeterId cannot be null or empty", nameof(meterId));
        if (string.IsNullOrWhiteSpace(locationId))
            throw new ArgumentException("LocationId cannot be null or empty", nameof(locationId));

        Id = id;
        Timestamp = timestamp;
        EnergyValue = energyValue;
        MeterId = meterId;
        LocationId = locationId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(DateTime timestamp, decimal energyValue)
    {
        Timestamp = timestamp;
        EnergyValue = energyValue;
        UpdatedAt = DateTime.UtcNow;
    }
}
