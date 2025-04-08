using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Entities.DTOs
{
    public class AuthRQ
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
