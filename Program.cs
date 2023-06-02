using System.Configuration;
using System.ServiceProcess;

namespace CreateWindowsService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            string exeFilePath = ConfigurationManager.AppSettings["exeFilePath"];

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1(exeFilePath)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
