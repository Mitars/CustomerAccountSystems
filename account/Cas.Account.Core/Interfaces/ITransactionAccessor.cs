using Cas.Account.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cas.Account.Core.Interfaces
{
    public interface ITransactionAccessor
    {
        Task<List<TransactionDetailsDto>> GetByAccountId(long id);
        Task<TransactionDetailsDto> Create(TransactionToCreateDto transaction);
    }
}