using CsvParser.Models;
using CsvParser.Services.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CsvParser.Services
{
    public class CsvConsolePrinterService : ICsvPrinterService
    {
        private readonly ILogger<CsvConsolePrinterService> Logger;

        public CsvConsolePrinterService(ILogger<CsvConsolePrinterService> logger)
        {
            Logger = logger;
        }

        public async Task<Stream> PrintCsvAsync(IAsyncEnumerable<ParsedCsvRow> rows)
        {
            try
            {
               await foreach (ParsedCsvRow row in rows)
                {
                    if(string.IsNullOrEmpty(row.ErrorMessage))
                    {
                        Console.WriteLine(row.ConcatAB);
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                Logger.LogError($"Error: {e.Message}");

                return null;
            }
        }
    }
}
