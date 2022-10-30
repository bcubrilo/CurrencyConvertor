using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CurrencyConvertor.Services;

namespace CurrencyConvertor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var host = CreateHost();
            base.OnStartup(e);

            Window mainWinodow = new MainWindow();
            mainWinodow.DataContext = host.Services.GetRequiredService<MainViewModel>();
            mainWinodow.ShowDialog();
        }
        static IHost CreateHost()
        {
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<ICurrencyApiService, CurrencyApiService>();
                    services.AddScoped<ICurrencyExchnageService, CurrencyExchangeService>();
                    services.AddScoped<MainViewModel>();
                    services.AddLogging(config => config.AddConsole());
                }).Build();
            return host;
        }

    }

    
}
