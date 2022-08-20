using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
