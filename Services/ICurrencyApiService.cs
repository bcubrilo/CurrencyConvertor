using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConvertor.Services
{

    public interface ICurrencyApiService
    {
        Task<Dictionary<string, Currency>?> GetCurrencies();
    }
}
