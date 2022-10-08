using System;
using System.Collections.Generic;
using System.Threading;

namespace GamesFarming.MVVM.Models
{
    internal static class ThreadHandler
    {
        public static void StartInThread(Action action)
        {
            Thread thread = new Thread(new ThreadStart(action));
            thread.Start();
        }

        public static void StartThreads(IEnumerable<Thread> threads)
        {
            new Thread( () =>
            {
                foreach (var thread in threads)
                {
                    thread.Start();
                    thread.Join();
                }

            })
            { ApartmentState = ApartmentState.STA}.Start();
        }
    }
}
