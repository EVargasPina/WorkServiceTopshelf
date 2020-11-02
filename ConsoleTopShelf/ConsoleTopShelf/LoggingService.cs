using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Timers;

namespace ConsoleTopShelf
{
    public class LoggingService
    {
        private readonly Timer _timer;
        
        public LoggingService()
        {
            _timer = new Timer(3000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs args)
        {
            //Revisa si esta instalado el workservice
            ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "PruebaService");
            ServiceController sc = new ServiceController("PruebaService");
            if (ctl == null)
            {
                //Proceso para ejecutar consola como cmd
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.Verb = "runas";
                process.Start();
                //Vamos a la ruta
                //process.StandardInput.WriteLine("cd " + @"C:\Program Files (x86)\"+Environment.UserName+@"\Setup1");
              //  process.StandardInput.WriteLine("cd " + @"C:\Users\ENZO\source\repos\WorkServiceTopshelf\ConsoleTopShelf\ConsoleTopShelf\bin\Debug\netcoreapp3.1");
                process.StandardInput.WriteLine("cd " + @"C:\Users\ENZO\source\repos\WorkServiceTopshelf\ConsoleTopShelf\ConsoleTopShelf\bin\Debug\netcoreapp3.1");
                //Iniciamos
                process.StandardInput.WriteLine("ConsoleTopShelf.exe install");
                process.StandardInput.WriteLine("ConsoleTopShelf.exe start");
                process.StandardInput.Flush();
                process.StandardInput.Close();
                Console.WriteLine(process.StandardOutput.ReadToEnd());
            }
           /* else
            {
                //Revisa si el workservice se esta detenido
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    //Proceso para ejecutar consola como cmd
                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                    //Vamos a la ruta
                    process.StandardInput.WriteLine("cd " + @"C:\Users\ENZO\source\repos\WorkServiceTopshelf\ConsoleTopShelf\ConsoleTopShelf\bin\Debug\netcoreapp3.1");
                    // process.StandardInput.WriteLine("cd " + @"C:\Program Files (x86)\"+Environment.UserName+@"\Setup1");
                    //Creamos
                    process.StandardInput.WriteLine("ConsoleTopShelf.exe start");
                    Console.WriteLine("Se inicio");
                    process.StandardInput.Flush();
                    process.StandardInput.Close();
                    Console.WriteLine(process.StandardOutput.ReadToEnd());

                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        Log();
                    }
                    Log();
                }
                else if (sc.Status == ServiceControllerStatus.Running)
                {
                    Log();
                }
                Log();
            }*/          
            Log();
        }

        //Escribe dentro de una archivo
        private void Log()
        {
            string[] lines = new string[] { DateTime.Now.ToString() };
            File.AppendAllLines(@"C:\Users\ENZO\Documents\servicelog.txt", lines);
        }

        public static string GetCurrentUser()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            return userName.Remove(0, Environment.MachineName.Length + 1);
        }


        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

    }
}
