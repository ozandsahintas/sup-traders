namespace sup_traders.Business.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; private set; }
        public float balance { get; private set; }

        public User()
        {
            name = DateTimeOffset.Now.ToUnixTimeSeconds().ToString() /* A name */;
            balance = 0f;
        }

        public bool UpdateBalance(float amount, OrgType t)
        {
            switch (t)
            {
                case OrgType.WITHDRAW:
                    if (balance > amount)
                    {
                        balance -= amount;
                        return true;
                    }
                    break;
                case OrgType.DEPOSIT:
                    if (amount >= 1)
                    {
                        balance += amount;
                        return true;
                    }
                    break;
                default:
                    return false;
            }

            return false;

        }
    }

    public class Balance
    {
        public int id { get; set; }
        public float amount { get; set; }
    }

    public enum OrgType
    {
        DEPOSIT,
        WITHDRAW,
    }
}
