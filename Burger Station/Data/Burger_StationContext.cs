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
        public DbSet<Burger_Station.Models.Branch> Branch { get; set; }
        public DbSet<Burger_Station.Models.Comment> Comment { get; set; }
        public DbSet<Burger_Station.Models.Item> Item { get; set; }
        public DbSet<Burger_Station.Models.Order> Order { get; set; }
        public DbSet<Burger_Station.Models.User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemOrder>().HasKey(itemoOrder=> new { itemoOrder.ItemId, itemoOrder.OrderId });

            modelBuilder.Entity<ItemOrder>()
                .HasOne(itemOrder => itemOrder.Item)
                .WithMany(order => order.OrdersOfItem)
                .HasForeignKey(itemoOrder => itemoOrder.ItemId);

            modelBuilder.Entity<ItemOrder>()
                .HasOne(itemOrder => itemOrder.Order)
                .WithMany(order => order.ItemsOrders)
                .HasForeignKey(itemoOrder => itemoOrder.OrderId);
        }

        public DbSet<Burger_Station.Models.ItemOrder> ItemOrder { get; set; }
    }
}