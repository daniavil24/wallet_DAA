using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Interfaces
{
    public interface IWalletService
    {
        Task<Wallet> CreateWalletAsync(Wallet wallet);
        Task TransferAsync(int fromWalletId, int toWalletId, decimal amount);
        Task<List<Movement>> GetMovementsAsync(int walletId);
    }
}
