using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ModelDTOs
{
    public partial class RateHistoryDTO
    {
        public int ExchangeRateId { get; set; }
        public int SourceCurrencyId { get; set; }
        public int TargetCurrencyId { get; set; }
        [DisplayName("DateTime")]
        public DateTime Date { get; set; }
        //public virtual Currency SourceCurrency { get; set; } = null!;
        //public virtual Currency TargetCurrency { get; set; } = null!;
        //public decimal SourceTargetRate { get; set; }
        //public decimal TargetSourceRate { get; set; }
        public decimal SourceCurrencyPrice { get; set; }
        public decimal TargetCurrencyPrice { get; set; }
        [DisplayName("SourceCurrencyName")]
        public string CurrencySourceName { get; set; }
        [DisplayName("DestinationCurrencyName")]
        public string CurrencyTargetName { get; set; }
        public string CurrencySourceCode { get; set; }
        public string CurrencyTargetCode { get; set; }


    }
}
