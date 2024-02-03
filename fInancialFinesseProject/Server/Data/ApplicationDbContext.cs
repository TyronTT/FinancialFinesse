using Duende.IdentityServer.EntityFramework.Options;
using fInancialFinesseProject.Server.Models;
using fInancialFinesseProject.Shared;
using fInancialFinesseProject.Shared.Domain;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.Extensions.Options;

namespace fInancialFinesseProject.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogComment> Comments { get; set; }
        public DbSet<BlogCategory> Categories { get; set; }
    }

    public class ForumDataContext : DbContext
    {
        public ForumDataContext(DbContextOptions<ForumDataContext> options) : base(options)
        {

        }

        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumComment> forumComments { get; set; }
        public DbSet<ForumCategory> fCategories { get; set; }
    }
}