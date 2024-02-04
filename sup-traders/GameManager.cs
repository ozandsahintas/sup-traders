namespace sup_traders
{
    public sealed class GameManager
    {
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
        public string Test()
        {
            return "Hey";
        }
    }
}