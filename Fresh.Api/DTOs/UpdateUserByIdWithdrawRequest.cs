using System.ComponentModel.DataAnnotations.Schema;

namespace Fresh.Api.DTOs;

public class UpdateUserByIdWithdrawRequest
{
    public int UserWithdrawId { get; set; }


    [Column(TypeName = "decimal(18,2)")]
    public decimal WithdrawAmount { get; set; }
}
