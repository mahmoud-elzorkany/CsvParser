﻿using Autofac;
using Microsoft.Extensions.Logging;

namespace CsvParser.ConsoleApplication.DependencyInjectionModules
{
    class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new LoggerFactory()).As<ILoggerFactory>();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
        }
    }
}
