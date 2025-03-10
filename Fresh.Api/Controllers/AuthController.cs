using Fresh.Api.DTOs;
using Fresh.Api.Models;
using Fresh.Api.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fresh.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, JwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");

            // Token oluştur
            var token = _jwtTokenGenerator.GenerateToken(user);

            // AspNetUserTokens tablosunu doldur
            await _userManager.SetAuthenticationTokenAsync(
                user,           // Kullanıcı
                "FreshApp",     // LoginProvider (Uygulama adı gibi)
                "AuthToken",    // Token adı
                token           // Token değeri
            );

            // Kullanıcıya token döndür
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            // Yeni kullanıcı oluştur
            var user = new AppUser
            {
                UserName = model.Username,
                Ad = model.Ad,
                Soyad = model.Soyad,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
              
            };

            // Kullanıcıyı oluştur ve şifreyi ayarla
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                // Hata durumunda geri dönüş yap
                return BadRequest(result.Errors);
            }

            // Kullanıcıya rol ekle
            var roleResult = await _userManager.AddToRoleAsync(user, "user"); // "user" rolü atanıyor

            if (!roleResult.Succeeded)
            {
                // Rol ekleme sırasında hata oluşursa kullanıcıyı sil
                await _userManager.DeleteAsync(user);
                return BadRequest(new { Message = "Rol atanırken bir hata oluştu.", Errors = roleResult.Errors });
            }

            // Başarılı kayıt sonrası yanıt
            return Ok(new { Message = "Kullanıcı başarıyla oluşturuldu ve rol atandı." });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string userId)
        {
            // Kullanıcıyı al
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized("Kullanıcı bulunamadı.");
            }

            // AspNetUserTokens tablosundan token sil
            await _userManager.RemoveAuthenticationTokenAsync(
                user,           // Kullanıcı
                "FreshApp",     // LoginProvider (Token sağlayıcı, uygulama adı)
                "AuthToken"     // Token adı
            );

            // Oturum sonlandır
            await _signInManager.SignOutAsync();

            return Ok(new { Message = "Başarıyla çıkış yapıldı." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                // Önce kullanıcının rollerini kaldır
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                // Sonra kullanıcıyı sil
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Kullanıcı başarıyla silindi" });
                }

                return BadRequest(new { Message = "Kullanıcı silinirken hata oluştu", Errors = result.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Sunucu hatası: {ex.Message}" });
            }
        }
    }
}
