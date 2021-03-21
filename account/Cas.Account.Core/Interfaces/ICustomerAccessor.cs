using System.Threading.Tasks;

namespace Cas.Account.Core.Interfaces
{
    public interface ICustomerAccessor
    {
        Task<bool> Exists(long id);
    }
}