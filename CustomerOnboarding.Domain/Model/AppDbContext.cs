using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Model
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<LocalGovernment> LocalGovernments { get; set; }
        public DbSet<OTPModel> OTPModels { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
               .Property("Id").UseIdentityColumn();

            modelBuilder.Entity<Customer>()
                .HasOne(b => b.User)
                .WithOne();


            modelBuilder.Entity<ApplicationRole>()
              .Property("Id").UseIdentityColumn();


            modelBuilder.Entity<OTPModel>()
                .HasNoKey();

          modelBuilder.ApplyConfiguration(new StateConfiguration());

            modelBuilder.ApplyConfiguration(new LocalGovernmentConfiguration());
            



        }
    }
}
