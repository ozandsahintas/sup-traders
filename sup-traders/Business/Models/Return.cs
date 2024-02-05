namespace sup_traders.Business.Models
{
    public class Return<T>
    {
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
