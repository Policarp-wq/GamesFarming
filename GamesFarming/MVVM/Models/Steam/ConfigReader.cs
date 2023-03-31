using System;

namespace GamesFarming.MVVM.Models
{
    public class ConfigReader
    {
        //public const string CfgFolderPath = @"\steamapps\common\Counter-Strike Global Offensive\csgo\cfg\";
        public const string ResolutionParameter = "mat_setvideomode";
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
            foreach(var line in ConfigParams.Split('\n'))
            {
                if(line.Contains(param))
                    return line.Trim();
            }
            return string.Empty;
        }
        public Resolution GetResolution()
        {
            var line = GetLineWithParameter(ResolutionParameter);
            var args = line.Trim().Split(' ');
            if (args.Length < 4)
                return null;
            try
            {
                return new Resolution(int.Parse(args[1]), int.Parse(args[2]));
            }
            catch(FormatException)
            {
                return new Resolution(-666, -666);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
