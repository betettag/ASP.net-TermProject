using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TermProject.Models
{
    public class Tournament
    {
        public int TournamentID { get; set; }
        public List<Duel> Duels { get; set; }
        //public Player TopAllTimePlayer { get; set; }
        //public Player TopWeeklyPlayer {get; set;}
        public int PlayerCount { get; set; }
        public int VoteCount { get; set; }

        public DateTime ExpiryTime { get; set; }


    }
}
