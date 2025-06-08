namespace DeliveryStorage.API.Dtos;

public class GetBoxResponseDto
{
    public Guid Id { get; set; }
    public DateOnly ProductionDate { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Depth { get; set; }
    public float Weight { get; set; }
}