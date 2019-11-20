using Microsoft.WindowsAzure.Storage.Table;

namespace HuntingSpots {
    public class Spot : TableEntity
    {
        public const string PARTITION_KEY = "1";

        public Spot()
        {
            PartitionKey = PARTITION_KEY;
        }

        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
}
}