using System;
using System.Text;
using System.Reflection;
using System.Configuration;

namespace DY.Data
{
    /// <summary>
    /// 数据驱动类
    /// </summary>
	public class DatabaseProvider
	{
		private DatabaseProvider()
		{ }

		private static IDataProvider _instance = null;
		private static object lockHelper = new object();

		static DatabaseProvider()
		{
			GetProvider();
		}

		private static void GetProvider()
		{
			try
			{
                _instance = (IDataProvider)Activator.CreateInstance(Type.GetType(string.Format("DY.Data.{0}.DataProvider, DY.Data.{0}", ConfigurationManager.AppSettings["dbType"]), false, true));
			}
			catch
			{
				throw new Exception("请检查web.config中dbType节点数据库类型是否正确，例如：SqlServer、Access、MySql");
			}
		}

		public static IDataProvider GetInstance()
		{
			if (_instance == null)
			{
				lock (lockHelper)
				{
					if (_instance == null)
					{
						GetProvider();
					}
				}
			}
			return _instance;
		}

        public static void ResetDbProvider()
        {
            _instance = null; 
        }
	}
}
