using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Data.SqlClient;
using Sueetie.Core;

namespace Saltie.Core
{

    public partial class DbSaltieDataProvider : SaltieDataProvider
    {

        #region ConnectionString

        private string connectionString;
        protected SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }
        public DbSaltieDataProvider(string _connectionString)
        {
            this.connectionString = _connectionString;
        }

        #endregion

        #region Saltie Testing

        public override int GetContentCount(int userid)
        {
            int contentCount = -1;
            using (SqlConnection cn = GetSqlConnection())
            {
                string sql = "select count(*) as 'contentcount' from sueetie_content where userid = " + userid;
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                contentCount = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                cn.Close();
            }
            return contentCount;
        }


        #endregion

    }
}
