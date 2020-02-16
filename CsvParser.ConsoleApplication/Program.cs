using System.Threading.Tasks;
using Autofac;
using CsvParser.ConsoleApplication.DependencyInjectionModules;

namespace CsvParser.ConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await GetContainer().Resolve<CsvParserEntryPoint>().Run(args.Length != 0 ? args[0] : "");
        }

        private static IContainer GetContainer()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<CsvParserEntryPoint>();
            containerBuilder.RegisterModule(new LogicModule());
            containerBuilder.RegisterModule(new ServiceModule());
            containerBuilder.RegisterModule(new LoggerModule());

            return containerBuilder.Build();
        }
    }
}
