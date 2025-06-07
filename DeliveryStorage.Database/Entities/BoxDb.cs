namespace DeliveryStorage.Database.Entities;

public class BoxDb
{
    public Guid Id { get; set; }
    public DateOnly ProductionDate { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Weight { get; set; }
    public PalletDb? Pallet { get; set; }
    private Guid? PalletId { get; set; }
}