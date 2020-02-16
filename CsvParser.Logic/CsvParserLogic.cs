using System;
using System.IO;
using System.Threading.Tasks;
using CsvParser.Logic.Contracts;
using CsvParser.Models;
using CsvParser.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace CsvParser.Logic
{
    public class CsvParserLogic : ICsvParserLogic
    {
        private readonly ICsvParserService CsvParserService;
        private readonly ILogger<CsvParserLogic> Logger;

        public CsvParserLogic(
            ICsvParserService csvParserService,
            ILogger<CsvParserLogic> logger)
        {
            CsvParserService = csvParserService;
            Logger = logger;
        }

        public async Task<Stream> ParseCsvAsync(string csvUri, OutputFormats ouputFormat)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(csvUri, UriKind.Absolute))
                {
                    Console.WriteLine("The Uri is not Well Formatted");

                    return null;
                }

                bool isUriCreated = Uri.TryCreate(csvUri, UriKind.Absolute, out Uri uri);

                if(isUriCreated == false || uri == null)
                {
                    Console.WriteLine("Invalid Uri");

                    return null;
                }


                if (Path.GetExtension(csvUri) != ".csv")
                {
                    Console.WriteLine("Invalid Csv file");

                    return null;
                }

                return await CsvParserService.ParseCsvAsync(csvUri, ouputFormat);
            }
            
            catch(Exception e)
            {
                Logger.LogError($"Error: {e.Message}");

                return null;
            }
        }
    }
}