using System.Text.Json.Serialization;

namespace sup_traders.Business.Models
{
    public class Share
    {
        public string code { get; set; } = string.Empty;
        public int count { get; set; }
        [JsonIgnore]
        public int baseCount { get; set; }
        public decimal price { get; set; }
    }
}
