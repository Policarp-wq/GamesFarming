using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace GamesFarming.MVVM.Models
{
    internal class ProcessStarter
    {
        private ProcessStartInfo process;
        public string Path { get; private set; }

        public ProcessStarter(string filePath)
        {
            Path = filePath;
            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = Path;
        }

        public void MultipleStart(IEnumerable<LaunchArgument> args)
        {
            foreach(var argument in args)
            {
                Start(argument);
            }
        }

        public void Start(LaunchArgument arg)
        {
            try
            {
                process.Arguments = arg.ToString();
                Process.Start(process);
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to start new process : Path {Path} || Argument : {arg} || Message : {ex.Message}");
            }
        }

    }
}
