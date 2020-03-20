using TermProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace TermProject.Repositories
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(
           DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext() : base() { }
        public DbSet<Card> Cards { get; set; }
        //public DbSet<Player> Players { get; set; }
        public DbSet<Duel> Duels { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
    }
}