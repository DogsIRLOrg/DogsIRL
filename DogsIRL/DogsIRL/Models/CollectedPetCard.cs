﻿using System;

namespace DogsIRL.Models
{
    public class CollectedPetCard
    {
        public int PetCardID { get; set; }
        public string Username { get; set; }

        // Navigation Property
        public PetCard PetCard { get; set; }

    }
}
