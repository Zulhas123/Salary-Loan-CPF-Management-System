using LMS_Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web
{
    public class Program
    {
        
        public static async Task Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

             //StdSchedulerFactory factory = new StdSchedulerFactory();
             //IScheduler scheduler = await factory.GetScheduler();

             //// and start it off
             //await scheduler.Start();

             //IJobDetail job = JobBuilder.Create<TestJob>()
             //    .WithIdentity("job1", "group1")
             //    .Build();

             // Trigger the job to run now, and then repeat every 10 seconds
             //ITrigger trigger = TriggerBuilder.Create()
             //    .WithIdentity("trigger1", "group1")
             //    .StartNow()
             //    .WithSimpleSchedule(x => x
             //        .WithIntervalInSeconds(10)
             //        .RepeatForever())
             //    .Build();

             //await scheduler.ScheduleJob(job, trigger);

             // some sleep to show what's happening
             //   await Task.Delay(TimeSpan.FromSeconds(10));

             // and last shut down the scheduler when you are ready to close your program
             //  await scheduler.Shutdown();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
