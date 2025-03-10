namespace Fresh.Api.DTOs
{
    public class ArbitrageResponse
    {
        public string UserId { get; set; }
        public int InvestmentId { get; set; }
        public decimal? MonthlyProfit { get; set; }
    }
}
