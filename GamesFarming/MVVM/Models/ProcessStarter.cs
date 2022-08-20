using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace GamesFarming.MVVM.Models
{
    internal class ProcessStarter
    {
        private ProcessStartInfo _process;
        public string Path { get; private set; }

        public ProcessStarter(string filePath)
        {
            Path = filePath;
            _process = new ProcessStartInfo();
            _process.FileName = Path;
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
                _process.Arguments = arg.ToString();
                Process.Start(_process);
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to start new process : Path {Path} || Argument : {arg} || Message : {ex.Message}");
            }
        }

    }
}
