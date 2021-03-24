using Cas.Account.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cas.Account.DataAccess
{
    /// <summary>
    /// The DataContext Class.
    /// Used to persist data to the database.
    /// Implements the <see cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class DataContext: DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Gets the accounts model.
        /// </summary>
        /// <value>The accounts model.</value>
        public DbSet<AccountModel> Accounts { get; init; }
    }
}
