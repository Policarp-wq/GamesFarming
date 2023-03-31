namespace GamesFarming.MVVM.Models
{
    public class ConfigReader
    {
        //public const string CfgFolderPath = @"\steamapps\common\Counter-Strike Global Offensive\csgo\cfg\";
        public readonly string Path;
        public readonly string ConfigParams;
        public ConfigReader(string cfgFolder, string cfgName)
        {
            if(cfgName== null)
            {
                ConfigParams = string.Empty;
                return;
            }
            if (!cfgName.Contains(".cfg") && !cfgName.Contains(".txt"))
                cfgName += ".cfg";
            Path = cfgFolder + "\\" + cfgName;
            ConfigParams = FileSafeAccess.ReadFile(Path); ;
        }

        public bool ContainsParameter(string param)
        {
            return ConfigParams.Contains(param);
        }
        public string GetLineWithParameter(string param) 
        {
            foreach(var line in ConfigParams)
            {
                
            }
            return "";
        }
    }
}
