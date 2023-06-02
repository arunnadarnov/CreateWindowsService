using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Timers;

namespace CreateWindowsService
{
    public partial class Service1 : ServiceBase
    {
        private Process myProcess;
        private Timer checkProcessTimer;
        private readonly string _exeFilePath;

        public Service1(string exeFilePath)
        {
            InitializeComponent();
            _exeFilePath = exeFilePath;
        }

        protected override void OnStart(string[] args)
        {
            // Get the path of the service executable
            string serviceExePath = Assembly.GetExecutingAssembly().Location;

            // Get the directory that contains the service executable
            string serviceExeDirectory = Path.GetDirectoryName(serviceExePath);

            // Combine the directory path with the name of your .exe file
            string programExePath = Path.Combine(serviceExeDirectory, _exeFilePath);

            // Start a new process that runs your compiled program
            myProcess = Process.Start(programExePath);

            // Create a timer to periodically check if the process is still running
            checkProcessTimer = new Timer(1000);
            checkProcessTimer.Elapsed += CheckProcessTimer_Elapsed;
            checkProcessTimer.Start();
        }

        private void CheckProcessTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Check if the process has exited
            if (myProcess.HasExited)
            {
                // Stop the service if the process has exited
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            // Stop checking if the process is still running
            checkProcessTimer.Stop();

            // Stop the process that runs your compiled program
            if (myProcess != null && !myProcess.HasExited)
            {
                myProcess.Kill();
            }
        }
    }
}
