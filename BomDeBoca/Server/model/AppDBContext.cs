using BomDeBoca.Client.Pages;
using BomDeBoca.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BomDeBoca.Server.model {
    public class AppDBContext : IdentityDbContext {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Shared.Entities.Client> Clients { get; set; }
        public DbSet<CompanyFeedback> CompanyFeedbacks { get; set; }
        public DbSet<Shared.Entities.Company> Companies { get; set; }
    }
}
