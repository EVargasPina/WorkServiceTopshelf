using System;
using Topshelf;
using Topshelf.ServiceConfigurators;

namespace ConsoleTopShelf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var exitCode = HostFactory.Run(configurarion =>
                {
                    configurarion.Service<LoggingService>(s =>
                    {
                        s.ConstructUsing(ServiceBuilderException => new LoggingService());
                        s.WhenStarted(service => service.Start());
                        s.WhenStopped(service => service.Stop());
                    });
                    configurarion.RunAsLocalSystem();
                    configurarion.SetServiceName("PruebaService");
                    configurarion.SetDisplayName("PruebaService");
                    configurarion.SetDescription("Hola");
                    configurarion.StartAutomatically();
                    configurarion.EnableServiceRecovery(recoveryOption =>
                    {
                        recoveryOption.RestartComputer(2,"Hubo un error se va a reiniciar el equipo");
                    });
                });

                int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
                Environment.ExitCode = exitCodeValue;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
