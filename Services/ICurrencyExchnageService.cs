using System.Threading.Tasks;

namespace CurrencyConvertor.Services
{
    public interface ICurrencyExchnageService
    {
        Task<decimal?> ConvertCurency(Currency fromCurrency, Currency toCurrency, decimal value);
    }
}
