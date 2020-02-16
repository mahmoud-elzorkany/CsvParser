using CsvParser.Logic.Contracts;
using CsvParser.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CsvParser.ConsoleApplication
{
    public class CsvParserEntryPoint
    {
        private readonly ICsvParserLogic CsvParserLogic;

        public CsvParserEntryPoint(ICsvParserLogic csvParserLogic)
        {
            CsvParserLogic = csvParserLogic;
        }

        public async Task Run(string ouputFormat)
        {
            Console.WriteLine("Enter The Uri for the Csv File");
            string csvUri = Console.ReadLine();
            Console.WriteLine();

            if (string.IsNullOrEmpty(csvUri))
            {
                Console.WriteLine("No Uri was provided for the Csv file");

                return;
            }

            OutputFormats outputformat = OutputFormats.console;

            if (!string.IsNullOrEmpty(ouputFormat) &&
                Enum.IsDefined(typeof(OutputFormats), ouputFormat.ToLower()))
            {
                outputformat = (OutputFormats)Enum.Parse(typeof(OutputFormats), ouputFormat.ToLower());
            }

            using (Stream stream = await CsvParserLogic.ParseCsvAsync(csvUri, outputformat))
            {
                if (stream != null)
                {
                    StreamReader reader = new StreamReader(stream);

                    Console.BufferHeight = short.MaxValue - 1;

                    Console.WriteLine(await reader.ReadToEndAsync());
                }
                else
                {
                    Console.WriteLine("The Output stream is empty");
                }
            }
        }
    }
}
