using System.ComponentModel.DataAnnotations.Schema;

namespace Fresh.Api.DTOs;

public class UserWithdrawResponse
{
    public int UserWithdrawId { get; set; }
    public string Ad { get; set; }
    public string Soyad { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RequestAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal WithdrawAmount { get; set; }

    public DateTime DateTime { get; set; }

    public bool Status { get; set; }
}
