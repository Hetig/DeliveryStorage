using AutoMapper;
using DeliveryStorage.API.Dtos;
using DeliveryStorage.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryStorage.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AssignBoxController : ControllerBase
{
    private readonly IAssignBoxService _assignBoxService;
    private readonly ILogger<AssignBoxController> _logger;
    private readonly IMapper _mapper;

    public AssignBoxController(IAssignBoxService assignBoxService, ILogger<AssignBoxController> logger, IMapper mapper)
    {
        _assignBoxService = assignBoxService;
        _logger = logger;
        _mapper = mapper;
    }
    
    [HttpPatch]
    public async Task<IActionResult> AssignBoxToPallet([FromBody] AssignBoxesQueryDto assignBoxesQuery)
    {
        try
        {
            var pallet = await _assignBoxService.AssignBoxToPallet(assignBoxesQuery.PalletId, assignBoxesQuery.BoxesId);
            return pallet == null ? BadRequest("Все идентификаторы должны быть существующими. Периметр коробки не должен превышать периметр паллета.") : Ok(_mapper.Map<GetPalletResponseDto>(pallet));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assign boxes to pallet boxes");
            return StatusCode(500, "Internal server error");
        }
     }
}