using Demo.Kafka.ClassLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.Kafka.Subscriber.One
{
    public class AppDbContext : DbContext
    {
        public const string ConnectionString = "";

        public DbSet<User> Users { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder<> optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        } */
    }
}
