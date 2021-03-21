using System.Threading.Tasks;

namespace Cas.Transaction.Core.Interfaces
{
    public interface IAccountAccessor
    {
        Task<bool> Exists(long id);
    }
}