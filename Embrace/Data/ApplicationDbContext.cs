using Embrace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Embrace.Data
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure the many-to-many relationship
            builder.Entity<ResourceServiceCategories>()
                .HasKey(rsc => new { rsc.ResourceId, rsc.ServiceCategoryId });

            builder.Entity<ResourceServiceCategories>()
                .HasOne(rsc => rsc.Resource)
                .WithMany(r => r.ServiceCategories)
                .HasForeignKey(rsc => rsc.ResourceId);
            
            builder.Entity<ResourceServiceCategories>()
                .HasOne(rsc => rsc.ServiceCategory)
                .WithMany(sc => sc.Resources)
                .HasForeignKey(rsc => rsc.ServiceCategoryId);

            builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = "1", Name = "Administrator" });
            builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = "2", Name = "User" });
        }


        public DbSet<Resource> Resources { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<ResourceServiceCategories> ResourceServiceCategories { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DiscussionBoard> DiscussionBoards { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
