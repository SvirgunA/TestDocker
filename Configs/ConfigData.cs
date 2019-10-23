using System.Configuration;

namespace Configs
{
    public class ConfigData
    {
        public string EnvName => ConfigurationManager.AppSettings["Environment"];
    }
}
