using Domain.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
    {
        private readonly ICurrentUserService currentUserService;

        public AppDbContext(DbContextOptions<AppDbContext> options,
            ICurrentUserService currentUserService) : base(options) 
        {
            this.currentUserService = currentUserService;
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AuditableEntity>().UseTpcMappingStrategy().HasQueryFilter(x => !x.IsDeleted);
            base.OnModelCreating(builder);
        }

    }
}
