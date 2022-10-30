using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertor.Services
{
    public class CurrencyApiService:ICurrencyApiService
    {

        public async Task<Dictionary<string,Currency>?> GetCurrencies()
        {
            var client = new HttpClient();

            try
            {
                string responseJson = await client.GetStringAsync("http://www.floatrates.com/daily/usd.json");
                if (!string.IsNullOrEmpty(responseJson))
                {
                    var data = JsonConvert.DeserializeObject<Dictionary<string,Currency>>(responseJson);
                    if(data != null)
                    {
                        var dolar = new Currency()
                        {
                            Code = "USD",
                            AlphaCode = "USD",
                            Date = DateTime.Now,
                            InverseRate = 1,
                            Rate = 1,
                            Name = "US dolar",
                            NumericCode = 1
                        };

                        var currs = new Dictionary<string, Currency>();
                        currs.Add("usd", dolar);

                        foreach(var currency in data)
                        {
                            currs.Add(currency.Key, currency.Value);
                        }
                        
                        
                        return currs;
                    }
                }

            }catch (Exception ex)
            {

            }
            return null;
        }
    }

}
