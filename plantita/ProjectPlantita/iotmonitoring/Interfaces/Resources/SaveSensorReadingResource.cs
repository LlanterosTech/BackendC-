namespace plantita.ProjectPlantita.iotmonitoring.Interfaces.Resources
{
    public class SaveSensorReadingResource
    {
        public Guid SensorId { get; set; }
        public Guid MyPlantId { get; set; }
        public decimal Value { get; set; }
        public DateTime Timestamp { get; set; }
        public bool TriggeredAlert { get; set; }
        public string Source { get; set; }
    }
}
