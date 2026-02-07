namespace Ecommerce.Infrastructure.Mongo;

public class MongoDbSettings
{
    public string ConnectionString { get; init; } = null!;
    public string DatabaseName { get; init; } = null!;
}