using System;
using System.Collections.Generic;

namespace OdbirReportingFix.Models
{
    public partial class TransactionLogs
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PaymentReferenceNumber { get; set; }
        public DateTime? PaymentDateTime { get; set; }
        public string ReceiptNumber { get; set; }
        public string BillNumber { get; set; }
        public string CustomerName { get; set; }
        public string Location { get; set; }
        public string Payment { get; set; }
        public string Orin { get; set; }
        public decimal? Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string DepositSlip { get; set; }
        public string ChequeValueDate { get; set; }
        public string Bank { get; set; }
        public string AdditionalInfo { get; set; }
        public string Mda { get; set; }
        public string BankBranch { get; set; }
        public string BankName { get; set; }
        public string RefCode1 { get; set; }
        public string RefCode2 { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionNumber { get; set; }
        public bool IsManuallyAdded { get; set; }
        public string GroupName { get; set; }
        public string Category { get; set; }
    }
}
