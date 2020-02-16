using CsvHelper;
using CsvHelper.Configuration;
using CsvParser.Services.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CsvParser.Services
{
    public class CsvReaderService : ICsvReaderService
    {
        private readonly ILogger<CsvReaderService> Logger;

        public CsvReaderService(ILogger<CsvReaderService> logger)
        {
            Logger = logger;
        }

        public async Task<IReader> GetCsvReaderAsync(string csvUri)
        {
            try
            {
                HttpClient client = new HttpClient();
                Stream stream = await client.GetStreamAsync(csvUri);

                if (stream != null)
                {
                    StreamReader streamReader = new StreamReader(stream);

                    CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture);
                    configuration.HasHeaderRecord = false;
                    configuration.Delimiter = ";";
                    configuration.MissingFieldFound = null;

                    IReader csvReader = new CsvReader(streamReader, configuration);
                    client.Dispose();

                    return csvReader;
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