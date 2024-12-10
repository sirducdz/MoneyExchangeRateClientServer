
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(Prn221ProjectContext context) : base(context)
        {
        }

        public async Task<List<Currency>> GetAllCurrenciesIncluded()
        {
            return await _context.Currencies.Include(c => c.RateHistorySourceCurrencies)
                .Include(m => m.RateHistoryTargetCurrencies).ToListAsync();
        }
    }
}
