using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using Topshelf;

namespace ConsoleTopShelf
{
    class LoggingService : ServiceControl
    {
        private const string _logFileLocation = @"C:\Users\ENZO\Documents\servicelog.txt";

        private void Log(string logMessage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));
            File.AppendAllText(_logFileLocation, DateTime.UtcNow.ToString() + " : " + logMessage + Environment.NewLine);
        }


        public bool Start(HostControl hostControl)
        {
            //Revisa si esta instalado el workservice
            ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "TestService");
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
                process.StandardInput.WriteLine("cd " + @"C:\Users\ENZO\source\repos\WorkServiceTopshelf\ConsoleTopShelf\ConsoleTopShelf\bin\Release\netcoreapp3.1\win-x64\publish");
                //Creamos
                process.StandardInput.WriteLine("ConsoleTopShelf.exe install");
                Console.WriteLine("Se creo workservice");
                process.StandardInput.WriteLine("ConsoleTopShelf.exe start");
                Console.WriteLine("Se inicio");
                process.StandardInput.Flush();
                process.StandardInput.Close();
                Console.WriteLine(process.StandardOutput.ReadToEnd());

                ServiceController sc = new ServiceController("TestService");
                    
                if(sc.Status == ServiceControllerStatus.Running)
                {
                    Log("Starting");
                }

            }
            else
            { 
                //Revisa si el workservice se esta ejecutando
                if (IsOS(OS_ANYSERVER) == false)
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
                    process.StandardInput.WriteLine("cd " + @"C:\Users\ENZO\source\repos\WorkServiceTopshelf\ConsoleTopShelf\ConsoleTopShelf\bin\Release\netcoreapp3.1\win-x64\publish");
                    //Creamos
                    process.StandardInput.WriteLine("ConsoleTopShelf.exe start");
                    Console.WriteLine("Se inicio");
                    process.StandardInput.Flush();
                    process.StandardInput.Close();
                    Console.WriteLine(process.StandardOutput.ReadToEnd());

                    ServiceController sc = new ServiceController("TestService");

                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        Log("Starting");
                    }
                }
            }

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Log("Stopping");
            return true;
        }

        const int OS_ANYSERVER = 29;

        [DllImport("shlwapi.dll", SetLastError = true, EntryPoint = "#437")]
        public static extern bool IsOS(int os);
    }
}
