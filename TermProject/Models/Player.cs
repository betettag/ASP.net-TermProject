using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TermProject.Models
{
    public class Player
    {
        public int PlayerID { get; set; }
        //public List<Card> Hand = new List<Card>();
        public string Username { get; set; }
        public string Password { get; set; }
        public int CardID { get; set; }
        public int Score { get; set; }
        public int Votes { get; set; }
        public bool Voted { get; set; }
        public bool IsDueling { get; set; }
    }
}
