using System;
using System.Text;

namespace DY.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DbException : Exception
    {
        /// <summary>
        /// 输出错误提示
        /// </summary>
        /// <param name="message">提示信息</param>
        public DbException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public int Number
        {
            get { return 0 ; }
        }

       
    }
}
