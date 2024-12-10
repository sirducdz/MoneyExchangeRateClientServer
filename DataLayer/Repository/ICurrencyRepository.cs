using DataLayer.Models;

namespace DataLayer.Repository
{
    public interface ICurrencyRepository : IGenericRepository<Currency>
    {
        public Task<List<Currency>> GetAllCurrenciesIncluded();
    }
}
