using System.ComponentModel.DataAnnotations;
using DeliveryStorage.API.Attributes;

namespace DeliveryStorage.API.Dtos;

public class AssignBoxesQueryDto
{
    
    [Required(ErrorMessage = "PalletId это обязательное поле")]
    public Guid PalletId { get; init; }
    
    
    [Required(ErrorMessage = "BoxesId это обязательное поле")]
    [UniqueItems(ErrorMessage = "Список BoxesId содержит дубликаты")]
    public List<Guid> BoxesId { get; init; }
}