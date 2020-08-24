using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Burger_Station.Models;

namespace Burger_Station.Data
{
    public class Burger_StationContext : DbContext
    {
        public Burger_StationContext(DbContextOptions<Burger_StationContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BranchItem>()
                .HasKey(bi => new { bi.BranchId, bi.ItemId });

            modelBuilder.Entity<BranchItem>()
                .HasOne(bi => bi.Item)
                .WithMany(i => i.BranchItems)
                .HasForeignKey(bi => bi.ItemId);

            modelBuilder.Entity<BranchItem>()
                .HasOne(bi => bi.Branch)
                .WithMany(b => b.BranchItems)
                .HasForeignKey(bi => bi.BranchId);
        }

        public DbSet<Burger_Station.Models.Item> Item { get; set; }

        public DbSet<Burger_Station.Models.User> User { get; set; }

        public DbSet<Burger_Station.Models.Branch> Branch { get; set; }

        public DbSet<Burger_Station.Models.BranchItem> BranchItem { get; set; }

        public DbSet<Burger_Station.Models.Comment> Comment { get; set; }
    }
}
