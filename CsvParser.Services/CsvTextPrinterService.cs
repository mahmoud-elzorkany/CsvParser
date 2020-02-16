using CsvParser.Models;
using CsvParser.Services.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CsvParser.Services
{
    public class CsvTextPrinterService : ICsvPrinterService
    {
        private readonly ILogger<CsvTextPrinterService> Logger;

        public CsvTextPrinterService(ILogger<CsvTextPrinterService> logger)
        {
            Logger = logger;
        }

        public async Task<Stream> PrintCsvAsync(IAsyncEnumerable<ParsedCsvRow> rows)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                StreamWriter writer = new StreamWriter(memoryStream);

                await foreach (ParsedCsvRow row in rows)
                {
                    if (string.IsNullOrEmpty(row.ErrorMessage))
                    {
                        await writer.WriteLineAsync($"lineNumber:{row.LineNumber}, type:{row.Type}, ConcatAb:{row.ConcatAB}, SumCD:{row.SumCD}");
                    }
                    else
                    {
                        await writer.WriteLineAsync($"lineNumber:{row.LineNumber}, type:{row.Type}, errorMessage:{row.ErrorMessage}");
                    }
                }

                memoryStream.Seek(0, SeekOrigin.Begin);

                return writer.BaseStream;
            }
            catch (Exception e)
            {
                Logger.LogError($"Error: {e.Message}");

                return null;
            }
        }
    }
}
