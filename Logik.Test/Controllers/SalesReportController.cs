using System.Diagnostics;
using Logik.Test.Models;
using Microsoft.AspNetCore.Mvc;

namespace Logik.Test.Controllers
{
    public class SalesReportController : Controller
    {
        private readonly ILogger<SalesReportController> _logger;
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SalesReport()
        {
            var test = new Test();
            var reportRows = new List<SalesReportRow>();
            string errorMessage = null;
            try
            {
                var stations = await test.GetStationSettingsAsync();
                var orders = await test.GetOrderHeadersAsync();
                var payments = new List<Test.OrderPaymentModel>();
                var transactions = new List<Test.OrderTransactionModel>();
                foreach (var order in orders.Where(o => o.OrderStatus == "2"))
                {
                    payments.AddRange(await test.GetOrderPaymentsAsync(order.OrderId.ToString()));
                    transactions.AddRange(await test.GetOrderTransactionsAsync(order.OrderId.ToString()));
                }
                var menuItems = await test.GetMenuItemsAsync();

                var totalAmountPaidPerStation = from S in stations
                                                join O in orders on S.StationId equals O.StationId
                                                join OP in payments on O.OrderId equals OP.OrderId
                                                group new { OP } by new { S.StationId } into z
                                                select new
                                                {
                                                    StationId = z.Key.StationId,
                                                    TotalAmountPaid = z.Sum(x => x.OP.AmountPaid)
                                                };

                var totalAmountPaidPerStationDictionary = totalAmountPaidPerStation.ToDictionary(x => x.StationId, x => x.TotalAmountPaid);

                var query = from S in stations
                            join O in orders on S.StationId equals O.StationId
                            join OP in payments on O.OrderId equals OP.OrderId
                            join OT in transactions on O.OrderId equals OT.OrderId
                            join MI in menuItems on OT.MenuItemId equals MI.MenuItemId
                            where S.LocationId == 690121713 && O.OrderStatus == "2"
                            group new { OP, MI } by new { S.StationId, OP.PaymentMethod, MI.VatPercentage, MI.MenuItemText } into g
                            select new SalesReportRow
                            {
                                StationId = g.Key.StationId,
                                TotalAmountPaidPerStation = totalAmountPaidPerStationDictionary.ContainsKey(g.Key.StationId) ? totalAmountPaidPerStationDictionary[g.Key.StationId] : 0,
                                PaymentType = g.Key.PaymentMethod,
                                AmountPaid = g.Sum(x => x.OP.AmountPaid),
                                VatPercentage = g.Key.VatPercentage,
                                MenuItemName = g.Key.MenuItemText,
                                TotalQuantity = g.Count(),
                                TotalAmount = g.Sum(x => x.MI.DefaultUnitPrice)
                            };
                reportRows = query.ToList();
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                _logger.LogError(ex, "Error fetching sales report data from APIs");
            }
            ViewBag.ApiError = errorMessage;
            return View(reportRows);
        }
    }
}
