using CsvHelper;
using CsvParser.Logic;
using CsvParser.Logic.Contracts;
using CsvParser.Models;
using CsvParser.Services;
using CsvParser.Services.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xunit;

namespace CsvParser.Tests
{
    public class CsvParserTests
    {
        [Fact]
        public void Should_Return_Null_For_Invalid_Urls_Or_Without_Csv_Extension()
        {
            ICsvParserService csvParserService = Substitute.For<ICsvParserService>();
            ILogger<CsvParserLogic> logger = Substitute.For<ILogger<CsvParserLogic>>();

            ICsvParserLogic csvParserLogic = new CsvParserLogic(csvParserService, logger);
            IEnumerable<string> invalidUrls;

            using (Stream stream = new MemoryStream())
            {
                csvParserService.ParseCsvAsync(Arg.Any<string>(), Arg.Any<OutputFormats>()).Returns(Task.FromResult(stream));

                invalidUrls = GetInvalidUrls();

                foreach (string url in invalidUrls)
                {
                    Stream result = csvParserLogic.ParseCsvAsync(url, OutputFormats.json).Result;
                    Assert.Null(result);
                }

                IEnumerable<string> validUrls = GetValidUrls();

                foreach (string url in validUrls)
                {
                    Stream result = csvParserLogic.ParseCsvAsync(url, OutputFormats.json).Result;
                    Assert.NotNull(result);
                }
            }
        }

        [Fact]
        public async Task Should_Filter_The_Csv_File_Correctly()
        {
            string csvFileUrl = "https://beezupcdn.blob.core.windows.net/recruitment/bigfile.csv";

            ILogger<CsvReaderService> logger = Substitute.For<ILogger<CsvReaderService>>();
            ICsvFilterService csvFilter;
            ICsvReaderService csvReaderService = new CsvReaderService(logger);
            IAsyncEnumerable<ParsedCsvRow> filteredRows;

            using (IReader csvReader = await csvReaderService.GetCsvReaderAsync(csvFileUrl))
            {
                Assert.NotNull(csvReader);

                if(csvReader != null)
                {
                    csvFilter = new CsvFilterService();

                    filteredRows = csvFilter.FilterCsvOnSumAsync(csvReader);

                    await foreach (ParsedCsvRow row in filteredRows)
                    {
                        Assert.False(string.IsNullOrEmpty(row.Type));
                        Assert.True(Enum.IsDefined(typeof(ParsingResult), row.Type));

                        if (row.Type == Enum.GetName(typeof(ParsingResult), ParsingResult.ok))
                        {
                            Assert.True(row.SumCD > 100);
                            Assert.True(string.IsNullOrEmpty(row.ErrorMessage));
                        }

                        if (row.Type == Enum.GetName(typeof(ParsingResult), ParsingResult.error))
                        {
                            Assert.Null(row.SumCD);
                            Assert.False(string.IsNullOrEmpty(row.ErrorMessage));
                        }
                    }
                }
            }
        }

        [Fact]
        public async Task Should_Serialize_Json_Correctly()
        {
            ILogger<CsvJsonPrinterService> logger = Substitute.For<ILogger<CsvJsonPrinterService>>();

            ICsvPrinterService jsonCsvPrinter = new CsvJsonPrinterService(logger);

            IAsyncEnumerable<ParsedCsvRow> parsedRows = GetParsedRow();

            using (Stream outputStream = await jsonCsvPrinter.PrintCsvAsync(parsedRows))
            {
                StreamReader reader = new StreamReader(outputStream);

                JsonSerializer jsonSerializer = new JsonSerializer();
                ParsedCsvRow deserializedJsonRecords = (ParsedCsvRow)jsonSerializer.Deserialize(reader, typeof(ParsedCsvRow));

                await foreach (var row in parsedRows)
                {
                    Assert.Equal(row.LineNumber, row.LineNumber);
                    Assert.Equal(row.Type, row.Type);
                    Assert.Equal(row.ConcatAB, row.ConcatAB);
                    Assert.Equal(row.SumCD, row.SumCD);
                    Assert.Equal(row.ErrorMessage, row.ErrorMessage);
                }
            }
        }

        [Fact]
        public async Task Should_Serialize_Xml_Correctly()
        {
            ILogger<CsvXmlPrinterService> logger = Substitute.For<ILogger<CsvXmlPrinterService>>();

            ICsvPrinterService xmlCsvPrinter = new CsvXmlPrinterService(logger);

            IAsyncEnumerable<ParsedCsvRow> parsedRows = GetParsedRow();

            using (Stream outputStream = await xmlCsvPrinter.PrintCsvAsync(parsedRows))
            {
                StreamReader reader = new StreamReader(outputStream);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ParsedCsvRow));
                ParsedCsvRow dserializedXmlRecord = (ParsedCsvRow)xmlSerializer.Deserialize(reader);

                await foreach (var row in parsedRows)
                {
                    Assert.Equal(row.LineNumber, row.LineNumber);
                    Assert.Equal(row.Type, row.Type);
                    Assert.Equal(row.ConcatAB, row.ConcatAB);
                    Assert.Equal(row.SumCD, row.SumCD);
                    Assert.Equal(row.ErrorMessage, row.ErrorMessage);
                }
            }
        }

        public IEnumerable<string> GetInvalidUrls()
        {
            IEnumerable<string> invalidUrls = new List<string>()
            {
                "https://beezupcdn.blob.core.windows.net/recruitment/bigfile.txt",
                "http://",
                "http://.",
                "http://..",
                "http://../",
                "http://?",
                "http://??",
                "http://??/",
                "http://#",
                "http://##",
                "http://##/",
                "http://foo.bar?q=Spaces should be encoded",
                "//",
                "//a",
                "///a",
                "///",
                "http:///a",
                "foo.com",
                "rdar://1234",
                "h://test",
                "http:// shouldfail.com",
                ":// should fail",
                "http://foo.bar/foo(bar)baz quux",
                "ftps://foo.bar/",
                "http://-error-.invalid/",
                "http://a.b--c.de/",
                "http://-a.b.co",
                "http://a.b-.co",
                "http://0.0.0.0",
                "http://10.1.1.0",
                "http://10.1.1.255",
                "http://224.1.1.1",
                "http://1.1.1.1.1",
                "http://123.123.123",
                "http://3628126748",
                "http://.www.foo.bar/",
                "http://www.foo.bar./",
                "http://.www.foo.bar./",
                "http://10.1.1.1",
                "http://10.1.1.254"
            };

            return invalidUrls;
        }

        public IEnumerable<string> GetValidUrls()
        {
            IEnumerable<string> validUrls = new List<string>()
            {
                "https://beezupcdn.blob.core.windows.net/recruitment/bigfile.csv",
                "http://foo.com/blah_blah/file.csv",
                "http://foo.com/blah_blah/file.csv",
                "http://userid@example.com:8080/file.csv",
                "http://userid@example.com:8080/file.csv",
                "http://userid:password@example.comf/ile.csv",
                "http://userid:password@example.com/fle.csv",
                "http://223.255.255.254/file.csv"
            };

            return validUrls;
        }

        public async IAsyncEnumerable<ParsedCsvRow> GetParsedRow()
        {
            IList<ParsedCsvRow> parsedCsvRows = new List<ParsedCsvRow>()
            {
                new ParsedCsvRow()
                {
                    LineNumber = 1,
                    Type = "ok",
                    ConcatAB = "ColumnAColumnB",
                    SumCD = 101,
                    ErrorMessage = "No Errors"
                }
            };

            foreach (var row in parsedCsvRows)
            {
                yield return row;
            }
        }
    }
}
