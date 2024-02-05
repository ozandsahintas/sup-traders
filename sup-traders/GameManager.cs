using sup_traders.Business.Models;

namespace sup_traders
{
    public sealed class GameManager
    {

        public readonly List<User> Users = [];
        public readonly Dictionary<string, Share> Shares = [];

        public GameManager() { }
        private static GameManager? _instance;

        public static GameManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
        public bool RegisterShare(Share s)
        {
            if (!Shares.ContainsKey(s.code))
            {
                Shares.Add(s.code, s);
                return true;
            }
            return false;
        }

        public bool RegisterUser(User u)
        {
            Users.Add(u);
            return true;
        }
    }
}