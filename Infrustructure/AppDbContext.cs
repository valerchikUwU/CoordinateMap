using CoordinateMap.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CoordinateMap.Infrustructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Coordinates> Coordinates { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CoordinateMapDb;Connect Timeout=30;Encrypt=False");
        }
    }
}
