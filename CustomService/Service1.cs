using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace CustomService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);  
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);  
            timer.Interval = 5000; //number in milisecinds  
            timer.Enabled = true;  
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e) {  
            WriteToFile("1");  
        }  

        public void WriteToFile(string Message) {  
            string path = "C:\\Logs";  
            if (!Directory.Exists(path)) {  
                Directory.CreateDirectory(path);  
            }  
            string filepath = path + "\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";  
            if (!File.Exists(filepath)) {  
                // Create a file to write to.   
                using(StreamWriter sw = File.CreateText(filepath)) {  
                    sw.WriteLine(Message);  
                }  
            } else {  
                using(StreamWriter sw = File.AppendText(filepath)) {  
                    sw.WriteLine(Message);  
                }  
            }  
        }  
    }
}
