using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamesFarming.MVVM.Models
{
    internal static class ThreadHandler
    {
        public static void StartInThread(Action action)
        {
            Thread thread = new Thread(new ThreadStart(action));
            thread.Start();
        }

        public static void StartThreads(IEnumerable<Thread> threads, CancellationToken cancellationToken, Action onStartEnd = null)
        {
            new Thread( () =>
            {
                new Task( () =>
                {
                    int cnt = 0;
                    foreach (var thread in threads)
                    {
                        if (cnt % 5 == 0 && cnt != 0)
                            Thread.Sleep(60000);
                        if (cancellationToken.IsCancellationRequested)
                            break;
                        thread.Start();
                        thread.Join();
                        cnt++;
                    }
                }, cancellationToken).Start();

                onStartEnd?.Invoke();
            })
            { ApartmentState = ApartmentState.STA}.Start();

        }
    }
}
