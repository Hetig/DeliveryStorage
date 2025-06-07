namespace DeliveryStorage.Database.Entities;

public class PalletDb
{
    public Guid Id { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Weight { get; set; }
    public List<BoxDb>? Boxes { get; set; }
}