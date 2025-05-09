namespace plantita.ProjectPlantita.iotmonitoring.domain.model.Entities;

public class SensorReading
{
    public Guid ReadingId { get; set; }
    public Guid SensorId { get; set; }
    public Guid MyPlantId { get; set; }
    public decimal Value { get; set; }
    public DateTime Timestamp { get; set; }
    public bool TriggeredAlert { get; set; }
    public string Source { get; set; }
}