namespace Fresh.Api.DTOs
{
    public class FundManagementResponse
    {
        public string UserId { get; set; }
        public int InvestmentId { get; set; }
        public decimal? WeeklyProfit { get; set; }
    }
}
