using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    public partial class RateHistory
    {
        public int ExchangeRateId { get; set; }
        public int SourceCurrencyId { get; set; }
        public int TargetCurrencyId { get; set; }
        public decimal SourceCurrencyPrice { get; set; }
        public decimal TargetCurrencyPrice { get; set; }
        public DateTime Date { get; set; }
        public virtual Currency SourceCurrency { get; set; } = null!;
        public virtual Currency TargetCurrency { get; set; } = null!;

        // test cái này!
        //public decimal SourceTargetRate
        //{
        //    get
        //    {
        //        return SourceCurrency != null && TargetCurrency != null ? SourceCurrency.Price / TargetCurrency.Price : 0;
        //    }
        //}
        //public decimal TargetSourceRate
        //{
        //    get
        //    {
        //        return SourceCurrency != null && TargetCurrency != null ? TargetCurrency.Price / SourceCurrency.Price : 0;
        //    }
        //}
    }
}
