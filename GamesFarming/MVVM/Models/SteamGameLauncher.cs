using System.Diagnostics;

namespace GamesFarming.MVVM.Models
{
    internal class SteamGameLauncher
    {
        public const string SteamArgs = "";
        private ProcessStartInfo _process;

        public readonly string SteamPath;

        public SteamGameLauncher(string steamPath)
        {
            SteamPath = steamPath; // Check
            _process = new ProcessStartInfo();
            _process.FileName = SteamPath;
        }
        public void StartGame(LaunchArgument arg)
        {
            _process.Arguments = arg.ToString();
            _process.UseShellExecute = true;
            Process.Start(_process);
        }
    }
}
