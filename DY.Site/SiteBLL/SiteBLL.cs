using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace DY.Site
{
    public partial class SiteBLL
    {
        /// <summary>
        /// 读取数据并填充到实体
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entity"></param>
        private static void ReaderToEntity(IDataRecord reader, Object entity)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                System.Reflection.PropertyInfo propertyInfo = entity.GetType().GetProperty(reader.GetName(i));
                if (propertyInfo != null)
                {
                    if (reader.GetValue(i) != DBNull.Value)
                    {
                        propertyInfo.SetValue(entity, reader.GetValue(i), null);
                    }
                }
            }
        }
    }
}