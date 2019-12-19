using System;
using System.Collections.Generic;

namespace OdbirReportingFix.Models
{
    public partial class TaxStationRevenueTargets
    {
        public TaxStationRevenueTargets()
        {
            Revenues = new HashSet<Revenues>();
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string TaxStationName { get; set; }
        public decimal MonthlyTarget { get; set; }
        public decimal AnnualTarget { get; set; }
        public DateTime Date { get; set; }
        public string Year { get; set; }

        public ICollection<Revenues> Revenues { get; set; }
    }
}
