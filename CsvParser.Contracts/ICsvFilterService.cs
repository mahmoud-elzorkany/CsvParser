using CsvHelper;
using CsvParser.Models;
using System.Collections.Generic;

namespace CsvParser.Services.Contracts
{
    public interface ICsvFilterService
    {
        IAsyncEnumerable<ParsedCsvRow> FilterCsvOnSumAsync(IReader csvReader);
    }
}
