using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TermProject.Models
{
    public class Player : IdentityUser
    {
        public int PlayerID { get; set; }
        public Card DuelCard { get; set;}
        public string Password { get; set; }
        public int CardID { get; set; }
        public int PromtID { get; set; }
        public int Score { get; set; }
        public bool Voted { get; set; }
        public bool IsDueling { get; set; }
    }
}
