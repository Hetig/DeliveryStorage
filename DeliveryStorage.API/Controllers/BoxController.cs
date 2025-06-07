using AutoMapper;
using DeliveryStorage.API.Dtos;
using DeliveryStorage.Domain.Interfaces;
using DeliveryStorage.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DeliveryStorage.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BoxController : ControllerBase
    {
        private readonly IBoxService _boxService;
        private readonly ILogger<BoxController> _logger;
        private readonly IMapper _mapper;

        public BoxController(IBoxService boxService, ILogger<BoxController> logger, IMapper mapper)
        {
            _boxService = boxService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var boxes = await _boxService.GetAllAsync();
                return Ok(boxes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all boxes");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBoxDto box)
        {
            try
            {
                var created = await _boxService.AddAsync(_mapper.Map<Box>(box));
                return Created("api/v1/Box", created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create box");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BoxDto box)
        {
            try
            {
                var updated = await _boxService.UpdateAsync(_mapper.Map<Box>(box));
                return updated == null ? NotFound() : Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update box");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                var isDeleted = await _boxService.DeleteAsync(id);
                return isDeleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete box");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var finded = await _boxService.GetByIdAsync(id);
                return finded == null ? NotFound() : Ok(finded);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get box by id");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
