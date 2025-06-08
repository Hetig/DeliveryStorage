using System.ComponentModel.DataAnnotations;

namespace DeliveryStorage.API.Dtos;

public class UpdatePalletQueryDto
{
    [Required(ErrorMessage = "Id это обязательное поле")]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Ширина это обязательное поле")]
    [Range(0.1f, float.MaxValue, ErrorMessage = "Ширина должна быть больше 0")]
    public float Width { get; set; }
    
    [Required(ErrorMessage = "Высота это обязательное поле")]
    [Range(0.1f, float.MaxValue, ErrorMessage = "Высота должна быть больше 0")]
    public float Height { get; set; }
    
    [Required(ErrorMessage = "Глубина это обязательное поле")]
    [Range(0.1f, float.MaxValue, ErrorMessage = "Глубина должна быть больше 0")]
    public float Depth { get; set; }
}