using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Interfaces;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Entities.DTOs;

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

        [Authorize]
        [HttpPost("create_wallet")]
        public async Task<IActionResult> CreateWallet([FromBody] Wallet wallet)
        {
            var result = await _walletService.CreateWalletAsync(wallet);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferDto transfer)
        {
            await _walletService.TransferAsync(transfer);
            return Ok("Transferencia realizada correctamente.");
        }

        [AllowAnonymous]
        [HttpGet("movements")]
        public async Task<IActionResult> GetMovements()
        {
            var movements = await _walletService.GetMovementsAsync();
            return Ok(movements);
        }
    }
}
