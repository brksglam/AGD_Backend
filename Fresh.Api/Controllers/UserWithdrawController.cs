using Fresh.Api.Data;
using Fresh.Api.DTOs;
using Fresh.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fresh.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserWithdrawController : ControllerBase
{
    

    private readonly FreshDbContext _context;
    public UserWithdrawController(FreshDbContext context)
    {
        _context = context;
    }


    [HttpPost]
    public async Task<IActionResult> AddUserWithdraw(AddUserWithdrawRequest request)
    {

        UserWithdraw userWithdraw = new();

        userWithdraw.UserId = request.UserId;
        userWithdraw.Ad = request.Ad;
        userWithdraw.Soyad = request.Soyad;
        userWithdraw.RequestAmount = request.RequestAmount;
        userWithdraw.WithdrawAmount = request.WithdrawAmount;
        userWithdraw.DateTime = request.DateTime;
        userWithdraw.Status = false;


        _context.UserWithdraws.Add(userWithdraw);

        await _context.SaveChangesAsync();



        return Ok("İşlem Başarılı");
    }


    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAllUserByIdWithdraw(string userId)
    {

        var result = await _context.UserWithdraws.Where(x => x.UserId == userId).ToListAsync();


        if (!result.Any())
        {

            return NotFound("Yatırma İşlemi bulunmamaktadır");

        }


        List<UserWithdrawResponse> resposes = new();

        foreach (var item in result)
        {
            UserWithdrawResponse userWithdrawResponse = new();
            userWithdrawResponse.UserWithdrawId = item.Id;
            userWithdrawResponse.Ad = item.Ad;
            userWithdrawResponse.Soyad = item.Soyad;
            userWithdrawResponse.RequestAmount = item.RequestAmount;
            userWithdrawResponse.WithdrawAmount = item.WithdrawAmount;
            userWithdrawResponse.Status = item.Status;
            userWithdrawResponse.DateTime = item.DateTime;

            resposes.Add(userWithdrawResponse);
        }



        return Ok(resposes);
    }


    [HttpPost("update")]
    public async Task<IActionResult> UpdateUserByIdWithdraw(UpdateUserByIdWithdrawRequest request)
    {

        var result=await _context.UserWithdraws.Where(x=>x.Id == request.UserWithdrawId).FirstOrDefaultAsync();    
        
        result.WithdrawAmount=request.WithdrawAmount;

        result.Status = true;

        _context.UserWithdraws.Update(result);

        await _context.SaveChangesAsync();



        return Ok("İşlem Başarılı");
    }


}
