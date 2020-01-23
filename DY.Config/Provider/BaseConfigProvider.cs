using System;
using System.IO;
using System.Web;
using DY.Common;

namespace DY.Config.Provider
{
	public class BaseConfigProvider
	{
		private BaseConfigProvider()
		{
		}

		static BaseConfigProvider()
		{
			config = GetRealBaseConfig();
		}

        private static BaseDataInfo config = null;

		/// <summary>
		/// ��ȡ��������ʵ��
		/// </summary>
		/// <returns></returns>
        public static BaseDataInfo Instance()
		{
			return config;
		}

		/// <summary>
		/// ���ö���ʵ��
		/// </summary>
		/// <param name="anConfig"></param>
        public static void SetInstance(BaseDataInfo anConfig)
		{
			if (anConfig == null)
				return;
			config = anConfig;
		}

		/// <summary>
		/// ��ȡ��ʵ�������ö���
		/// </summary>
		/// <returns></returns>
        public static BaseDataInfo GetRealBaseConfig()
		{
            BaseDataInfo newBaseConfig = null;
            string filename = BaseConfigFileManager.ConfigFilePath;
			//string filename = null;            
            //HttpContext context = HttpContext.Current;
            //if(context != null)
            //    filename = context.Server.MapPath("/DNT.config");
            //else
            //    filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  "DNT.config");

			try
			{
				newBaseConfig = (BaseDataInfo)SerializationHelper.Load(typeof(BaseDataInfo), filename);
			}
			catch
			{
				newBaseConfig = null;
			}
			
			if (newBaseConfig == null)
			{
				try
				{
					BaseDataInfoCollection bcc = (BaseDataInfoCollection)SerializationHelper.Load(typeof(BaseDataInfoCollection), filename);
					foreach (BaseDataInfo bc in bcc)
					{
						if (Utils.GetTrueForumPath() == bc.Forumpath)
						{
							newBaseConfig = bc;
							break;
						}
					}
					if (newBaseConfig == null)
					{
						BaseDataInfo rootConfig = null;
						foreach (BaseDataInfo bc in bcc)
						{
							if (Utils.GetTrueForumPath().StartsWith(bc.Forumpath) && bc.Forumpath != "/")
							{
								newBaseConfig = bc;
								break;
							}
							if (("/").Equals(bc.Forumpath))
							{
								rootConfig = bc;
							}
						}
						if (newBaseConfig == null)
						{
							newBaseConfig = rootConfig;
						}
					}

				}
				catch
				{
					newBaseConfig = null;
				}
			}
			if (newBaseConfig == null) 
			{
                throw new Exception("��������: ����Ŀ¼����վ��Ŀ¼��û����ȷ��CTMON.config�ļ�������û�����л�Ȩ��");
			}
			return newBaseConfig;

		}


	}
}
