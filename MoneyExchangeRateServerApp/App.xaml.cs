using DataLayer.Models;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace MoneyExchangeRateServerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider serviceProvider { get; set; }
        public IConfiguration configuration { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.AddSingleton<IUnitOfWork, UnitOfWork>();
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var connectionString = configuration.GetConnectionString("Default");
            serviceCollection.AddDbContext<Prn221ProjectContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)), ServiceLifetime.Singleton);
            serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetRequiredService<MainWindow>().Show();
        }

        //private void Application_Startup(object sender, StartupEventArgs e)
        //{
        //    var serviceCollection = new ServiceCollection();
        //    serviceCollection.AddSingleton<LoginWindow>();
        //    serviceCollection.AddSingleton<Prn221ProjectContext>();
        //    serviceProvider = serviceCollection.BuildServiceProvider();
        //    serviceProvider.GetRequiredService<LoginWindow>().Show();
        //}
    }

}
