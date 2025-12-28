using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace EmsService.Infrastructure.Persistence;

public static class DynamoDbConfiguration
{
    public static IServiceCollection AddDynamoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var dynamoDbConfig = configuration.GetSection("DynamoDb");
        var serviceUrl = dynamoDbConfig["ServiceUrl"] ?? "http://localhost:8000";
        var region = dynamoDbConfig["Region"] ?? "us-east-1";

        var clientConfig = new AmazonDynamoDBConfig
        {
            ServiceURL = serviceUrl
            // Don't set RegionEndpoint when using ServiceURL for DynamoDB Local
        };

        // For DynamoDB Local, use dummy credentials (signing is disabled when ServiceURL is set)
        var credentials = new Amazon.Runtime.BasicAWSCredentials("dummy", "dummy");
        var client = new AmazonDynamoDBClient(credentials, clientConfig);
        services.AddSingleton<IAmazonDynamoDB>(client);
        services.AddScoped<IDynamoDBContext>(provider => 
        {
            var dbClient = provider.GetRequiredService<IAmazonDynamoDB>();
            return new DynamoDBContext(dbClient);
        });

        return services;
    }
}

