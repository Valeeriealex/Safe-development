using System;
using Microsoft.Extensions.DependencyInjection;
using SafeDevelopment;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddSingleton<MongoDbService>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new MongoDbService(
                configuration.GetConnectionString("MongoDb"),
                configuration["MongoDb:Database"],
                configuration["MongoDb:Collection"]);
        });

        services.AddSingleton<ElasticsearchService>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new ElasticsearchService(configuration.GetConnectionString("Elasticsearch"));
        });

        var serviceProvider = services.BuildServiceProvider();

        var mongoDbService = serviceProvider.GetService<MongoDbService>();
        var elasticsearchService = serviceProvider.GetService<ElasticsearchService>();
    }
}