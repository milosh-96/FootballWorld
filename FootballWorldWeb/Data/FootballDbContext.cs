using FootballWorld.Data;
using FootballWorldWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Data
{
    public class FootballDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,int>
    {

        public FootballDbContext(DbContextOptions options) : base(options)
        {
        }

     
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<GroupTeam>().HasKey(cs => new { cs.GroupId, cs.TeamId });
        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupTeam> GroupTeams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchResult> MatchResults { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Standings> Standings { get; set; }
        public DbSet<StandingsRow> StandingRows { get; set; }

    }
}
