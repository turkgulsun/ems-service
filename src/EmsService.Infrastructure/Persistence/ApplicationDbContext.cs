using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using EmsService.Application.Interfaces;
using EmsService.Domain.Entities;

namespace EmsService.Infrastructure.Persistence;

public class ApplicationDbContext : IApplicationDbContext
{
    private readonly IDynamoDBContext _dynamoDbContext;
    private readonly IAmazonDynamoDB _dynamoDbClient;

    public ApplicationDbContext(IDynamoDBContext dynamoDbContext, IAmazonDynamoDB dynamoDbClient)
    {
        _dynamoDbContext = dynamoDbContext;
        _dynamoDbClient = dynamoDbClient;
    }

    public async Task<string> CreateEnergyReadingWithOutboxAsync(
        EnergyReading energyReading, 
        OutboxMessage outboxMessage, 
        CancellationToken cancellationToken = default)
    {
        // Create EnergyReading item manually
        var energyReadingItem = new Dictionary<string, AttributeValue>
        {
            ["Id"] = new AttributeValue { S = energyReading.Id },
            ["Timestamp"] = new AttributeValue { S = energyReading.Timestamp.ToString("O") },
            ["EnergyValue"] = new AttributeValue { N = energyReading.EnergyValue.ToString() },
            ["MeterId"] = new AttributeValue { S = energyReading.MeterId },
            ["LocationId"] = new AttributeValue { S = energyReading.LocationId },
            ["CreatedAt"] = new AttributeValue { S = energyReading.CreatedAt.ToString("O") }
        };
        if (energyReading.UpdatedAt.HasValue)
        {
            energyReadingItem["UpdatedAt"] = new AttributeValue { S = energyReading.UpdatedAt.Value.ToString("O") };
        }

        // Create OutboxMessage item manually
        var outboxItem = new Dictionary<string, AttributeValue>
        {
            ["Id"] = new AttributeValue { S = outboxMessage.Id },
            ["EventType"] = new AttributeValue { S = outboxMessage.EventType },
            ["Payload"] = new AttributeValue { S = outboxMessage.Payload },
            ["CreatedAt"] = new AttributeValue { S = outboxMessage.CreatedAt.ToString("O") },
            ["IsProcessed"] = new AttributeValue { BOOL = outboxMessage.IsProcessed }
        };
        if (outboxMessage.ProcessedAt.HasValue)
        {
            outboxItem["ProcessedAt"] = new AttributeValue { S = outboxMessage.ProcessedAt.Value.ToString("O") };
        }

        // DynamoDB Transaction: Save both EnergyReading and OutboxMessage atomically
        var transactionRequest = new TransactWriteItemsRequest
        {
            TransactItems = new List<TransactWriteItem>
            {
                new TransactWriteItem
                {
                    Put = new Put
                    {
                        TableName = "EnergyReadings",
                        Item = energyReadingItem
                    }
                },
                new TransactWriteItem
                {
                    Put = new Put
                    {
                        TableName = "OutboxMessages",
                        Item = outboxItem
                    }
                }
            }
        };

        await _dynamoDbClient.TransactWriteItemsAsync(transactionRequest, cancellationToken);
        return energyReading.Id;
    }

    public async Task<EnergyReading?> GetEnergyReadingByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dynamoDbContext.LoadAsync<EnergyReading>(id, cancellationToken);
    }

    public async Task<IEnumerable<EnergyReading>> GetAllEnergyReadingsAsync(CancellationToken cancellationToken = default)
    {
        var scanResult = await _dynamoDbContext.ScanAsync<EnergyReading>(new List<Amazon.DynamoDBv2.DataModel.ScanCondition>()).GetRemainingAsync(cancellationToken);
        return scanResult ?? Enumerable.Empty<EnergyReading>();
    }
}
