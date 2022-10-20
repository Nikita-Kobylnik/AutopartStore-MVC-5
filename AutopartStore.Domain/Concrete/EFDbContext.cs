using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AutopartStore.Domain.Entities;

namespace AutopartStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Autopart> Autoparts { get; set; }
    }
}
