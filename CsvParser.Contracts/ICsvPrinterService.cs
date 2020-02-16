using CsvHelper;
using CsvParser.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CsvParser.Services.Contracts
{
   public interface ICsvPrinterService
    {
        Task<Stream> PrintCsvAsync(IAsyncEnumerable<ParsedCsvRow> rows);
    }
}