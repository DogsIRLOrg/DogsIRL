using System;
using System.Collections.Generic;
using System.Text;

namespace DogsIRL.Models
{
    public class Petting
    {
        public int PetCardId { get; set; }
        public string Username { get; set; }
        public DateTime DateTimePetted { get; set; }
        public PetCard PetCard { get; set; }
    }
}
