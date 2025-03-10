using Fresh.Api.Data;
using Fresh.Api.DTOs;
using Fresh.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fresh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountInfoController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly FreshDbContext _freshDbContext;

        public AccountInfoController(UserManager<AppUser> userManager, FreshDbContext freshDbContext)
        {
            _userManager = userManager;
            _freshDbContext = freshDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _userManager.Users.ToListAsync();

            if (!result.Any())
            {
                return NotFound("Kullanıcı yok");
            }

            List<AllUserRespone> users = result.Select(user => new AllUserRespone
            {
                Ad = user.Ad,
                Soyad = user.Soyad,
                Id = user.Id
            }).ToList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _userManager.FindByIdAsync(id);

            if (result == null)
            {
                return NotFound("Kullanıcı yok");
            }

            var investment = await _freshDbContext.Investments
                .Where(x => x.UserId == result.Id)
                .FirstOrDefaultAsync();

            UserByIdResponse response = new()
            {
                Ad = result.Ad,
                Soyad = result.Soyad,
                HesapNo = investment?.AccountNumber, // Null kontrolü
                SozlesmeNo = investment?.ContractNumber // Null kontrolü
            };

            return Ok(response);
        }
    }
}
