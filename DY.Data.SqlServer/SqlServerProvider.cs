using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DY.Data
{
    public class SqlServerProvider : IDbProvider
    {
        public DbProviderFactory Instance()
        {
            return SqlClientFactory.Instance;
        }

        public void DeriveParameters(IDbCommand cmd)
        {
            if ((cmd as SqlCommand) != null)
            {
                SqlCommandBuilder.DeriveParameters(cmd as SqlCommand);
            }
        }

        public DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size)
        {
            SqlParameter param;

            //return new SqlParameter();

            if (Size > 0)
                param = new SqlParameter(ParamName, (SqlDbType)DbType, Size);
            else
                param = new SqlParameter(ParamName, (SqlDbType)DbType);

            return param;
        }

        public DbType MakeDbType(Type type)
        {
            DbType dbType = (DbType)SqlDbType.NVarChar;
            if (type == typeof(System.Int32) || type == typeof(System.Int16) || type == typeof(System.Int64))
                dbType = (DbType)SqlDbType.Int;
            else if (type == typeof(System.DateTime))
                dbType = (DbType)SqlDbType.DateTime;
            else if (type == typeof(System.Decimal))
                dbType = (DbType)SqlDbType.Decimal;
            else if (type == typeof(System.Boolean))
                dbType = (DbType)SqlDbType.Bit;

            return dbType;
        }

        public bool IsFullTextSearchEnabled()
        {
            return true;
        }

        public bool IsCompactDatabase()
        {
            return true;
        }

        public bool IsBackupDatabase()
        {
            return true;
        }

        public string GetLastIdSql()
        {
            return "SELECT SCOPE_IDENTITY()";
        }

        public bool IsDbOptimize()
        {
            return false;
        }

        public bool IsShrinkData()
        {
            return true;
        }

        public bool IsStoreProc()
        {
            return true;
        }
    }
}
