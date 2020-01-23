using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
namespace DY.Site
{
    public class EnumUtils
    {
        public enum Module:byte
        {
            /// <summary>
            /// 长尾词
            /// </summary>
            [Description("长尾词")]
            NagaoWord=1,
            /// <summary>
            /// 指定关键词
            /// </summary>
            [Description("指定关键词")]
            SpecifiedWord=9,
            /// <summary>
            /// 新词
            /// </summary>
            [Description("新词")]
            NewWord=3,
            /// <summary>
            /// 热词
            /// </summary>
            [Description("热词")]
            HotWord=4,
            /// <summary>
            /// 热词预测
            /// </summary>
            [Description("热词预测")]
            HotTreadWord=5,
            /// <summary>
            /// 网站词库
            /// </summary>
            [Description("网站词库")]
            SiteWord=2
        }

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumObj)
        {
            System.Reflection.FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);
            if (attribArray.Length == 0)
            {
                return String.Empty;
            }
            else
            {
                DescriptionAttribute des = (DescriptionAttribute)attribArray[0];

                return des.Description;
            }
        }
    }
}
