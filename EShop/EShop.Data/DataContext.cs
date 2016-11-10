using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") { }

        public DbSet<DomainModels.Member> Members { get; set; }

        public DbSet<DomainModels.Property> Propertys { get; set; }
    }
}
