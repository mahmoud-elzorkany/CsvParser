using CsvParser.Models;
using CsvParser.Services.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CsvParser.Services
{
    public class CsvJsonPrinterService : ICsvPrinterService
    {
        private readonly ILogger<CsvJsonPrinterService> Logger;

        public CsvJsonPrinterService(ILogger<CsvJsonPrinterService> logger)
        {
            Logger = logger;
        }

        public async Task<Stream> PrintCsvAsync(IAsyncEnumerable<ParsedCsvRow> rows)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                StreamWriter writer = new StreamWriter(memoryStream);

                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Formatting = Formatting.Indented;

                await foreach (ParsedCsvRow row in rows)
                {
                    serializer.Serialize(writer, row);
                    await writer.WriteLineAsync();
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
