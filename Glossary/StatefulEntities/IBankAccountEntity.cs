using System.Threading.Tasks;

namespace StatefulEntities
{
    public interface IBankAccountEntity
    {
        Task ModifyBalanceAsync(decimal delta);
        Task<decimal> GetBalanceAsync();
    }
}
