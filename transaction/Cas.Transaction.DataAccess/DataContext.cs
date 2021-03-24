using Cas.Transaction.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cas.Transaction.DataAccess
{
    /// <summary>
    /// The DataContext class.
    /// Used to persist data to the database.
    /// Implements the <see cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class DataContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Gets the transactions model.
        /// </summary>
        /// <value>The transactions model.</value>
        public DbSet<TransactionModel> Transactions { get; init; }
    }
}
