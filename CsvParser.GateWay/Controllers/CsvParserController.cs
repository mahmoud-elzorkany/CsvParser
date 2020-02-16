using System;
using System.IO;
using System.Threading.Tasks;
using CsvParser.GateWay.HelperMethods;
using CsvParser.Logic.Contracts;
using CsvParser.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CsvParser.GateWay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CsvParserController : ControllerBase
    {
        private readonly ICsvParserLogic CsvParserlogic;

        public CsvParserController(ICsvParserLogic csvParserlogic)
        {
            CsvParserlogic = csvParserlogic;
        }

        [HttpGet("filter")]
        [HttpPost("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<Stream>> ParseCsvAsync([FromQuery]string csvUri, [FromQuery] string outPutFormat)
        {
            if (string.IsNullOrEmpty(csvUri))
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(outPutFormat)||
                !Enum.IsDefined(typeof(OutputFormats), outPutFormat.ToLower()) || 
                outPutFormat == "console")
            {
                outPutFormat = "json";
            }

            Stream outputStream = await CsvParserlogic.ParseCsvAsync(csvUri, (OutputFormats)Enum.Parse(typeof(OutputFormats), outPutFormat.ToLower()));

            if (outputStream != null)
            {
                FileStreamResult outputFileStreamResult = new FileStreamResult(outputStream, MimeProvider.GetMime(outPutFormat));

                return outputFileStreamResult;
            }

            return BadRequest();
        }
    }
}