namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class StocktakeDetailsDTO
    {
        public StocktakeDTO Stocktake { get; set; } = new();
        public List<StocktakeItemDTO> Items { get; set; } = [];
    }
}
