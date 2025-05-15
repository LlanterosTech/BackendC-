using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plantita.ProjectPlantita.plantmanagment.domain.model.aggregates;
using plantita.ProjectPlantita.plantmanagment.domain.Services;
using plantita.ProjectPlantita.plantmanagment.Interfaces.Resources;
using plantita.ProjectPlantita.plantmanagment.Interfaces.Transform;

namespace plantita.ProjectPlantita.plantmanagment.Interfaces.Controllers;


[Authorize]
[ApiController]
[Route("plantita/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class MyPlantController : ControllerBase
{
    private readonly IMyPlantCommandService _myPlantCommandService;
    private readonly IPlantQueryService _plantQueryService;

    public MyPlantController(IMyPlantCommandService myPlantCommandService, IPlantQueryService plantQueryService)
    {
        _myPlantCommandService = myPlantCommandService;
        _plantQueryService = plantQueryService;
    }

    // Explicación:
// - El endpoint POST recibe solo name, location y note en el body.
// - El PlantId se recibe como parámetro en la URL.
// - Se obtiene la imagen de la planta base usando PlantId.

    // plantita/ProjectPlantita/plantmanagment/Interfaces/Controllers/MyPlantController.cs
    // MyPlantController.cs
    // MyPlantController.cs
    [HttpPost("{plantId:guid}")]
    public async Task<IActionResult> Create(Guid plantId, [FromBody] SaveMyPlantResource resource)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("No se pudo obtener el usuario autenticado.");

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("El identificador de usuario no es válido.");

        var created = await _myPlantCommandService.RegisterMyPlantAsync(userId, plantId, resource);
        return CreatedAtAction(nameof(Create), new { id = created.MyPlantId }, MyPlantTransform.ToResource(created));
    }
}