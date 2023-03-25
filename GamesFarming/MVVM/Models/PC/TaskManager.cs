using System;
using System.Diagnostics;

namespace GamesFarming.MVVM.Models.PC
{
    public static class TaskManager
    {
        public const int ClosingTimeMlSecs = 15000;
        public static void CloseProcces(string name)
        {
            foreach (var procces in Process.GetProcessesByName(name))
            {
                try 
                {
                    procces.Kill();
                }
                catch(Exception ex)
                {
                    throw new Exception($"{ex.Message} while closing proccesses by name {name}");
                }
            }
        }
    }
}
