using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataLayer.Models
{
    public partial class Currency
    {
        public Currency()
        {
            RateHistorySourceCurrencies = new HashSet<RateHistory>();
            RateHistoryTargetCurrencies = new HashSet<RateHistory>();
        }

        public int CurrencyId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        [JsonIgnore]
        public virtual ICollection<RateHistory> RateHistorySourceCurrencies { get; set; }
        [JsonIgnore]
        public virtual ICollection<RateHistory> RateHistoryTargetCurrencies { get; set; }
    }
}
