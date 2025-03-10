using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fresh.Api.Models
{
    public class Investment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string ContractNumber { get; set; } // Sözleşme numarası

        [Required]
        [StringLength(100)]
        public string AccountNumber { get; set; } // Hesap numarası

        [Required]
        public int ShareCount { get; set; } // Hisse adedi

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal NominalValue { get; set; } // Nominal tutar

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentShareValue { get; set; } // Güncel hisse değeri

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentFundValue { get; set; } // Fon güncel değeri

        [Required]
        public string Type { get; set; } // "Arbitrage" veya "FundManagement"

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyProfit { get; set; } // Arbitraj için aylık kar

        [Column(TypeName = "decimal(18,2)")]
        public decimal? WeeklyProfit { get; set; } // Fon yönetimi için haftalık kar

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Yatırımın oluşturulma tarihi
    }
}
