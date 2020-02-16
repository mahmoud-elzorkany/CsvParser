using CsvHelper;
using System.Threading.Tasks;

namespace CsvParser.Services.Contracts
{
   public interface ICsvReaderService
    {
        Task<IReader> GetCsvReaderAsync(string csvFileUrl);
    }
}
