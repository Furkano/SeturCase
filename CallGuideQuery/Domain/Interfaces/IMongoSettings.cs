namespace Domain.Interfaces
{
    public interface IMongoSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}