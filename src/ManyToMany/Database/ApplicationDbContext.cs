using Microsoft.EntityFrameworkCore;
using ManyToMany.Models;

namespace ManyToMany.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerDevice>()
            .HasKey(c => new { c.CustomerId, c.DeviceId });

            modelBuilder.Entity<CustomerDevice>()
                .HasOne(cd => cd.Customer)
                .WithMany(c => c.CustomerDevices)
                .HasForeignKey(cd => cd.CustomerId);

            modelBuilder.Entity<CustomerDevice>()
                .HasOne(cd => cd.Device)
                .WithMany(d => d.CustomerDevices)
                .HasForeignKey(cd => cd.DeviceId);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<CustomerDevice> CustomerDevices { get; set; }
    }
}
