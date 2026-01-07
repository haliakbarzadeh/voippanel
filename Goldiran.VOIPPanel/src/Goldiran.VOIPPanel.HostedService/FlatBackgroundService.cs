

using Goldiran.VOIPPanel.HostedService.BackGroundService.Enums;
using Goldiran.VOIPPanel.HostedService.BackGroundService.IServices;

namespace Goldiran.VOIPPanel.HostedService;

public class FlatBackgroundService : BackgroundService
//public class FraudBackgroundService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<FlatBackgroundService> _logger;
    private readonly IExecuteFlatDataService _executeFlatDataService;
    private readonly ITokenService _tokenService;
    private Timer? _timer = null;
    private int _counter = 1;
    private DateTime _nextRun;


    public FlatBackgroundService(ILogger<FlatBackgroundService> logger, IExecuteFlatDataService executeFlatDataService, ITokenService tokenService)
    {
        _logger = logger;
        _executeFlatDataService = executeFlatDataService;
        _tokenService = tokenService;
    }


    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(60));
        return Task.CompletedTask;

    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TimeSpan _interval = TimeSpan.FromSeconds(5);
        _nextRun = DateTime.Now.Add(_interval);
        //int counter = 1;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                //if (DateTime.Now.Hour <= 16 && DateTime.Now.Hour >= 8)
                //    continue;

                if (DateTime.Now >= _nextRun)
                {
                    ReportType reportType;

                    if (_counter % 2 == 0)
                        reportType = ReportType.AutoDial;
                    else
                        reportType = ReportType.Normal;
                    //reportType = ReportType.Normal;


                    var tokenResponse = await _tokenService.GetToken();

                    await _executeFlatDataService.ExecuteService(reportType, tokenResponse.AccessToken);
                    _nextRun = DateTime.Now.Add(_interval);

                    _counter++;

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            }

        }
    }
}