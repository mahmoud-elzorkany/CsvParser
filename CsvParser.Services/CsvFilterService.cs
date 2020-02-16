using CsvHelper;
using CsvParser.Models;
using CsvParser.Services.Contracts;
using System;
using System.Collections.Generic;

namespace CsvParser.Services
{
   public class CsvFilterService : ICsvFilterService
    {
        public async IAsyncEnumerable<ParsedCsvRow> FilterCsvOnSumAsync(IReader csvReader)
        {
            int rowNumber = 1;

            while (await csvReader.ReadAsync())
            {
                CsvRow record = csvReader.GetRecord<CsvRow>();

                if (!int.TryParse(record.ColumnC, out int columnC) ||
                    !int.TryParse(record.ColumnD, out int columnD))
                {
                    ParsedCsvRow parsedCsvRow = new ParsedCsvRow
                    {
                        LineNumber = rowNumber,
                        Type = Enum.GetName(typeof(ParsingResult), ParsingResult.error),
                        ErrorMessage = "Error in parsing Columns C & D"
                    };

                    yield return parsedCsvRow;
                }

                else if (columnC + columnD > 100)
                {
                    ParsedCsvRow parsedCsvRow = new ParsedCsvRow
                    {
                        LineNumber = rowNumber,
                        Type = Enum.GetName(typeof(ParsingResult), ParsingResult.ok),
                        ConcatAB = record.ColumnA + record.ColumnB,
                        SumCD = columnC + columnD
                    };

                    yield return parsedCsvRow;
                }

                rowNumber++;
            }
        }
    }
}
