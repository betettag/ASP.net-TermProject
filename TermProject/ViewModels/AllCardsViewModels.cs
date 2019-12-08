using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;

namespace TermProject.ViewModels
{
    public class AllCardsViewModels
    {
        public Player player { get; set; }
        public List<Card> Cards { get; set; }
    }
}
