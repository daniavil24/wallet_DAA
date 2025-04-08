using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Entities.DTOs;

namespace WalletApp.Application.Interfaces
{
    public interface IWalletService
    {
        Task<Wallet> CreateWalletAsync(Wallet wallet);
        Task TransferAsync(TransferDto transfer);
        Task<List<Movement>> GetMovementsAsync();
    }
}
