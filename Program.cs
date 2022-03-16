using System;
using Topshelf;

namespace DownloadsDeleteService
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<DownloadDeleteService>(s =>
                {
                    s.ConstructUsing(EhsService => new DownloadDeleteService());
                    s.WhenStarted(EhsService => EhsService.Start());
                    s.WhenStopped(EhsService => EhsService.Stop());
                });

                x.RunAsLocalSystem();

                x.SetServiceName("MyDownloadFolderDeleteService");//name for the service
                x.SetDisplayName("My Download older Delete Service");//name to be displayed
                //desciption of the windows service!
                x.SetDescription("My personal windows service created to delete downloads folder files after x days");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
