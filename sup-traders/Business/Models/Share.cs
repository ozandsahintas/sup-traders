﻿namespace sup_traders.Business.Models
{
    public class Share
    {
        public string code { get; set; } = string.Empty;
        public int count { get; set; }
        public float price { get; set; }
    }
}