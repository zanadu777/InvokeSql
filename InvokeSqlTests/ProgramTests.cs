using System;
using FluentAssertions;
using InvokeSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InvokeSqlTests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void TestGenerateFileName()
        {
            var generationTime = DateTime.Parse("2020-06-19T20:11:55.0055928-04:00");
            var options = new Options { AutoName = true, OutputDirectory = @"D:\Temp\InvokeSql\Output" };
            var sqlPath = @"D:\Temp\InvokeSql\Input\Test.sql";

            var exportFile = Program.GenerateFileName(options, sqlPath, generationTime);
            exportFile.Should().Be(@"D:\Temp\InvokeSql\Output\Test.xlsx");

            options = new Options { AutoName = true, OutputDirectory = @"D:\Temp\InvokeSql\Output" , IsDateAppended = true};
            exportFile = Program.GenerateFileName(options, sqlPath, generationTime);
            exportFile.Should().Be(@"D:\Temp\InvokeSql\Output\Test_2020-11-19.xlsx");

            options = new Options { AutoName = true, OutputDirectory = @"D:\Temp\InvokeSql\Output", IsDateTimeAppended =  true };
            exportFile = Program.GenerateFileName(options, sqlPath, generationTime);
            exportFile.Should().Be(@"D:\Temp\InvokeSql\Output\Test_2020-11-19_T_20-11-55.xlsx");



            options = new Options {   OutputDirectory = @"D:\Temp\InvokeSql\Output" , ExcelOut = "output.xlsx"};
            exportFile = Program.GenerateFileName(options, sqlPath, generationTime);
            exportFile.Should().Be(@"D:\Temp\InvokeSql\Output\output.xlsx");


            options = new Options {  ExcelOut = @"D:\Temp\output.xlsx" };
            exportFile = Program.GenerateFileName(options, sqlPath, generationTime);
            exportFile.Should().Be(@"D:\Temp\output.xlsx");

        }
    }
}
