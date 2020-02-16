using CsvParser.Models;
using CsvParser.Services.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CsvParser.Services
{
    public class CsvXmlPrinterService : ICsvPrinterService
    {
        private readonly ILogger<CsvXmlPrinterService> Logger;

        public CsvXmlPrinterService(ILogger<CsvXmlPrinterService> logger)
        {
            Logger = logger;
        }

        public async Task<Stream> PrintCsvAsync(IAsyncEnumerable<ParsedCsvRow> rows)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                StreamWriter writer = new StreamWriter(memoryStream);

                XmlSerializer serializer = new XmlSerializer(typeof(ParsedCsvRow));

                await foreach (var row in rows)
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
