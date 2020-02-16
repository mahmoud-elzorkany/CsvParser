# CsvParser

-A Csv parser developed with C# and the .NetCore Framework mainly designed for parsing and filtering 
large files with lots of records using strems.

-The project contains a console application and a Gateway application(For HTTP Api Requests). 

-Both accept an Url for a downloadable Csv file.

-The reading of the Csv is done via a stream.

-The Csv gets filtered according to specified filteration rules.

-The Filtered results gets streamed back to the user in the desired format (json,Xml,Text).
