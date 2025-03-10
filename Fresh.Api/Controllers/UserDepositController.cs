using Fresh.Api.Data;
using Fresh.Api.DTOs;
using Fresh.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fresh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDepositController : ControllerBase
    {

        private readonly FreshDbContext _context;
        public UserDepositController(FreshDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> AddUserDeposit(AddUserDepositRequest request)
        {

            UserDeposit userDeposit = new();

            userDeposit.UserId = request.UserId;
            userDeposit.Ad = request.Ad;
            userDeposit.Soyad = request.Soyad;
            userDeposit.Amount = request.Amount;
            userDeposit.DateTime = request.DateTime;


            _context.UserDeposits.Add(userDeposit);

            await _context.SaveChangesAsync();



            return Ok("İşlem Başarılı");
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllUserByIdDeposit(string userId)
        {

            var result = await _context.UserDeposits.Where(x => x.UserId == userId).ToListAsync();


            if (!result.Any())
            {

                return NotFound("Yatırma İşlemi bulunmamaktadır");

            }


            List<UserDepositRespose> resposes = new();

            foreach (var item in result)
            {
                UserDepositRespose userDepositRespose = new();

                userDepositRespose.Ad = item.Ad;
                userDepositRespose.Soyad = item.Soyad;
                userDepositRespose.Amount = item.Amount;
                userDepositRespose.DateTime = item.DateTime;

                resposes.Add(userDepositRespose);
            }



            return Ok(resposes);
        }



    }
}
