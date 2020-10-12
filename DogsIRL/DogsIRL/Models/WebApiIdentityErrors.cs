using System;
using System.Collections.Generic;

namespace DogsIRL.Models
{
    public class WebApiIdentityErrors
    {
        public IEnumerable<string> Username { get; set; }
        public IEnumerable<string> Email { get; set; }
        public IEnumerable<string> Password { get; set; }
        public IEnumerable<string> ConfirmPassword { get; set; }
    }
}
