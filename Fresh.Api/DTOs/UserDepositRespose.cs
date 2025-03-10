using System.ComponentModel.DataAnnotations.Schema;

namespace Fresh.Api.DTOs;

public class UserDepositRespose
{
  

    public string Ad { get; set; }
    public string Soyad { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime DateTime { get; set; }
}
