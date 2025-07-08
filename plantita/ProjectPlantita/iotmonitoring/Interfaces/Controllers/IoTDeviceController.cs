using Microsoft.AspNetCore.Mvc;
using plantita.ProjectPlantita.iotmonitoring.Application.Internal.Services;
using plantita.ProjectPlantita.iotmonitoring.domain.model.aggregates;
using plantita.ProjectPlantita.iotmonitoring.Interfaces.Resources;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace plantita.ProjectPlantita.iotmonitoring.Interfaces.Controllers
{
    [ApiController]
    [Route("plantita/v1/[controller]")]
    public class IoTDeviceController : ControllerBase
    {
        private readonly IIoTDeviceService _iotDeviceService;
        public IoTDeviceController(IIoTDeviceService iotDeviceService)
        {
            _iotDeviceService = iotDeviceService;
        }

        [HttpGet]
        public async Task<IEnumerable<IoTDeviceResource>> GetAll()
        {
            var devices = await _iotDeviceService.ListAsync();
            var resources = new List<IoTDeviceResource>();
            foreach (var d in devices)
            {
                resources.Add(new IoTDeviceResource {
                    DeviceId = d.DeviceId,
                    AuthUserId = d.AuthUserId,
                    MyPlantId = d.MyPlantId,
                    DeviceName = d.DeviceName,
                    ConnectionType = d.ConnectionType,
                    Location = d.Location,
                    ActivatedAt = d.ActivatedAt,
                    Status = d.Status,
                    FirmwareVersion = d.FirmwareVersion
                });
            }
            return resources;
        }
        
        private IoTDeviceResource ToResource(IoTDevice entity)
        {
            return new IoTDeviceResource
            {
                DeviceId = entity.DeviceId,
                AuthUserId = entity.AuthUserId,
                MyPlantId = entity.MyPlantId,
                DeviceName = entity.DeviceName,
                ConnectionType = entity.ConnectionType,
                Location = entity.Location,
                ActivatedAt = entity.ActivatedAt,
                Status = entity.Status,
                FirmwareVersion = entity.FirmwareVersion
            };
        }

        
        [HttpGet("me")]
        public async Task<IActionResult> GetAllDevicesByUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            
            var userId = Guid.Parse(userIdClaim.Value);
            var myDevices = await _iotDeviceService.GetAllUsersDevicesAsync(userId);
    
            var resources = myDevices.Select(ToResource);
            return Ok(resources);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IoTDeviceResource>> GetById(Guid id)
        {
            var device = await _iotDeviceService.GetByIdAsync(id);
            if (device == null) return NotFound();
            return new IoTDeviceResource {
                DeviceId = device.DeviceId,
                AuthUserId = device.AuthUserId,
                MyPlantId = device.MyPlantId,
                DeviceName = device.DeviceName,
                ConnectionType = device.ConnectionType,
                Location = device.Location,
                ActivatedAt = device.ActivatedAt,
                Status = device.Status,
                FirmwareVersion = device.FirmwareVersion
            };
        }

        [HttpPost]
        public async Task<ActionResult<IoTDeviceResource>> Create([FromBody] SaveIoTDeviceResource resource)
        {
            
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);
            
            var device = new IoTDevice {
                AuthUserId = userId,
                MyPlantId = resource.MyPlantId, 
                DeviceName = resource.DeviceName,
                ConnectionType = resource.ConnectionType,
                Location = resource.Location,
                ActivatedAt = resource.ActivatedAt,
                Status = resource.Status,
                FirmwareVersion = resource.FirmwareVersion
            };
            var result = await _iotDeviceService.CreateAsync(device);
            return CreatedAtAction(nameof(GetById), new { id = result.DeviceId }, new IoTDeviceResource {
                DeviceId = result.DeviceId,
                AuthUserId = result.AuthUserId,
                MyPlantId = result.MyPlantId,
                DeviceName = result.DeviceName,
                ConnectionType = result.ConnectionType,
                Location = result.Location,
                ActivatedAt = result.ActivatedAt,
                Status = result.Status,
                FirmwareVersion = result.FirmwareVersion
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IoTDeviceResource>> Update(Guid id, [FromBody] SaveIoTDeviceResource resource)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);
            var device = new IoTDevice {
                AuthUserId = userId,
                DeviceName = resource.DeviceName,
                MyPlantId = resource.MyPlantId,
                ConnectionType = resource.ConnectionType,
                Location = resource.Location,
                ActivatedAt = resource.ActivatedAt,
                Status = resource.Status,
                FirmwareVersion = resource.FirmwareVersion
            };
            var result = await _iotDeviceService.UpdateAsync(id, device);
            if (result == null) return NotFound();
            return new IoTDeviceResource {
                DeviceId = result.DeviceId,
                AuthUserId = result.AuthUserId,
                DeviceName = result.DeviceName,
                MyPlantId = result.MyPlantId,
                ConnectionType = result.ConnectionType,
                Location = result.Location,
                ActivatedAt = result.ActivatedAt,
                Status = result.Status,
                FirmwareVersion = result.FirmwareVersion
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _iotDeviceService.DeleteAsync(id);
            return NoContent();
        }
    }
}
