using DataLayer;
using DataLayer.Repository;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Prn221ProjectContext _db;
        private ICurrencyRepository currencyRepository;
        private IRateHistoryRepository rateHistoryRepository;
        public UnitOfWork(Prn221ProjectContext db)
        {
            _db = db;
            currencyRepository = new CurrencyRepository(_db);
            rateHistoryRepository = new RateHistoryRepository(_db);
        }
        public ICurrencyRepository CurrencyRepository
        {
            get { return currencyRepository ?? new CurrencyRepository(_db); }
        }
        public IRateHistoryRepository RateHistoryRepository
        {
            get { return rateHistoryRepository ?? new RateHistoryRepository(_db); }
        }
        public void Commit()
        {
            _db.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Rollback()
        {
            _db.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _db.DisposeAsync();
        }
    }
}
