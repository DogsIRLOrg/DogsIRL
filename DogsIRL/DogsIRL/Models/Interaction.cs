using System;
using System.Collections.Generic;
using System.Text;

namespace DogsIRL.Models
{
    public class Interaction
    {
        public int Id { get; set; }
        public string OpeningLine { get; set; }
        public string OpeningLineOther { get; set; }
        public string ConversationLine { get; set; }
        public string GoodbyeLine { get; set; }
        public string GoodbyeLineOther { get; set; }
    }
}
