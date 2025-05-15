using plantita.ProjectPlantita.plantmanagment.domain.model.aggregates;
using plantita.ProjectPlantita.plantmanagment.Interfaces.Resources;

namespace plantita.ProjectPlantita.plantmanagment.Interfaces.Transform;

public static class MyPlantMapper
{
    public static MyPlantResource ToResource(MyPlant model)
    {
        return new MyPlantResource
        {
            MyPlantId = model.MyPlantId,
            UserId = model.UserId,
            PlantId = model.PlantId,
            CustomName = model.CustomName,
            AcquiredAt = model.AcquiredAt,
            Location = model.Location,
            Note = model.Note,
            PhotoUrl = model.PhotoUrl,
            CurrentStatus = model.CurrentStatus
        };
    }

    public static MyPlant ToModel(SaveMyPlantResource resource, Guid userId,string photoUrl)
    {
        return new MyPlant
        {
            UserId = userId,
            PlantId = resource.PlantId,
            CustomName = resource.CustomName,
            AcquiredAt = resource.AcquiredAt,
            Location = resource.Location,
            Note = resource.Note,
            PhotoUrl = photoUrl,
            CurrentStatus = resource.CurrentStatus
        };
    }
}