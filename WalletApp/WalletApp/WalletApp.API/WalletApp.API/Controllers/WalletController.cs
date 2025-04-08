using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Interfaces;
using WalletApp.Domain.Entities;

namespace WalletApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] Wallet wallet)
        {
            var result = await _walletService.CreateWalletAsync(wallet);
            return Ok(result);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(int fromId, int toId, decimal amount)
        {
            await _walletService.TransferAsync(fromId, toId, amount);
            return Ok("Transferencia realizada correctamente.");
        }

        [HttpGet("{walletId}/movements")]
        public async Task<IActionResult> GetMovements(int walletId)
        {
            var movements = await _walletService.GetMovementsAsync(walletId);
            return Ok(movements);
        }
    }
}
