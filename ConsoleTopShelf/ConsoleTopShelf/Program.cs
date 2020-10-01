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
                HostFactory.Run(configurarion =>
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
                    configurarion.StartAutomaticallyDelayed();
                    configurarion.EnableServiceRecovery(recoveryOption =>
                    {
                        recoveryOption.RestartService(0);
                    });
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
