using CsvParser.Models;
using System.IO;
using System.Threading.Tasks;

namespace CsvParser.Logic.Contracts
{
  public  interface ICsvParserLogic
    {
        Task<Stream> ParseCsvAsync(string csvFileUrl, OutputFormats format);
    }
}
