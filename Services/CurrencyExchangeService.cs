using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CurrencyConvertor.Services
{
    public class CurrencyExchangeService : ICurrencyExchnageService
    {
        private readonly ICurrencyApiService _currencyApiService;
        private readonly ILogger<CurrencyExchangeService> _logger;
        public CurrencyExchangeService(ICurrencyApiService currencyApiService, ILogger<CurrencyExchangeService> logger)
        {
            _currencyApiService = currencyApiService;
            _logger = logger;
        }
        public async Task<decimal?> ConvertCurency(Currency fromCurrency, Currency toCurrency, decimal value)
        {
            try
            {
                var fromCode = (fromCurrency.Code ?? "").ToLower();
                var toCode = (toCurrency.Code ?? "").ToLower();
                if(string.IsNullOrEmpty(fromCode) || string.IsNullOrEmpty(toCode))
                {
                    _logger.LogError("Currency code is empty ({from} => {to})", fromCode, toCode);
                    return null;
                }
                var currencies = await _currencyApiService.GetCurrencies();
                if(currencies == null)
                {
                    _logger.LogError("Couldn't get exchange rate");
                    return null;
                }

                currencies.TryGetValue(fromCode, out var fromCurr);
                currencies.TryGetValue(toCode, out var toCurr);

                if (fromCurr == null)
                {
                    _logger.LogError("Coulnd't fine currency with code {fromCode}", fromCode);
                    return null;
                }

                if(toCurr == null)
                {
                    _logger.LogError("Coulnd't fine currency with code {toCode}", toCode);
                    return null;
                }

                if (fromCode == "usd")
                {
                    return value * toCurr.Rate;
                }


                if(toCode == "usd")
                {
                    return value * fromCurr.InverseRate;
                }

                var dolarValue = value * fromCurr.InverseRate;

                return dolarValue * toCurr.Rate;
                
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error converting data");
            }

            return null;
            
        }
    }
}
