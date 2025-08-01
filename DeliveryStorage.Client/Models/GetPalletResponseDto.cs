namespace PalletApiClient.Models;

public class GetPalletResponseDto
{
    public Guid Id { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Weight { get; set; }
    public float Depth { get; set; }
    public float Volume { get; set; }
    public List<GetBoxResponseDto> Boxes { get; set; }
}