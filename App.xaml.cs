using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.Views;

namespace LibraryManagementSystem
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Database - Register as Singleton for desktop app
                    services.AddDbContext<LibraryDbContext>(options =>
                        options.UseSqlite("Data Source=library.db"),
                        ServiceLifetime.Singleton);

                    // Services
                    services.AddSingleton<IAuthenticationService, AuthenticationService>();
                    services.AddSingleton<IBookService, BookService>();
                    services.AddSingleton<IPatronService, PatronService>();
                    services.AddSingleton<ITransactionService, TransactionService>();
                    services.AddSingleton<IFineService, FineService>();

                    // ViewModels
                    services.AddTransient<LoginViewModel>();
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<BookManagementViewModel>();
                    // Use a singleton for PatronManagementViewModel so the same instance (and its ObservableCollection) is shared
                    services.AddSingleton<PatronManagementViewModel>();
                    services.AddTransient<DashboardViewModel>();
                    services.AddTransient<TransactionViewModel>();
                    services.AddTransient<ReportsViewModel>();

                    // Views
                    services.AddTransient<LoginWindow>();
                    services.AddTransient<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            // Initialize database
            var dbContext = _host.Services.GetRequiredService<LibraryDbContext>();
            await dbContext.Database.EnsureCreatedAsync();

            var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
            loginWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }

        public static T GetService<T>() where T : class
        {
            return ((App)Current)._host.Services.GetRequiredService<T>();
        }
    }
}
