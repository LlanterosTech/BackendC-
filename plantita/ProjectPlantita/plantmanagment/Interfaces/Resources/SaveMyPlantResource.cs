namespace plantita.ProjectPlantita.plantmanagment.Interfaces.Resources;

public class SaveMyPlantResource
{
    public Guid PlantId { get; set; }
    public string CustomName { get; set; }
    public DateTime AcquiredAt { get; set; }
    public string Location { get; set; }
    public string Note { get; set; }
    public string PhotoUrl { get; set; }
    public string CurrentStatus { get; set; }
}