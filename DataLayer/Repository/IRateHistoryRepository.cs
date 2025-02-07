﻿using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IRateHistoryRepository : IGenericRepository<RateHistory>
    {
        public Task<List<RateHistory>> GetAllRateHistoryIncluded();
    }
}
