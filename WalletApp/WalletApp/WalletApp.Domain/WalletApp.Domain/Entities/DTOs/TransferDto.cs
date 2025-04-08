using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Entities.DTOs
{
    public class TransferDto
    {
        public int fromId { get; set; }
        public int toId { get; set; }
        public decimal amount { get; set; }
    }
}
