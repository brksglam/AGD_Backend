using System.ComponentModel.DataAnnotations;

namespace Fresh.Api.DTOs;

public class ResetPasswordRequest
{
    public string UserId { get; set; }

    public string Password { get; set; }

    [Required(ErrorMessage = "Şifre tekrar alanı zorunludur.")]
    [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
    public string RePassword { get; set; }


}

