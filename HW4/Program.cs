//ThreadPool Home Work
//Сделать имитацию процеса загрузки процентами.
//Надо будет по нажатию какой то кнопки прервать процесс загрузки и запустить процесс загрузки обратно.
//P.S. использование Task => ThreadPool и CancellationToken обязательно.


using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    static bool isPaused = false;
    static bool isReversing = false; // Flag for reverse countdown
    static int currentProgress = 0;
    static object lockObj = new object();

    static void Main(string[] args)
    {
        Console.WriteLine("Press 'S' to start loading, 'P' to pause, 'R' to resume, 'C' to cancel, 'B' for reverse countdown.");

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.S)
            {
                cancellationTokenSource = new CancellationTokenSource();
                Task.Run(() => StartLoading(cancellationTokenSource.Token));
            }

            else if (key == ConsoleKey.P)
            {
                lock (lockObj)
                {
                    isPaused = true;
                }
                Console.WriteLine("Loading paused.");
            }

            else if (key == ConsoleKey.R)
            {
                lock (lockObj)
                {
                    isPaused = false;
                    Monitor.Pulse(lockObj);  // Prodoljenie posle pauzi
                }
                Console.WriteLine("Loading resumed.");
            }

            else if (key == ConsoleKey.C)
            {
                cancellationTokenSource.Cancel();
                Console.WriteLine("Loading cancelled.");
            }

            else if (key == ConsoleKey.B)
            {
                lock (lockObj)
                {
                    isReversing = true;
                    Monitor.Pulse(lockObj);  // Trigger reverse countdown
                }
                Console.WriteLine("Starting reverse countdown.");
            }
        }
    }

    static void StartLoading(CancellationToken token)
    {
        currentProgress = 0;
        isReversing = false;

        for (int i = 0; i <= 100; i++)
        {
            lock (lockObj)
            {
                while (isPaused)
                    Monitor.Wait(lockObj);  // Wait while loading is paused
            }

            if (token.IsCancellationRequested)
            {
                Console.WriteLine("Loading interrupted.");
                return;
            }

            if (isReversing)
            {
                ReverseLoading(token);
                return;
            }

            currentProgress = i;
            Console.WriteLine($"Loading: {i}%");
            Thread.Sleep(100);  // Simulate loading time

            if (i == 100)
                Console.WriteLine("Loading complete!");
        }
    }

    static void ReverseLoading(CancellationToken token)
    {
        for (int i = currentProgress; i >= 0; i--)
        {
            lock (lockObj)
            {
                while (isPaused)
                    Monitor.Wait(lockObj);  // Wait while reverse countdown is paused
            }

            if (token.IsCancellationRequested)
            {
                Console.WriteLine("Reverse countdown interrupted.");
                return;
            }

            Console.WriteLine($"Reverse countdown: {i}%");
            Thread.Sleep(100);  // Simulate reverse countdown time

            if (i == 0)
                Console.WriteLine("Reverse countdown complete!");
        }
    }
}
