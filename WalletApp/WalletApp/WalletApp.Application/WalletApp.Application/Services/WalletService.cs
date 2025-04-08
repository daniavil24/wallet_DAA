using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Services
{
    using WalletApp.Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using WalletApp.Application.Interfaces;
    using WalletApp.Domain.Entities;
    using WalletApp.Domain.Entities.DTOs;

    public class WalletService : IWalletService
    {
        private readonly WalletDbContext _context;

        public WalletService(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet> CreateWalletAsync(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task TransferAsync(TransferDto transfer)
        {
            if (transfer.amount <= 0)
                throw new ArgumentException("El monto debe ser mayor a cero.");

            var fromWallet = await _context.Wallets.FindAsync(transfer.fromId);
            var toWallet = await _context.Wallets.FindAsync(transfer.toId);

            if (fromWallet == null || toWallet == null)
                throw new ArgumentException("Una o ambas wallets no existen.");

            if (fromWallet.Balance < transfer.amount)
                throw new InvalidOperationException("Saldo insuficiente.");

            fromWallet.Balance -= transfer.amount;
            toWallet.Balance += transfer.amount;

            fromWallet.UpdatedAt = DateTime.UtcNow;
            toWallet.UpdatedAt = DateTime.UtcNow;

            _context.Movements.AddRange(
                new Movement { WalletId = transfer.fromId, Amount = transfer.amount, Type = "Debit" },
                new Movement { WalletId = transfer.toId, Amount = transfer.amount, Type = "Credit" });

            await _context.SaveChangesAsync();
        }

        public async Task<List<Movement>> GetMovementsAsync()
        {
            return await _context.Movements
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
    }
}
