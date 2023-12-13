using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080072.DataLayers.SQLServer
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class _BaseDAL
    {
        protected string _connectionString;
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public _BaseDAL(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Tạo và mở kết nối đến cơ sở dữ liệu
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _connectionString;
            conn.Open();
            return conn;
        }
    }
}
