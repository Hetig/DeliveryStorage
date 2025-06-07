using System.ComponentModel.DataAnnotations;

namespace DeliveryStorage.API.Dtos;

public class CreateBoxDto
{
    [Required(ErrorMessage = "Дата производства это обязательное поле")]
    [DataType(DataType.Date)]
    public DateOnly ProductionDate { get; set; }
    
    [Required(ErrorMessage = "Ширина это обязательное поле")]
    [Range(0.1f, float.MaxValue, ErrorMessage = "Ширина должна быть больше 0")]
    public float Width { get; set; }
    
    [Required(ErrorMessage = "Высота это обязательное поле")]
    [Range(0.1f, float.MaxValue, ErrorMessage = "Высота должна быть больше 0")]
    public float Height { get; set; }
    
    [Required(ErrorMessage = "Вес это обязательное поле")]
    [Range(0.01f, float.MaxValue, ErrorMessage = "Вес должен быть больше 0")]
    public float Weight { get; set; }
}