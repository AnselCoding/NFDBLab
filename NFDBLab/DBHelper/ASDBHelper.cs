using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NFDBLab.DBHelper
{
    public class ASDBHelper
    {
        private string defConnstring = ConfigurationManager.ConnectionStrings["linkToDb"].ConnectionString;
        private SqlConnection conn;
        private SqlDataAdapter adapter;
        private SqlCommandBuilder builder;
        private DataTable _dt;
        private string _tableName;
        private string _pk;
        public DataRow dr { get; set; }

        /// <summary>
        /// 使用預設連線字串與指定的 table，實作 SqlDataAdapter 與 SqlCommandBuilder。
        /// </summary>
        /// <param name="tableName">需求互動的 table</param>
        public ASDBHelper(string tableName)
        {
            // 建立基礎設施
            _tableName = tableName;
            string sql = $"select * from {tableName}";
            conn = new SqlConnection(defConnstring);
            adapter = new SqlDataAdapter(sql, conn);
            builder = new SqlCommandBuilder(adapter);
            // 初始化設置，完成基礎設施
            Initial();
        }

        public void Initial()
        {
            // 使用 FillSchema 方法填充資料表結構，取得 DataRow 格式
            _dt = new DataTable();
            adapter.FillSchema(_dt, SchemaType.Source);
            dr = _dt.NewRow();

            // 讓 Update、Delete 指令 WHERE 條件只使用 Pkey。
            builder.ConflictOption = ConflictOption.OverwriteChanges;
            // 取得PK
            SqlParameterCollection sqlParameters = builder.GetDeleteCommand(true).Parameters;
            _pk = GetPK(sqlParameters);

            // 設置 INSERT、UPDATE 與 DELETE SQL語法
            adapter.InsertCommand = builder.GetInsertCommand();
            adapter.UpdateCommand = builder.GetUpdateCommand();
            adapter.DeleteCommand = builder.GetDeleteCommand();
        }
        private string GetPK(SqlParameterCollection sqlParameters)
        {
            string pk = string.Empty;
            foreach (SqlParameter sqlParameter in sqlParameters)
            {
                if (sqlParameter.ToString().StartsWith("@Original_"))
                {
                    string parameter = sqlParameter.ToString();
                    pk = parameter.Replace("@Original_", "");
                    break;
                }
            }

            return pk;
        }

        #region more method
        /// <summary>
        /// 使用預設連線字串，實作SqlConnection; 
        /// 實作 SqlDataAdapter;
        /// 使用 SqlDataAdapter 實作 SqlCommandBuilder;
        /// </summary>
        //public MyDBHelper()
        //{
        //    conn = new SqlConnection(defConnstring);
        //    adapter = new SqlDataAdapter();
        //    builder = new SqlCommandBuilder(adapter);
        //    // 讓 Update、Delete 指令 WHERE 條件只使用 Pkey。
        //    builder.ConflictOption = ConflictOption.OverwriteChanges;
        //}
        /// <summary>
        /// 使用提供的連線字串，實作 SqlConnection; 
        /// 使用提供的 DataTable，實作 SqlDataAdapter;
        /// 使用 SqlDataAdapter 實作 SqlCommandBuilder
        /// </summary>
        /// <param name="connstr">連線字串</param>
        /// <param name="initTableName">DataTable名稱</param>
        //public MyDBHelper(string connstr, string initTableName)
        //{
        //    conn = new SqlConnection(connstr);
        //    adapter = new SqlDataAdapter($"select * from {initTableName}", conn);
        //    builder = new SqlCommandBuilder(adapter);
        //}

        /// <summary>
        /// 提供 initTableName 實作MyDBHelper，可以使用此Select()方法，取得該 Table 所有訊息
        /// </summary>
        /// <returns>實作SqlDataAdapter時填入的DataTable</returns>
        //public DataTable Get()
        //{
        //    dt = new DataTable();
        //    adapter.FillSchema(dt, SchemaType.Mapped);
        //    adapter.Fill(dt);
        //    return dt;
        //}

        /// <summary>
        /// SelectCommand 使用提供的查詢語法(T-SQL) 執行 Fill() 方法，取回該 SqlCommand 需求的 DataTable
        /// </summary>
        /// <param name="sql">需求執行的查詢語法T-SQL</param>
        /// <returns>回傳SqlCommand的DataTable</returns>

        #endregion


        #region Select method
        /// <summary>
        /// 使用提供的 T-SQL 執行查詢，取回資料。
        /// </summary>
        /// <param name="sql">需要執行的 T-SQL</param>
        /// <returns>回傳查詢結果</returns>
        public DataTable GetBySql(string sql)
        {
            DataTable dt = new DataTable();
            adapter.SelectCommand = new SqlCommand(sql, conn);
            adapter.Fill(dt);
            return dt;
        }

        /// <summary>
        /// 使用 id 取得指定的資料。
        /// </summary>
        /// <param name="id">需求的資料 id</param>
        /// <returns>回傳一筆要求的資料</returns>
        public DataTable Get(int id)
        {
            DataTable dt = new DataTable();
            string sql = $"select * from {_tableName} WHERE {_pk} = @pk";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@pk", id);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            return dt;
        }

        /// <summary>
        /// 取得指定的 Table 的所有資料。
        /// </summary>
        /// <returns>回傳該 table 的全部內容</returns>
        public DataTable GetAll()
        {
            string sql = $"select * from {_tableName}";
            adapter.SelectCommand = new SqlCommand(sql, conn);
            adapter.FillSchema(_dt, SchemaType.Mapped);
            adapter.Fill(_dt);
            return _dt;
        }
        #endregion

        #region NoQuery
        public void Create()
        {
            try
            {
                _dt.Rows.Add(dr);
                UpdateDB();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update()
        {
            try
            {
                GetAll(); //Get the latest information
                int id = (int)dr[_pk];
                UpdateDataTable(id);
                UpdateDB();
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void UpdateDataTable(int id)
        {
            var rowToUpdate = _dt.Rows.Find(id);

            if (rowToUpdate != null)
            {
                // 逐個更新每個欄位，避開主鍵欄位
                for (int i = 0; i < _dt.Columns.Count; i++)
                {
                    DataColumn column = _dt.Columns[i];
                    if (!column.ReadOnly && column.ColumnName != _pk)
                    {
                        rowToUpdate[column] = dr[column.ColumnName];
                    }
                }
            }
        }

        public void Delete(int id)
        {
            try
            {
                GetAll(); //Get the latest information
                _dt.Rows.Find(id).Delete();

                UpdateDB();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateDB()
        {
            int count = adapter.Update(_dt);
            GetAll();
            dr = _dt.NewRow();
        }
        #endregion

    }
}