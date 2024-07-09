using System;
using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using Quartz;
using Quartz.Impl;

class Program
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); //初始化記錄器

    static async Task Main(string[] args) //async 允許使用 await 關鍵字
    {
        var configuration = new ConfigurationBuilder() 
            .SetBasePath(Directory.GetCurrentDirectory()) //設置配置文件的基路徑為當前目錄
            .AddJsonFile("jsconfig1.json")
            .Build();

        var settings = configuration.GetSection("Settings").Get<Setting[]>();

        foreach (var setting in settings)
        {
            if (setting.Type == 1)
            {
                await ScheduleIntervalTask(setting.IntervalSeconds);
            }
            else if (setting.Type == 2)
            {
                await ScheduleSpecificTimeTask(setting.ScheduledTime);
            }
        }

        Console.ReadLine();
    }

    private static async Task ScheduleIntervalTask(int intervalSeconds)
    {
        // 實現每間隔指定時間打印當下時間並記錄至Log
        // Quartz scheduler
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<IntervalJob>()
            .WithIdentity("intervalJob", "group1")
            .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("intervalTrigger", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(intervalSeconds)
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }

    private static async Task ScheduleSpecificTimeTask(DateTime scheduledTime)
    {
        // 實現特定時間打印當下時間並記錄至Log
        // Quartz scheduler
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<SpecificTimeJob>()
            .WithIdentity("specificTimeJob", "group2")
            .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("specificTimeTrigger", "group2")
            .StartAt(scheduledTime)
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }
}

public class Setting
{
    public int Type { get; set; }
    public int IntervalSeconds { get; set; }
    public DateTime ScheduledTime { get; set; }
}
