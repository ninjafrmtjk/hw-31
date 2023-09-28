using System.Timers;

System.Timers.Timer timer1 = new(2000);
TimerCallback timerCallback = new TimerCallback(DoSomething);

timer1.AutoReset = true;
timer1.Enabled = true;
timer1.Start();

Thread thread = new Thread(Func);
thread.Priority = ThreadPriority.Highest;
thread.Start();

Thread thread1 = new Thread(ExceptionMessage);
thread1.Start();

thread1.Join();

Console.ReadKey();

//thread.Abort(); //прерывание потока


CancellationTokenSource cts = new CancellationTokenSource();
CancellationToken token = cts.Token;

Thread thread2 = new Thread(() => Func1(cts.Token));
thread2.Start();

Console.ReadKey();

cts.Cancel();
thread2.Join();


//отмена выполнения задачи
cts.Cancel();


static void Func()
{
    Console.WriteLine("Hello this is: Thread with id - {0} and {1} priority ", Environment.CurrentManagedThreadId, Thread.CurrentThread.Priority);
}

static void DoSomething(object state)
{
    Console.WriteLine("Таймер сработал в {0}", DateTime.Now);
}

static void Func1(CancellationToken token)
{
    while (!token.IsCancellationRequested)
    {
        Console.WriteLine("Поток работает...");
        Thread.Sleep(1000);
    }

    Console.WriteLine("Поток был отменён!");

}

static void ExceptionMessage()
{
    try
    {
        if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            throw new Exception("Произошла ошибка!");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

var cancellationTokenSource = new CancellationTokenSource();
cancellationTokenSource.CancelAfter(3000);
await MyTask(cancellationTokenSource.Token);

async Task MyTask(CancellationToken cancellationToken)
{
    await Task.Delay(5000, cancellationToken);
    // код, который выполнится после задержки
}