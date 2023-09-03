using NFDBLab.DBHelper;
using NFDBLab.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NFDBLab.Services
{
    public class ArticleService
    {
        private ASDBHelper _myDb;
        public ArticleService()
        {
            _myDb = new ASDBHelper("Article_Table");
        }
        
        public DataTable GetTotal()
        {
            var sql = "select COUNT(*) total from Article_Table";
            DataTable dt = _myDb.GetBySql(sql);
            return dt;
        }

        public DataTable GetSingle(int id)
        {
            DataTable dt = _myDb.Get(id);
            return dt;
        }
        public DataTable GetTable()
        {
            DataTable dt = _myDb.GetAll();
            return dt;
        }
        
        public void Create(Article_Table article)
        {
            article.ClassToDataRow(_myDb.dr);
            _myDb.Create();
        }
        
        
        public void Update(Article_Table article)
        {
            article.ClassToDataRow(_myDb.dr);
            _myDb.Update();
        }
        
        public void Delete(int id)
        {
            _myDb.Delete(id);
        }
        
    }
}