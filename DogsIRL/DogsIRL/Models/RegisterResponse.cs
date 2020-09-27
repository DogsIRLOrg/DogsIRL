using System;
using System.Collections.Generic;
using System.Text;

namespace DogsIRL.Models
{
    public class RegisterResponse
    {
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
