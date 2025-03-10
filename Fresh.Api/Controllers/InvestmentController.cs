using Fresh.Api.DTOs;
using Fresh.Api.Models;
using Fresh.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fresh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentController : ControllerBase
    {
        private readonly FreshDbContext _context;

        public InvestmentController(FreshDbContext context)
        {
            _context = context;
        }

        // GET: api/Investment/Arbitrage
        [HttpGet("Arbitrage")]
        public async Task<IActionResult> GetArbitrageInvestments()
        {
            var arbitrageInvestments = await _context.Investments
                .Where(i => i.Type == "Arbitrage")
                .ToListAsync();

            if (!arbitrageInvestments.Any())
            {
                return NotFound("Arbitraj yatırımı bulunamadı");
            }

            var response = arbitrageInvestments.Select(i => new
            {
                i.Id,
                i.UserId,
                i.ContractNumber,
                i.AccountNumber,
                i.ShareCount,
                i.NominalValue,
                i.CurrentShareValue,
                i.CurrentFundValue,
                i.MonthlyProfit,
                i.CreatedDate
            }).ToList();

            return Ok(response);
        }

        // GET: api/Investment/FundManagement
        [HttpGet("FundManagement")]
        public async Task<IActionResult> GetFundManagementInvestments()
        {
            var fundInvestments = await _context.Investments
                .Where(i => i.Type == "FundManagement")
                .ToListAsync();

            if (!fundInvestments.Any())
            {
                return NotFound("Fon yönetimi yatırımı bulunamadı");
            }

            var response = fundInvestments.Select(i => new
            {
                i.Id,
                i.UserId,
                i.ContractNumber,
                i.AccountNumber,
                i.ShareCount,
                i.NominalValue,
                i.CurrentShareValue,
                i.CurrentFundValue,
                i.WeeklyProfit,
                i.CreatedDate
            }).ToList();

            return Ok(response);
        }

        // GET: api/Investment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvestmentById(string id)
        {
            var investment = await _context.Investments.Where(x=>x.UserId==id).FirstOrDefaultAsync();

            if (investment == null)
            {
                return NotFound("Yatırım bulunamadı");
            }

            return Ok(new
            {
                investment.Id,
                investment.UserId,
                investment.ContractNumber,
                investment.AccountNumber,
                investment.ShareCount,
                investment.NominalValue,
                investment.CurrentShareValue,
                investment.CurrentFundValue,
                investment.Type,
                investment.MonthlyProfit,
                investment.WeeklyProfit,
                investment.CreatedDate
            });
        }

        // POST: api/Investment
        [HttpPost]
        public async Task<IActionResult> CreateInvestment([FromBody] InvestmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value.Errors.Select(e =>
                            e.ErrorMessage.Contains("System.Int32")
                            ? "Hisse adedi yalnızca tam sayı olmalıdır."
                            : e.ErrorMessage
                        ).ToList()
                    );

                return BadRequest(new
                {
                    message = "Geçersiz giriş verileri.",
                    errors
                });
            }


            var newInvestment = new Investment
            {
                UserId = request.UserId,
                ContractNumber = request.ContractNumber,
                AccountNumber = request.AccountNumber,
                ShareCount = request.ShareCount,
                NominalValue = request.NominalValue,
                CurrentShareValue = request.CurrentShareValue,
                CurrentFundValue = request.CurrentFundValue,
                Type = request.Type,
                MonthlyProfit = request.Type == "Arbitrage" ? request.Profit : null,
                WeeklyProfit = request.Type == "FundManagement" ? request.Profit : null,
                CreatedDate = DateTime.Now
            };

            await _context.Investments.AddAsync(newInvestment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInvestmentById), new { id = newInvestment.Id }, newInvestment);
        }

    }
}
