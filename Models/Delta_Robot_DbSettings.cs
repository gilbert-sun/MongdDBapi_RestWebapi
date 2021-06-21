namespace WebApplication3.Models
{
    public class DeltaRobotDbSettings : IDeltaRobotDbSettings
    {
        public string DeltaRobotCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDeltaRobotDbSettings
    {
        string DeltaRobotCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}