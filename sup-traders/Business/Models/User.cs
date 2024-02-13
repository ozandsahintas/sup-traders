namespace sup_traders.Business.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; private set; }
        public decimal balance { get; private set; }

        public User()
        {
            name = DateTimeOffset.Now.ToUnixTimeSeconds().ToString() /* A name */;
            balance = 0;
        }
    }



    public class Balance
    {
        public int id { get; set; }
        public decimal amount { get; set; }
    }

    public enum OrgType
    {
        DEPOSIT,
        WITHDRAW,
        BUY,
        SELL,
    }
}
