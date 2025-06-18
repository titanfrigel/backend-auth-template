using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendAuthTemplate.Application.Common.Settings
{
    public class SmtpSenderSettings
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string Email { get; init; }
        public required string Name { get; init; }
    }
}
