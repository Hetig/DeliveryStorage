using AutoMapper;
using DeliveryStorage.API.Dtos;
using DeliveryStorage.Domain.Interfaces;
using DeliveryStorage.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryStorage.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PalletController : ControllerBase
    {
        private readonly IPalletService _palletService;
        private readonly ILogger<PalletController> _logger;
        private readonly IMapper _mapper;

        public PalletController(IPalletService palletService, ILogger<PalletController> logger, IMapper mapper)
        {
            _palletService = palletService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var pallets = await _palletService.GetAllAsync();
                return Ok(_mapper.Map<List<GetPalletResponseDto>>(pallets));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all pallets");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePalletQueryDto palletQuery)
        {
            try
            {
                var created = await _palletService.AddAsync(_mapper.Map<Pallet>(palletQuery));
                return Created("api/v1/Box", created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create pallet");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePalletQueryDto updatePalletQuery)
        {
            try
            {
                var updated = await _palletService.UpdateAsync(_mapper.Map<Pallet>(updatePalletQuery));
                return updated == null ? NotFound() : Ok(_mapper.Map<UpdatePalletResponseDto>(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update pallet");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var isDeleted = await _palletService.DeleteAsync(id);
                return isDeleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete pallet");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var finded = await _palletService.GetByIdAsync(id);
                return finded == null ? NotFound() : Ok(_mapper.Map<GetPalletResponseDto>(finded));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get pallet by id");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
