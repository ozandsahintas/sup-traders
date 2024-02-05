namespace sup_traders.Business.Models
{
    public class User
    {
        public string name;
        public float balance { get; private set; }

        public User()
        {
            name = "";
            balance = 0f;
        }

    }
}
