﻿namespace plantita.ProjectPlantita.plantmanagment.Interfaces.Resources;

public class IdentifiedPlantResource
{
    public string ScientificName { get; set; }
    public string CommonName { get; set; }
    public string Description { get; set; }
    public string Watering { get; set; }
    public string Sunlight { get; set; }
    public string WikiUrl { get; set; }
    public string ImageUrl { get; set; }
}