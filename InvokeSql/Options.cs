using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using SqlAutomation;

namespace InvokeSql
{
  public  class Options
    {

        [Option('c', "connectionString", Required = true, HelpText = "Connection string for database.")]
        public string ConnectionString { get; set; }

        [Option('f', "file", Required = true, HelpText = "sql file to execute")]
        public string File { get; set; }

        [Option('i', "indir",  HelpText = "Default directory for input")]
        public string InputDirectory { get; set; }

        [Option('o', "outdir", HelpText = "Default directory for output")]
        public string OutputDirectory { get; set; }

        //public string Directory { get; set; }

        [Option('e', "excel", HelpText = "name of excel file")]
        public string ExcelOut { get; set; }

        [Option('a', "auto", HelpText = "names the excel file after the sql file")]
        public bool AutoName { get; set; }

        [Option(   "date", HelpText = "appends the date and time to the end of the excel file")]
        public bool IsDateAppended { get; set; }

        [Option("datetime", HelpText = "appends the date and time to the end of the excel file")]
        public bool IsDateTimeAppended { get; set; }

        [Option( "timeformat", HelpText = "appends the date and time to the end of the excel file")]
        public string TimeFormatToAppend { get; set; }

    }
}
