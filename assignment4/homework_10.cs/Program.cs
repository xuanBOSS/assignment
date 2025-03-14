// See https://aka.ms/new-console-template for more information
/*使⽤事件机制，模拟实现⼀个闹钟功能。闹钟可以有嘀嗒（Tick）事件和响铃
（Alarm）两个事件。在闹钟⾛时时或者响铃时，在控制台显示提示信息。*/
using System;
using System.Threading;//基础的多线程功能
using System.Threading.Tasks;//任务并行库，用于创建和管理异步任务
/*Tick事件和Alarm事件*/
/*先声明委托类型，再定义事件相当于创建一个委托实例*/
/*构造一个启动闹钟的方法，每秒Tick，到达指定时间后Alarm*/
public class Clock
{
    public delegate void TickHandler();
    public delegate void AlarmHandler(DateTime dateTime);

    public event TickHandler Tick;
    public event AlarmHandler Alarm;

    //设定的闹钟参数
    private DateTime dateTime;  

    //构造函数
    public Clock(DateTime dateTime)
    {
        this.dateTime = dateTime;
    }

    /*启动方法:需要在后台创建一个新的闹钟线程来模拟时间流逝*/
    /*需要注意：event实际上是delegate的一种。
     如果没有任何方法订阅事件，事件的值就是 null，如果直接调用 event()，就会抛出 NullReferenceException*/
    public void StartAlarm()
    {
        Task.Run(() =>
        {
            while (true)
            {
                Thread.Sleep(1000); // 让当前线程暂停1秒
                if (Tick != null) { Tick(); } //触发 Tick 事件（表示“滴答”）
                if (DateTime.Now >= dateTime)
                {
                    if (Alarm != null) { Alarm(dateTime); }// 触发 Alarm 事件（表示闹钟响了）
                    break;
                }
            }
        });
    }
}
class Program
{
    //Tick处理方法
    static void OnTick()
    {
        Console.WriteLine("滴答...");
    }
    //Alarm处理方法
    static void OnAlarm(DateTime time)
    {
        //time:G为常规长日期
        Console.WriteLine($"响铃！时间：{time:G}");
    }
    static void Main()
    {
        //在当前时间之后十秒响铃
        DateTime dateTime = DateTime.Now.AddSeconds(10);
        Clock clock = new Clock(dateTime);

        //订阅事件
        clock.Tick += OnTick;
        clock.Alarm += OnAlarm;

        //启动闹钟
        clock.StartAlarm();
        //防止主线程立即退出，让异步任务可以运行
        Console.ReadLine();
    }
}

