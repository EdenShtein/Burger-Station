﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Burger_Station.Models;

namespace Burger_Station.Data
{
    public class Burger_StationContext : DbContext
    {
        public Burger_StationContext (DbContextOptions<Burger_StationContext> options)
            : base(options)
        {
        }

        public DbSet<Burger_Station.Models.MenuItem> MenuItem { get; set; }
    }
}