using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;

namespace TermProject.ViewModels
{
    public class HighScoresViewModel
    {
        public Player player { get; set; }
        public List<Player> Players { get; set; }
    }
}
