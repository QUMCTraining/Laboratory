using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LaboratoryProject.Models;

namespace LaboratoryProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LaboratoryProject.Models.Request>? Request { get; set; }
        public DbSet<LaboratoryProject.Models.Managment>? Mangement { get; set; }
        public DbSet<LaboratoryProject.Models.College>? College { get; set; }


    }
}