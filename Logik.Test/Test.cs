using System.Text.Json.Serialization;

namespace Logik.Test
{
    public class Test
    {
        public static string api = "https://admin.elogik.ro/api/";
        
        //StationSettings
        public static string stationSettings = api + "StationSettings";

        //OrderHeaders(Comenzi) - orderstatus 1 - Deschis, 2 - Platit, 3 - Anulat
        public static string orderHeaders = api + "Rechemare/date=2025-08-23&showAll=true";

        //OrderPayments(Plati) - by orderId - PaymentMethod 1 - Cash, 9 - Card
        public static string orderPayments = api + "OrderPayments/orderId=";

        //OrderTransactions(Tranzactii) - by orderId - TransactionStatus - 2 - Anulat, 1 - Valid
        public static string orderTransactions = api + "OrderTransactions/getOrderID=";

        //MenuItems
        public static string menuitems = api + "MenuItems";

        /*
            Raport Vanzari pe Statie
            
            Total pe statie, Total CASH pe statie, Total CARD pe statie

            Produse cumulate pe statie (nume, cantitate, total)
        */

        HttpClient client;
        public Test()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("ClientId", "amoom");
            client.DefaultRequestHeaders.Add("LocationId", "690121713");
        }

        public async Task<List<StationSettingModel>> GetStationSettingsAsync()
        {
            var response = await client.GetAsync(stationSettings);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<StationSettingModel>>(json);
        }

        public async Task<List<OrderHeaderModel>> GetOrderHeadersAsync()
        {
            var response = await client.GetAsync(orderHeaders);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<OrderHeaderModel>>(json);
        }

        public async Task<OrderPaymentModel> GetOrderPaymentsAsync(string orderId)
        {
            var url = orderPayments + orderId;
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<OrderPaymentModel>(json);
        }

        public async Task<List<OrderTransactionModel>> GetOrderTransactionsAsync(string orderId)
        {
            var url = orderTransactions + orderId;
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<OrderTransactionModel>>(json);
        }

        public async Task<List<MenuItemModel>> GetMenuItemsAsync()
        {
            var response = await client.GetAsync(menuitems);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<MenuItemModel>>(json);
        }

        public class StationSettingModel
        {
            [JsonPropertyName("stationID")]
            public int StationId { get; set; }

            [JsonPropertyName("locationId")]
            public int LocationId { get; set; }
        }

        public class OrderHeaderModel
        {
            [JsonPropertyName("orderID")]
            public int OrderId { get; set; }

            [JsonPropertyName("stationID")]
            public int StationId { get; set; }

            [JsonPropertyName("orderStatus")]
            public string OrderStatus { get; set; } = String.Empty;
        }

        public class OrderPaymentModel
        {
            [JsonPropertyName("orderID")]
            public int OrderId { get; set; }

            [JsonPropertyName("paymentMethod")]
            public string PaymentMethod { get; set; } = String.Empty;

            [JsonPropertyName("amountPaid")]
            public decimal AmountPaid { get; set; }
        }

        public class OrderTransactionModel
        {
            [JsonPropertyName("orderID")]
            public int OrderId { get; set; }

            [JsonPropertyName("menuItemID")]
            public int MenuItemId { get; set; }
        }

        public class MenuItemModel
        {
            [JsonPropertyName("menuItemID")]
            public int MenuItemId { get; set; }

            [JsonPropertyName("vatPercentage")]
            public decimal VatPercentage { get; set; }

            [JsonPropertyName("menuItemText")]
            public string MenuItemText { get; set; } = String.Empty;

            [JsonPropertyName("defaultUnitPrice")]
            public decimal DefaultUnitPrice { get; set; }
        }
    }
}
