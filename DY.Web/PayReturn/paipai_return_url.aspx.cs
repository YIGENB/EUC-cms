using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using tenpay;
using System.Collections.Specialized;

namespace DY.Web.PayReturn
{
	/// <summary>
	/// return_url ��ժҪ˵����
	/// </summary>
    public partial class PaiPai_return_url : DY.Site.WebPage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            string title = "";
            string content = "";

            DY.Entity.PaymentInfo payinfo = DY.Site.SiteBLL.GetPaymentInfo("pay_code='tenpay'");
            if (payinfo == null)
            {
                Response.Write("������ʾ��֧�������ò���ȷ��");
                return;
            }

			//��Կ
            string key = (payinfo.pay_config + ",,").Split(',')[1];

			//����PayResponseHandlerʵ��
			MediPayResponse resHandler = new MediPayResponse(Context);

			resHandler.setKey(key);

			//�ж�ǩ��
			if(resHandler.isTenpaySign()) 
			{
	
				//֧�����
				string retcode = resHandler.getParameter("retcode");

				//֧��״̬
				string status = resHandler.getParameter("status");

				//�̻�������
				string mch_vno = resHandler.getParameter("mch_vno");

                string trade_price = resHandler.getParameter("trade_price");

				if("0".Equals(retcode)) 
				{
					//Response.Write("status:" + status + "<br/>");

					//------------------------------
					//����ҵ��ʼ
					//------------------------------ 
		
					//��״̬�������ҵ���߼�
					switch(Int32.Parse(status)) 
					{
						case 1: 
							//���״���
							break;
						case 2:
							//�ջ��ַ��д���
							break;
						case 3:
							//��Ҹ���ɹ���ע���ж϶����Ƿ��ظ����߼�
                            DY.Entity.OrderInfoInfo orderinfo = DY.Site.SiteBLL.GetOrderInfoInfo("order_sn='" + mch_vno + "'");
                            if (orderinfo == null)
                            {
                                title = "�ܱ�Ǹ������ʧ�ܣ�";
                                
                                content = "����ύ����ʧ��,�������ǵĿͻ���ϵ��";
                            }
                            else
                            {
                                orderinfo.pay_status = 1;
                                orderinfo.pay_time = DateTime.Now;
                                //�������ݿ⸶��״̬
                                DY.Site.SiteBLL.UpdateOrderInfoInfo(orderinfo);
                                //���»�Ա����
                                user.UpdateUserIntegral(mch_vno);


                                title = "��ϲ��������ɹ���";
                                content += "�� �� �ţ�" + mch_vno + "</br>";
                                content += "�ɹ�֧����" + trade_price;
                            }
							break;
						case 4:
							//���ҷ����ɹ�
							break;
						case 5:
							//����ջ�ȷ�ϣ����׳ɹ�
							break;
						case 6:
							//���׹رգ�δ��ɳ�ʱ�ر�
							break;
						case 7:
							//�޸Ľ��׼۸�ɹ�
							break;
						case 8:
							//��ҷ����˿�
							break;
						case 9:
							//�˿�ɹ�
							break;
						case 10:
							//�˿�ر�
							break;
						default:
							//error	
							break;
					}	
					//����doShow, ��ӡmetaֵ��js����,���߲Ƹ�ͨ����ɹ�,�����û��������ʾ$showҳ��.
					//resHandler.doShow();

				} 
				else 
				{
					//�������ɹ�����
                    title = "֧��ʧ��";  // Response.Write("֧��ʧ��");
				}
	
			} 
			else 
			{
                title = "��֤ǩ��ʧ��";  // Response.Write("��֤ǩ��ʧ��");
				
			}

			//string debugInfo = resHandler.getDebugInfo();
			//Response.Write("<br/>debugInfo:" + debugInfo);
            IDictionary context = new Hashtable();
            context.Add("pay_title", title);
            context.Add("pay_content", content);

