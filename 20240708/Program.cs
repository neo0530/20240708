using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NLog;
using Quartz;
using Quartz.Impl;

class Program
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
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
    }

    private static async Task ScheduleSpecificTimeTask(DateTime scheduledTime)
    {
        // 實現特定時間打印當下時間並記錄至Log
    }
}

public class Setting
{
    public int Type { get; set; }
    public int IntervalSeconds { get; set; }
    public DateTime ScheduledTime { get; set; }
}
