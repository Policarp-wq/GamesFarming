namespace GamesFarming.MVVM.Models
{
    public class ConfigReader
    {
        public const string CfgFolderPath = @"\steamapps\common\Counter-Strike Global Offensive\csgo\cfg\";
        public readonly string Path;
        public readonly string ConfigParams;
        public ConfigReader(string steamFolderPath, string cfgName)
        {
            Path = steamFolderPath + CfgFolderPath + cfgName;
            ConfigParams = FileSafeAccess.ReadFile(Path); ;
        }

        public bool ContainsParameter(string param)
        {
            return ConfigParams.Contains(param);
        }
    }
}
