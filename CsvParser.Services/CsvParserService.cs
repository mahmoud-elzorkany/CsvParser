using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvParser.Models;
using CsvParser.Services.Contracts;
using Autofac.Features.Indexed;
using Microsoft.Extensions.Logging;

namespace CsvParser.Services
{
    public class CsvParserService : ICsvParserService
    {
        private readonly ICsvReaderService CsvReaderService;
        private readonly ICsvFilterService CsvFilterService;
        private readonly IIndex<OutputFormats, ICsvPrinterService> Printers;
        private readonly ILogger<CsvParserService> Logger;

        public CsvParserService(
            ICsvReaderService csvReaderService,
            ICsvFilterService csvFilterService,
            IIndex<OutputFormats, ICsvPrinterService> printers,
            ILogger<CsvParserService> logger)
        {
            CsvReaderService = csvReaderService;
            CsvFilterService = csvFilterService;
            Printers = printers;
            Logger = logger;
        }

        public async Task<Stream> ParseCsvAsync(string csvUri, OutputFormats outputFormat)
        {
            try
            {
                IReader csvReader = await CsvReaderService.GetCsvReaderAsync(csvUri);

                if (csvReader != null)
                {
                    IAsyncEnumerable<ParsedCsvRow> filteredRows = CsvFilterService.FilterCsvOnSumAsync(csvReader);

                    ICsvPrinterService CsvPrinter = Printers[outputFormat];

                    Stream outputStream = await CsvPrinter.PrintCsvAsync(filteredRows);

                    csvReader.Dispose();

                    return outputStream;
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