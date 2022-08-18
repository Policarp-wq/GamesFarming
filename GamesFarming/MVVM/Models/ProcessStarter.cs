using System.Diagnostics;

namespace GamesFarming.MVVM.Models
{
    internal class ProcessStarter
    {
        internal static class ProgramStarter
        {
            public static void Start(string path, string args = "")
            {
                ProcessStartInfo process = new ProcessStartInfo();
                process.FileName = path;
                process.Arguments = args;
                Process.Start(process);
            }

        }
    }
}
