﻿using Autofac;
using CsvParser.Logic;
using CsvParser.Logic.Contracts;

namespace CsvParser.GateWay.DependencyInjectionModules
{
    public class LogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CsvParserLogic>().As<ICsvParserLogic>().SingleInstance();
        }
    }
}
