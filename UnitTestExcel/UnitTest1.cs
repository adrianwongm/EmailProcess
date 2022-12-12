using ExcelReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestExcel
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ReadExcel()
        {
            var pathExcel = $"C:\\Users\\Adria\\source\\repos\\EmailReadAtachment\\EmailReadAtachment\\archivos\\Formato.xls";
            Lector.procesarExcel(pathExcel,"Excel");
             
        }
    }
}
