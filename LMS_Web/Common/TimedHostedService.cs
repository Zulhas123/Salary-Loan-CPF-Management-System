using LMS_Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LMS_Web.Common
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        //  private ApplicationDbContext db;
        private IConfiguration configuration;

        public TimedHostedService(ILogger<TimedHostedService> logger, IConfiguration _configuration)
        {
            _logger = logger;
            // db = _db;
            configuration = _configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(6));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            string connString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySQL(connString);
            ApplicationDbContext db = new ApplicationDbContext(optionsBuilder.Options);
            //ApplicationDbContext db=new ApplicationDbContext();
            Utility utility = new Utility(db, configuration);
            utility.CalculateLeave();
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
