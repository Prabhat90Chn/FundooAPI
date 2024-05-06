using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class FundooApiContext : DbContext
    {
        public FundooApiContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<UserEntity> User { get; set; }
        public DbSet<UserNote> Notes { get; set; }
    }
}
