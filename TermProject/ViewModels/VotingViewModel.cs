using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;

namespace TermProject.ViewModels
{
    public class VotingViewModel
    {
        public Player player { get; set; }
        public List<Duel> Duels { get; set; }
        public Duel duel { get; set; }
    }
}
