using System.Collections.Generic;

namespace CsvParser.GateWay.HelperMethods
{
    public static class MimeProvider
    {
       public static string GetMime(string extension)
        {
            switch(extension)
            {
                case "json":
                    return "application/json";

                case "xml":
                    return "text/xml";

                case "txt":
                    return "text/plain";

                default:
                    return "";
            }
        }
    }
}
