using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class RateHistoryRepository : GenericRepository<RateHistory>, IRateHistoryRepository
    {
        public RateHistoryRepository(Prn221ProjectContext context) : base(context)
        {
        }

        public async Task<List<RateHistory>> GetAllRateHistoryIncluded()
        {
            return await _context.RateHistories
                .Include(c => c.SourceCurrency)
                .Include(m => m.TargetCurrency).ToListAsync();
        }
    }
}
