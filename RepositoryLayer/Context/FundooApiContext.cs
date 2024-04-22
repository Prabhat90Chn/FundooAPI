using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Context
{
    public class FundooApiContext : DbContext
    {
        public FundooApiContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<UserEntity> User { get; set; }
    }
}
