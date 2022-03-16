using System;
using System.Configuration;
using System.IO;
using System.Timers;

namespace DownloadsDeleteService
{
    class DownloadDeleteService
    {
        private readonly Timer _timer;
        public DownloadDeleteService()
        {
            //Mentioning timer interval i have set for every 12 hrs from service staring
            _timer = new Timer(12*60*60 * 1000) { AutoReset = true };
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var path = ConfigurationManager.AppSettings["downloadDirectoryFilePath"];
                string[] Files = Directory.GetFiles(@""+path);
                foreach(var file in Files)
                {
                    FileInfo fi = new FileInfo(file);
                    if(fi.CreationTime<DateTime.Now.Subtract(TimeSpan.FromDays(7)))
                    {
                        WriteLogFile("Deleted:" + fi.Name);
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogFile("Exception caught: Message=" + ex.Message);
                Stop();
                throw;
            }
        }

        public void Start()
        {
            WriteLogFile("Service is Started!");
            _timer.Start();

        }
        public void Stop()
        {
            WriteLogFile("Service is Stopped!");
            _timer.Stop();
        }
        public void WriteLogFile(string message)
        {
            var logpath = ConfigurationManager.AppSettings["logFilePath"];
            StreamWriter sw = null;
            string date = DateTime.Now.Date.ToString("ddMMyyyy");
            sw = new StreamWriter(@"" + logpath + @"\Log-" + date + ".txt", true);
            sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
            sw.Flush();
            sw.Close();
        }
    }
}
