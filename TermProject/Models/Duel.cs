using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TermProject.Models
{
    public class Duel
    {
        public Card Prompt { get; set; }
        public int PromptID { get; set; }
        public int DuelID { get; set; }
        public int TournamentID { get; set; }
        public List<Player> Players { get; set; }
        public List<Card> Cards { get; set; }
    }
}
