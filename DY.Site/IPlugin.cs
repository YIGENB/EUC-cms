using System.Web.UI;

namespace DY.Site
{
    /// <summary>
    /// 插件接口
    /// </summary>
    public partial interface IPlugin
    {
        /// <summary>
        /// 获取插件输出的内容
        /// </summary>
        /// <param name="page">System.Web.UI.Page</param>
        /// <returns>字符串</returns>
        void GetOutput(string strPluginName,Page page);

        /// <summary>
        /// 安装插件时执行的代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page</param>
        void Install(Page page);

        /// <summary>
        /// 卸载插件时执行的代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page</param>
        void Uninstall(Page page);

        /// <summary>
        /// 更新插件时执行的代码
        /// </summary>
        /// <param name="page">System.Web.UI.Page</param>
        void Update(Page page);
    }

    /// <summary>
    /// 捕获安装时发生的异常
    /// </summary>
    public class PluginInstallException : System.Exception
    {
        protected string Msg = string.Empty;
        /// <summary>
        /// 捕获安装时发生的异常
        /// </summary>
        /// <param name="msg"></param>
        public PluginInstallException(string msg)
        {
            this.Msg = msg;
        }
        /// <summary>
        /// 捕获安装时发生的异常
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Msg;
        }
    }
}
