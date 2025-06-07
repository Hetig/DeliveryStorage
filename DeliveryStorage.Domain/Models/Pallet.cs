namespace DeliveryStorage.Domain.Models;

public class Pallet
{
    public Guid Id { get; init; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Weight { get; set; }
    public List<Box>? Boxes { get; set; }
}