using System;
namespace DogsIRL.Models
{
    public class PetCard
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Sex { get; set; }
        public string Owner { get; set; }
        public int AgeYears { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateCollected { get; set; }
        public sbyte GoodDog { get; set; }
        public sbyte Floofiness { get; set; }
        public sbyte Energy { get; set; }
        public sbyte Snuggles { get; set; }
        public sbyte Appetite { get; set; }
        public sbyte Bravery { get; set; }
    }
}
