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
        /// ���������ʾ
        /// </summary>
        /// <param name="message">��ʾ��Ϣ</param>
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
