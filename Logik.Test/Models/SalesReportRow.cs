namespace Logik.Test.Models
{
    public sealed class SalesReportRow
    {
        public int StationId { get; set; }
        public decimal TotalAmountPaidPerStation { get; set; }
        public string PaymentType { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public decimal VatPercentage { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
