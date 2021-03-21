using Cas.Account.Core.Dtos;
using Cas.Account.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cas.Account.Core.Interfaces
{
    public interface IAccountAccessor
    {
        /// <summary>
        /// Gets the <see cref="AccountModel"/> with the specified ID.
        /// </summary>
        /// <param name="id">The account model ID.</param>
        /// <returns>.</returns>
        Task<AccountModel> Get(long id);

        /// <summary>
        /// Gets the by customer identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;List&lt;AccountModel&gt;&gt;.</returns>
        Task<List<AccountModel>> GetByCustomerId(long id);
        Task<AccountModel> Create(AccountToCreateDto account);
    }
}