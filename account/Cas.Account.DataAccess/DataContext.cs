using Cas.Account.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Cas.Account.DataAccess
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options, ILogger<DataContext> logger) : base(options)
        {
            try
            {
                this.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during migration");
            }
        }

        public DbSet<AccountModel> Accounts { get; init; }
    }
}
