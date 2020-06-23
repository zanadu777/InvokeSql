using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ClosedXML.Excel;
using CommandLine;
using SqlAutomation;

namespace InvokeSql
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    var sqls = new Dictionary<string, string>();
                    var sqlPath = GernerateSqlFilePath(o, o.File);
                    var fileSql = System.IO.File.ReadAllText(sqlPath);
                    sqls.Add(o.File, fileSql);

                    var sqlServer = new SqlServerInstance(o.ConnectionString);
                    foreach (var sql in sqls)
                    {
                        var ds = sqlServer.ExecuteQuery(sql.Value);

                        using (var workbook = new XLWorkbook())
                        {
                            foreach (DataTable table in ds.Tables)
                            {
                                var workshet = workbook.Worksheets.Add(table);
                                for (var iCol = 1; iCol <= table.Columns.Count; iCol++)
                                    workshet.Column(iCol).AdjustToContents();
                            }

                            var outputFileName = GenerateFileName(o, sql.Key, DateTime.Now);
                            workbook.SaveAs(outputFileName);
                        }
                    }

                });
            // Console.Read();
        }


        private static String GernerateSqlFilePath(Options o, string sqlFileName)
        {
            if (File.Exists(sqlFileName))
                return sqlFileName;


            var inInputFolder = Path.Combine(o.InputDirectory, sqlFileName);
            if (File.Exists(inInputFolder))
                return inInputFolder;

            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var inAssemblyDirectory = Path.Combine(assemblyPath, sqlFileName);
            if (File.Exists(inAssemblyDirectory))
                return inAssemblyDirectory;

            throw new Exception($"Sql Source {sqlFileName} file not found");
        }

        public static string GenerateFileName(Options o, string sqlFileName, DateTime generationTime)
        {
            FilePath filePath = null;
            // an output file is directly specified 
            if (o.AutoName && !string.IsNullOrWhiteSpace(o.ExcelOut))
                throw new ArgumentException("Cannot specify both  AutoName and a specified file name");

            if (!o.AutoName && string.IsNullOrWhiteSpace(o.ExcelOut))
                throw new ArgumentException("Either specify AutoName or a specified file name");
            if (o.AutoName)
            {
                filePath = new FilePath(new DirectoryInfo(o.OutputDirectory), new FileInfo(sqlFileName));
                filePath.SetExtension(".xlsx");
            }
            else if (FilePath.TestFilePath(o.ExcelOut))
                filePath = new FilePath(new FileInfo(o.ExcelOut));
            else
                filePath = new FilePath(new DirectoryInfo(o.OutputDirectory), new FileInfo(o.ExcelOut));

            Debug.Assert(filePath != null);

            if (!string.IsNullOrWhiteSpace(o.TimeFormatToAppend))
                filePath.Append(generationTime, o.TimeFormatToAppend);
            else if (o.IsDateTimeAppended)
                filePath.Append(generationTime, "_yyyy-m-dd_T_H-m-s");
            else if (o.IsDateAppended)
                filePath.Append(generationTime, "_yyyy-m-dd");

            return filePath.ToString();
        }
    }
}
