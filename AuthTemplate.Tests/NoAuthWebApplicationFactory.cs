namespace AuthTemplate.Tests
{
    public class NoAuthWebApplicationFactory<TStartup>(string databaseName) : BaseWebApplicationFactory<TStartup>(databaseName) where TStartup : class
    {
    }
}

