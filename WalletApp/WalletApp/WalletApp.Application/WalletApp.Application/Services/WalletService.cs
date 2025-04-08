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

        public async Task TransferAsync(int fromWalletId, int toWalletId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("El monto debe ser mayor a cero.");

            var fromWallet = await _context.Wallets.FindAsync(fromWalletId);
            var toWallet = await _context.Wallets.FindAsync(toWalletId);

            if (fromWallet == null || toWallet == null)
                throw new ArgumentException("Una o ambas wallets no existen.");

            if (fromWallet.Balance < amount)
                throw new InvalidOperationException("Saldo insuficiente.");

            fromWallet.Balance -= amount;
            toWallet.Balance += amount;

            fromWallet.UpdatedAt = DateTime.UtcNow;
            toWallet.UpdatedAt = DateTime.UtcNow;

            _context.Movements.AddRange(
                new Movement { WalletId = fromWalletId, Amount = amount, Type = "Debit" },
                new Movement { WalletId = toWalletId, Amount = amount, Type = "Credit" });

            await _context.SaveChangesAsync();
        }

        public async Task<List<Movement>> GetMovementsAsync(int walletId)
        {
            return await _context.Movements
                .Where(m => m.WalletId == walletId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
    }
}
