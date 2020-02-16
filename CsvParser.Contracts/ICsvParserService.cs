using CsvParser.Models;
using System.IO;
using System.Threading.Tasks;

namespace CsvParser.Services.Contracts
{
    public interface ICsvParserService
    {
        Task<Stream> ParseCsvAsync(string csvUri, OutputFormats outputFormat);
    }
}