using System.IO;
using System.Threading;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sockman.Views;

namespace Sockman;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ILogger<App> logger;

    public IHost AppHost { get; init; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(cd =>
            {
                cd.SetBasePath(Directory.GetCurrentDirectory());
                cd.AddJsonFile("appsettings.Development.json", optional: true);
                cd.AddEnvironmentVariables(prefix: "DI_DEMO_");
            })
            .ConfigureLogging((hc, cl) =>
            {
                cl.AddFile(hc.Configuration.GetSection("LoggingFile"));
                cl.AddConsole();
            })
            .ConfigureServices((hc, services) =>
            {
                services.AddSingleton<MainWindow>();
            })
            .Build();
        logger = AppHost.Services.GetRequiredService<ILogger<App>>();
        MainWindow = AppHost.Services.GetRequiredService<MainWindow>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        logger.LogInformation("startup begin in: {0}", Thread.CurrentThread.ManagedThreadId);
        await AppHost.StartAsync();
        MainWindow.Show();
        logger.LogInformation("startup end in: {0}", Thread.CurrentThread.ManagedThreadId);
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        logger.LogInformation("exit begin in: {0}", Thread.CurrentThread.ManagedThreadId);
        await AppHost.StopAsync();
        logger.LogInformation("exit end in: {0}", Thread.CurrentThread.ManagedThreadId);
        base.OnExit(e);
    }
}
