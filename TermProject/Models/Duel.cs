using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TermProject.Models
{
    public class Duel
    {
        public Duel()
        {
            VoterID = 0;
            Player1ID = null;
            Player2ID = null;
            VotesP1 = 0;
            VotesP2 = 0;
        }
        public Card Prompt { get; set; }
        public int CardID { get; set; }
        public int DuelID { get; set; }
        public int TournamentID { get; set; }
        public List<Player> Players { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public int VoterID { get; set; }
        public int VotesP1 { get; set; }
        public int VotesP2 { get; set; }
        public string Player1ID { get; set; }
        public string Player2ID { get; set; }
    }
}
