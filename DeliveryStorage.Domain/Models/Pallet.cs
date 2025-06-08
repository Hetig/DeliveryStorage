namespace DeliveryStorage.Domain.Models;

public class Pallet
{
    public Guid Id { get; init; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Depth { get; set; }

    public float Weight => 30 + (Boxes?.Sum(box => box.Weight) ?? 0);
    public float Volume => Width * Depth * Height + (Boxes?.Sum(box => box.Volume) ?? 0);
    public DateOnly? ExpirationDate => Boxes == null || Boxes.Count == 0 ? null : Boxes.Min(box => box.ProductionDate)
                                                                                        .AddDays(100);
    public List<Box>? Boxes { get; set; }
}