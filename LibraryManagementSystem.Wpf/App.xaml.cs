using System.Windows;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagementSystem;

public partial class App : Application
{
    public static IHost AppHost { get; private set; } = null!;

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<LibraryDbContext>(options =>
                    options.UseSqlite(context.Configuration.GetConnectionString("DefaultConnection")));

                // Core Services
                services.AddSingleton<PasswordHasher>();
                services.AddSingleton<UserSession>();
                services.AddSingleton<Services.SettingsService>();
                services.AddScoped<AuthenticationService>();
                services.AddScoped<DbSeeder>();

                // Business Services
                services.AddScoped<BookService>();
                services.AddScoped<PatronService>();
                services.AddScoped<TransactionService>();
                services.AddScoped<FineService>();

                // ViewModels
                services.AddTransient<LoginViewModel>();
                services.AddTransient<MainViewModel>();
                services.AddTransient<BooksViewModel>();
                services.AddTransient<PatronsViewModel>();
                services.AddTransient<TransactionsViewModel>();
                services.AddTransient<UsersViewModel>();
                services.AddTransient<ReportsViewModel>();
                services.AddTransient<ViewModels.SettingsViewModel>();

                // Views
                services.AddTransient<LoginWindow>();
                services.AddTransient<MainWindow>();
                services.AddTransient<BooksView>();
                services.AddTransient<PatronsView>();
                services.AddTransient<TransactionsView>();
                services.AddTransient<UsersView>();
                services.AddTransient<ReportsView>();
                services.AddTransient<Views.SettingsView>();
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        AppHost.Start();

        using var scope = AppHost.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
        db.Database.EnsureCreated();
        scope.ServiceProvider.GetRequiredService<DbSeeder>().SeedDefaults();

        var loginWindow = AppHost.Services.GetRequiredService<LoginWindow>();
        MainWindow = loginWindow;
        loginWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (AppHost)
        {
            await AppHost.StopAsync();
        }

        base.OnExit(e);
    }
}
