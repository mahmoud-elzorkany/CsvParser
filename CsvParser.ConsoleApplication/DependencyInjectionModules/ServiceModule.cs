using Autofac;
using CsvHelper;
using CsvParser.Models;
using CsvParser.Services;
using CsvParser.Services.Contracts;

namespace CsvParser.ConsoleApplication.DependencyInjectionModules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CsvParserService>().As<ICsvParserService>().SingleInstance();
            builder.RegisterType<CsvReaderService>().As<ICsvReaderService>().SingleInstance();
            builder.RegisterType<CsvFilterService>().As<ICsvFilterService>().SingleInstance();
            builder.RegisterType<CsvConsolePrinterService>().Keyed<ICsvPrinterService>(OutputFormats.console).SingleInstance();
            builder.RegisterType<CsvJsonPrinterService>().Keyed<ICsvPrinterService>(OutputFormats.json).SingleInstance();
            builder.RegisterType<CsvXmlPrinterService>().Keyed<ICsvPrinterService>(OutputFormats.xml).SingleInstance();
            builder.RegisterType<CsvTextPrinterService>().Keyed<ICsvPrinterService>(OutputFormats.txt).SingleInstance();
        }
    }
}
