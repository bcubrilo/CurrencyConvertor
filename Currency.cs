using Newtonsoft.Json;
using System;

namespace CurrencyConvertor
{
    public class Currency
    {
        [JsonProperty("code")]
        public string? Code { get; set; }
        [JsonProperty("alphaCode")]
        public string? AlphaCode { get; set; }
        [JsonProperty("numericCode")]
        public int? NumericCode { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("rate")]
        public decimal? Rate { get; set; }
        [JsonProperty("date")]
        public DateTime? Date { get; set; }
        [JsonProperty("inverseRate")]
        public decimal InverseRate { get; set; }
    }
}