            base.DisplayTemplate(context, "store/payreturn");
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}

    /// <summary>
    /// ResponseHandler ��ժҪ˵����
    /// </summary>
    public class ResponseHandler
    {
        /** ��Կ */
        private string key;

        /** Ӧ��Ĳ��� */
        protected Hashtable parameters;

        /** debug��Ϣ */
        private string debugInfo;

        protected HttpContext httpContext;

        //��ȡ������֪ͨ���ݷ�ʽ�����в�����ȡ
        public ResponseHandler(HttpContext httpContext)
        {
            parameters = new Hashtable();

            this.httpContext = httpContext;
            NameValueCollection collection;
            if (this.httpContext.Request.HttpMethod == "POST")
            {
                collection = this.httpContext.Request.Form;
            }
            else
            {
                collection = this.httpContext.Request.QueryString;
            }

            foreach (string k in collection)
            {
                string v = (string)collection[k];
                this.setParameter(k, v);
            }
        }

        /** ��ȡ��Կ */
        public string getKey()
        { return key; }

        /** ������Կ */
        public void setKey(string key)
        { this.key = key; }

        /** ��ȡ����ֵ */
        public string getParameter(string parameter)
        {
            string s = (string)parameters[parameter];
            return (null == s) ? "" : s;
        }

        /** ���ò���ֵ */
        public void setParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                if (parameters.Contains(parameter))
                {
                    parameters.Remove(parameter);
                }

                parameters.Add(parameter, parameterValue);
            }
        }

        /** �Ƿ�Ƹ�ͨǩ��,������:����������a-z����,������ֵ�Ĳ������μ�ǩ���� 
         * @return boolean */
        public virtual Boolean isTenpaySign()
        {
            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList(parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + this.getKey());
            string sign = MD5Util.GetMD5(sb.ToString(), getCharset()).ToLower();

            //debug��Ϣ
            this.setDebugInfo(sb.ToString() + " => sign:" + sign);
            return getParameter("sign").ToLower().Equals(sign);
        }

        /**
        * ��ʾ��������
        * @param show_url ��ʾ��������url��ַ,����url��ַ����ʽ(http://www.xxx.com/xxx.aspx)��
        * @throws IOException 
        */
        public void doShow(string show_url)
        {
            string strHtml = "<html><head>\r\n" +
                "<meta name=\"TENCENT_ONLINE_PAYMENT\" content=\"China TENCENT\">\r\n" +
                "<script language=\"javascript\">\r\n" +
                "window.location.href='" + show_url + "';\r\n" +
                "</script>\r\n" +
                "</head><body></body></html>";

            this.httpContext.Response.Write(strHtml);

            this.httpContext.Response.End();
        }

        /** ��ȡdebug��Ϣ */
        public string getDebugInfo()
        { return debugInfo; }

        /** ����debug��Ϣ */
        protected void setDebugInfo(String debugInfo)
        { this.debugInfo = debugInfo; }

        protected virtual string getCharset()
        {
            return this.httpContext.Request.ContentEncoding.BodyName;

        }

        /** �Ƿ�Ƹ�ͨǩ��,������:����������a-z����,������ֵ�Ĳ������μ�ǩ���� 
         * @return boolean */
        public virtual Boolean _isTenpaySign(ArrayList akeys)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string k in akeys)
            {
                string v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + this.getKey());
            string sign = MD5Util.GetMD5(sb.ToString(), getCharset()).ToLower();

            //debug��Ϣ
            this.setDebugInfo(sb.ToString() + " => sign:" + sign);
            return getParameter("sign").ToLower().Equals(sign);
        }
    }

    public class MediPayResponse : ResponseHandler
    {
        public MediPayResponse(HttpContext httpContext)
            : base(httpContext)
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        /**
            * �Ƿ�Ƹ�ͨǩ��
            * @Override
            * @return boolean
        */

        public override Boolean isTenpaySign()
        {



            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList();
            akeys.Add("attach");
            akeys.Add("buyer_id");
            akeys.Add("cft_tid");
            akeys.Add("chnid");
            akeys.Add("cmdno");
            akeys.Add("mch_vno");
            akeys.Add("retcode");
            akeys.Add("seller");
            akeys.Add("status");
            akeys.Add("total_fee");
            akeys.Add("trade_price");
            akeys.Add("transport_fee");
            akeys.Add("version");
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + this.getKey());
            string sign = MD5Util.GetMD5(sb.ToString(), getCharset());

            //debug��Ϣ
            this.setDebugInfo(sb.ToString() + " => sign:" + sign);
            return getParameter("sign").Equals(sign);

        }


        public void doShow()
        {
            string strHtml = "<html><head>\r\n" +
                "<meta name=\"TENCENT_ONLINE_PAYMENT\" content=\"China TENCENT\">\r\n" +
                "</head><body></body></html>";

            this.httpContext.Response.Write(strHtml);

            this.httpContext.Response.End();
        }
    }
}
