using System;
using System.Collections.Generic;

namespace OdbirReportingFix.Models
{
    public partial class Revenues
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid TaxStationRevenueTargetId { get; set; }

        public TaxStationRevenueTargets TaxStationRevenueTarget { get; set; }
    }
}
