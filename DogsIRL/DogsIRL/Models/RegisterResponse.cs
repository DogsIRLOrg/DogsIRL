using System;
using System.Collections.Generic;
using System.Text;

namespace DogsIRL.Models
{
    class RegisterResponse
    {
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
