using System;
using System.Collections.Generic;
using System.Text;

namespace CsvParser.Models
{
    [Serializable]
    public class ParsedCsvRow
    {
        public int LineNumber { get; set; }

        public string Type { get; set; }

        public string ConcatAB { get; set; }

        public int? SumCD { get; set; }

        public string ErrorMessage { get; set; }
    }
}
