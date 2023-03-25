using System;
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
    }
}
