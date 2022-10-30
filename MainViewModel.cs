using CurrencyConvertor.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CurrencyConvertor
{
    public class MainViewModel:ViewModelBase
    {
        private readonly ICurrencyApiService _currencyApiService;
        private readonly ICurrencyExchnageService _currencyExchnageService;
        private readonly ILogger<MainViewModel> _logger;

        private List<Currency> _currencies = new List<Currency>();
        public List<Currency> Currencies
        {
            get { return _currencies; }
            set { _currencies = value; OnPropertyChanged(nameof(Currencies)); }
        }

        private bool _loading;

        public bool Loading
        {
            get { return _loading; }
            set { _loading = value; OnPropertyChanged(nameof(Loading)); }
        }

        private Currency _fromCurrency;
        public Currency FromCurrency
        {
            get { return _fromCurrency; }
            set { _fromCurrency = value; OnPropertyChanged(nameof(FromCurrency)); OnPropertyChanged(nameof(ToCurrencyList)); }
        }

        private Currency _toCurrency;
        public Currency ToCurrency
        {
            get { return _toCurrency; }
            set { _toCurrency = value; OnPropertyChanged(nameof(ToCurrency)); }
        }

        private decimal? _value;
        public decimal? Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(nameof(Value)); }
        }

        private decimal? _convertedValue;
        public decimal? ConvertedValue
        {
            get { return _convertedValue; }
            set { _convertedValue = value;  OnPropertyChanged(nameof(ConvertedValue)); }
        }


        public List<Currency> ToCurrencyList => FromCurrency != null ? Currencies.Where(x => !string.Equals(x.Code, FromCurrency.Code)).ToList() : new List<Currency>();

        /// <summary>
        /// MainViewModel constructor
        /// </summary>
        /// <param name="currencyApiService"></param>
        /// <param name="currencyExchnageService"></param>
        /// <param name="logger"></param>
        public MainViewModel(ICurrencyApiService currencyApiService, ICurrencyExchnageService currencyExchnageService, ILogger<MainViewModel> logger)
        {
            _logger = logger;
            _currencyApiService = currencyApiService;
            _currencyExchnageService = currencyExchnageService;
        }

        #region Commands
        private ICommand _onLoadedCommand;
        public ICommand LoadedCommand => _onLoadedCommand ??= new CommandAsync<object>((o) => OnLoaded());

        private async Task OnLoaded()
        {
            Loading = true;
            try
            {
                var currencies = await _currencyApiService.GetCurrencies();
                if (currencies != null && currencies.Count > 0)
                {
                    Currencies = currencies.Select(x => x.Value).ToList();
                }
            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading data");
            }
            finally
            {
                Loading = false;
            }
        }

        private ICommand _convertValueCommand;
        public ICommand ConvertValueCommand => _convertValueCommand ??= new CommandAsync<object>((o) => ConvertValue(), (o) => !Loading && FromCurrency != null && ToCurrency != null && Value.HasValue);

        private async Task ConvertValue()
        {
            Loading = true;
            try
            {
                if (FromCurrency != null && ToCurrency != null && Value.HasValue)
                {
                    var t = await _currencyExchnageService.ConvertCurency(FromCurrency, ToCurrency, Value.Value);
                    if (t != null)
                    {
                        ConvertedValue = t;
                    }
                }
            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting value");
            }
            finally
            {
                Loading =false; 
            }
            
        }
        #endregion
    }
}
