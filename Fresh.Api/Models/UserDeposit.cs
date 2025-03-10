using System.ComponentModel.DataAnnotations.Schema;

namespace Fresh.Api.Models;

public class UserDeposit
{
    public int Id { get; set; }
    public string UserId { get; set; }

    public string Ad { get; set; }
    public string Soyad { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime DateTime { get; set; }

}
