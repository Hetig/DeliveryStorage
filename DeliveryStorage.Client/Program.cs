using System.Net.Http.Json;
using PalletApiClient.Models;

namespace PalletApiClient
{
    class Program
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "http://localhost:8080/api/v1/Pallet";

        static async Task Main(string[] args)
        {
            try
            {
                var pallets = await GetAllPallets();
                
                if (pallets == null || !pallets.Any())
                {
                    Console.WriteLine("Не найдено ни одной паллеты.");
                    return;
                }

                DisplayPalletsGroupedByExpiration(pallets);

                DisplayTop3PalletsByBoxExpiration(pallets);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        private static async Task<List<GetPalletResponseDto>> GetAllPallets()
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<GetPalletResponseDto>>();
        }

        private static void DisplayPalletsGroupedByExpiration(List<GetPalletResponseDto> pallets)
        {
            Console.WriteLine("Паллеты, сгруппированные по сроку годности:");
            Console.WriteLine("-------------------------------------------");

            var groupedPallets = pallets
                .GroupBy(p => p.ExpirationDate)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    ExpirationDate = g.Key,
                    Pallets = g.OrderBy(p => p.Weight)
                });

            foreach (var group in groupedPallets)
            {
                Console.WriteLine($"\nСрок годности: {group.ExpirationDate:yyyy-MM-dd}");
                Console.WriteLine("-------------------------------------------");
                
                foreach (var pallet in group.Pallets)
                {
                    Console.WriteLine($"ID: {pallet.Id}, Вес: {pallet.Weight}, Объем: {pallet.Volume}");
                    Console.WriteLine($"Кол-во коробок: {pallet.Boxes?.Count ?? 0}");
                }
            }
        }

        private static void DisplayTop3PalletsByBoxExpiration(List<GetPalletResponseDto> pallets)
        {
            Console.WriteLine("\n\n\nТоп 3 паллеты с коробками наибольшим сроком годности:");
            Console.WriteLine("--------------------------------------------------");

            var palletsWithMaxBoxExpiration = pallets
                .Where(p => p.Boxes != null && p.Boxes.Any())
                .Select(p => new
                {
                    Pallet = p,
                    MaxBoxExpiration = p.ExpirationDate
                })
                .OrderByDescending(x => x.MaxBoxExpiration)
                .Take(3)
                .OrderBy(x => x.Pallet.Volume)
                .ToList();

            if (!palletsWithMaxBoxExpiration.Any())
            {
                Console.WriteLine("Не найдено паллет с коробками.");
                return;
            }

            foreach (var item in palletsWithMaxBoxExpiration)
            {
                Console.WriteLine($"\nПаллет ID: {item.Pallet.Id}");
                Console.WriteLine($"Объем: {item.Pallet.Volume}");
                Console.WriteLine($"Максимальный срок годности коробки: {item.MaxBoxExpiration:yyyy-MM-dd}");
                Console.WriteLine($"Всего коробок: {item.Pallet.Boxes.Count}");
            }
        }
    }
}