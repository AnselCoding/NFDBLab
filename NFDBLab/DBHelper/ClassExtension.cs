using NFDBLab.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NFDBLab.DBHelper
{
    public static class ClassExtension
    {
        /// <summary>
        /// table Class 物件轉為 DataRow 物件。
        /// </summary>
        /// <param name="table">進行擴充方法的對象，轉換來源</param>
        /// <param name="dr">轉換的結果</param>
        public static void ClassToDataRow<T>(this T table, DataRow dr) where T : class
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var colName = property.Name;
                dr[colName] = property.GetValue(table);
            }
            //return dr;
        }
    }
}