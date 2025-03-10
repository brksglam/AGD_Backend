using Fresh.Api.DTOs;
using Fresh.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fresh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    );

                return BadRequest(new
                {
                    message = "Geçersiz giriş verileri.",
                    errors
                });
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = "Şifre sıfırlama başarısız.",
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new { message = "Şifre başarıyla sıfırlandı." });
        }
    }
}
