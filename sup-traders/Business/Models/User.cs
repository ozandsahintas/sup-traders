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

        public decimal CalculateNewBalance(decimal balance, decimal amount, OrgType t, decimal price)
        {
            switch (t)
            {
                case OrgType.DEPOSIT:
                case OrgType.SELL:
                    if (amount != 0)
                    {
                        balance += amount * price;
                        return balance;
                    }
                    break;
                case OrgType.WITHDRAW:
                case OrgType.BUY:
                    if (balance >= amount * price)
                    {
                        balance -= amount * price;
                        return balance;
                    }
                    break;
                default:
                    return -1;
            }

            return -1;
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
