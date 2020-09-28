using System;
using System.Collections.Generic;
using System.Text;

namespace DogsIRL.Models
{
    public class LoginJwt
    {
        public string Jwt { get; set; }
        public DateTime Expiration { get; set; }
        public string Username { get; set; }
    }
}
