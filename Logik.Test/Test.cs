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
        public static string orderPayments = api + "OrderPayments/";  //orderId="; trebuie sa fac publish pentru asta

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
    }
}
