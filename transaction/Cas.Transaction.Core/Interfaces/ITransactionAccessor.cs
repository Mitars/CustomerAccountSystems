using Cas.Transaction.Core.Dtos;
using Cas.Transaction.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cas.Transaction.Core.Interfaces
{
    public interface ITransactionAccessor
    {
        Task<TransactionModel> Get(long id);
        Task<List<TransactionModel>> GetByAccountId(long accountId);
        Task<TransactionModel> Create(TransactionToCreateDto account);
    }
}
