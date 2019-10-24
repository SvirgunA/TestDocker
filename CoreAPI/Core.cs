using Configs;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace CoreAPI
{
    public class Core1
    {
        private readonly string _args;
        public Core1(string arguments)
        {
            _args = arguments;
        }

        public void ProcessLoop()
        {
            try
            {
                var env = new ConfigData().EnvName;
                Console.WriteLine($"env - {env}");
                WriteToFile($"env - {env}");
                WriteToFile($"Command line arguments: {_args}");
                var i = 0;
                while (true)
                {
                    i++;
                    WriteToFile(JsonConvert.SerializeObject(new Test1("Func recall", DateTime.Now), Formatting.Indented));
                    Thread.Sleep(2000);
                    if (i > 5)
                        throw new Exception("MoreThanTen");
                }
            }
            catch(Exception e)
            {
                WriteToFile($"EXCEPTION {e.Message}");
            }
        }

        public void WriteToFile(string Message)
        {
            Console.WriteLine(Message);
            string path = "C:\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = path + "\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
