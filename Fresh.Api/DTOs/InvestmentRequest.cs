using System.ComponentModel.DataAnnotations;

namespace Fresh.Api.DTOs
{
    public class InvestmentRequest
    {
        [Required(ErrorMessage = "Kullanıcı kimliği zorunludur.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Sözleşme numarası zorunludur.")]
        public string ContractNumber { get; set; }

        [Required(ErrorMessage = "Hesap numarası zorunludur.")]
        public string AccountNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Hisse adedi pozitif tam sayı olmalıdır.")]
        public int ShareCount { get; set; }

        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Nominal değer 0'dan büyük bir ondalıklı sayı olmalıdır.")]
        public decimal NominalValue { get; set; }

        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Hisse güncel değeri 0'dan büyük bir ondalıklı sayı olmalıdır.")]
        public decimal CurrentShareValue { get; set; }

        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Fon güncel değeri 0'dan büyük bir ondalıklı sayı olmalıdır.")]
        public decimal CurrentFundValue { get; set; }

        [Required(ErrorMessage = "Yatırım tipi zorunludur.")]
        [RegularExpression("^(Arbitrage|FundManagement)$", ErrorMessage = "Yatırım tipi yalnızca 'Arbitrage' veya 'FundManagement' olabilir.")]
        public string Type { get; set; } // "Arbitrage" veya "FundManagement"

        /// <summary>
        /// Eğer Type "Arbitrage" ise bu değer MonthlyProfit, "FundManagement" ise WeeklyProfit olarak atanacaktır.
        /// </summary>
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Kar oranı 0'dan büyük bir ondalıklı sayı olmalıdır.")]
        public decimal? Profit { get; set; }
    }
}
