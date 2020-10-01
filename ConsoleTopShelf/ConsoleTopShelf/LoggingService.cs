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
            _timer = new Timer(5000.0) { AutoReset = true };
            _timer.Elapsed += ExecuteService;
        }

        private void ExecuteService(object sender, ElapsedEventArgs args)
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
                process.StandardInput.WriteLine("cd " + @"C:\Program Files (x86)\"+Environment.UserName+@"\Setup1");
                //Creamos
                process.StandardInput.WriteLine("ConsoleTopShelf.exe install");
                Console.WriteLine("Se creo workservice");
                process.StandardInput.WriteLine("ConsoleTopShelf.exe start");
                Console.WriteLine("Se inicio");
                process.StandardInput.Flush();
                process.StandardInput.Close();
                Console.WriteLine(process.StandardOutput.ReadToEnd());


                if (sc.Status == ServiceControllerStatus.Running)
                {
                    Log("Starting");
                }

            }
            else
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
                    process.StandardInput.WriteLine("cd " + @"C:\Program Files (x86)\"+Environment.UserName+@"\Setup1");
                    //Creamos
                    process.StandardInput.WriteLine("ConsoleTopShelf.exe start");
                    Console.WriteLine("Se inicio");
                    process.StandardInput.Flush();
                    process.StandardInput.Close();
                    Console.WriteLine(process.StandardOutput.ReadToEnd());
                    
                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        Log("Starting");
                    }
                }
                else if (sc.Status == ServiceControllerStatus.Running)
                {
                    Log("Starting");
                }
            }

            try
            {
                _timer.Stop();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Something went wrong:"+ ex.Message );
            }
            finally
            {
                _timer.Start();
            }
        }

        //Escribe dentro de una archivo
        private void Log(string logMessage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(@"C:\Users\" + Environment.UserName + @"\Documents\servicelog.txt"));
            File.AppendAllText(@"C:\Users\" + Environment.UserName + @"\Documents\servicelog.txt", DateTime.UtcNow.ToString() + " : " + logMessage + Environment.NewLine);
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
