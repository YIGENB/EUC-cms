using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.UI;

namespace DY.Site
{
    /// <summary>
    /// ��ɹ�������
    /// 1:֧������
    /// 2:֧���������
    /// 3:��ѯ��������.
    /// 4:��ѯ�����������.
    /// </summary>
    public class tenpay
    {
        /// <summary>
        /// ������ֵ,�ܶ������ֵ�󲢲��ܴ����Ƹ�ͨǰ̨��֤
        /// ����ֻ�Ǹ���һ������.
        /// </summary>
        private string chnid = "335588";
        /// <summary>
        /// ƽ̨�ṩ�ߵĲƸ�ͨ�˺�
        /// </summary>
        /// <value>The chnid.</value>
        public string Chnid
        {
            get { return chnid; }
            set { chnid = value; }
        }

        private int cmdno = 12;
        /// <summary>
        /// �������,�ݶ�ֵ:12
        /// </summary>
        /// <value>The cmdno.</value>
        public int Cmdno
        {
            get { return cmdno; }
            set { cmdno = value; }
        }

        private int encode_type = 1;
        /// <summary>
        /// 1��GB2312���룬Ĭ��ΪGB2312���롣2��UTF-8���롣
        /// </summary>
        /// <value>The encode_type.</value>
        public int Encode_type
        {
            get { return encode_type; }
            set { encode_type = value; }
        }

        private string mch_desc = "���Խ�������";
        /// <summary>
        /// ����˵�������ܰ���&lt; &gt;����%�����ַ�
        /// </summary>
        /// <value>The mch_desc.</value>
        public string Mch_desc
        {
            get { return mch_desc; }
            set { mch_desc = value; }
        }

        private string mch_name = "������Ʒ";
        /// <summary>
        /// ��Ʒ���ƣ����ܰ���&lt; &gt;����%�����ַ�
        /// </summary>
        /// <value>The mch_name.</value>
        public string Mch_name
        {
            get { return mch_name; }
            set { mch_name = value; }
        }

        private int mch_price = 1;
        /// <summary>
        /// ��Ʒ�ܼۣ���λΪ�֡����Ƹ�ͨ���治������ѡ������
        /// </summary>
        /// <value>The mch_price.</value>
        public int Mch_price
        {
            get { return mch_price; }
            set { mch_price = value; }
        }

        private int mch_type = 1;
        /// <summary>
        /// �������ͣ�1��ʵ�ｻ�ף�2�����⽻��
        /// </summary>
        /// <value>The mch_type.</value>
        public int Mch_type
        {
            get { return mch_type; }
            set { mch_type = value; }
        }

        private int need_buyerinfo = 2;
        /// <summary>
        /// �Ƿ���Ҫ�ڲƸ�ͨ�������Ϣ��1����Ҫ��2������Ҫ��
        /// </summary>
        /// <value>The need_buyerinfo.</value>
        public int Need_buyerinfo
        {
            get { return need_buyerinfo; }
            set { need_buyerinfo = value; }
        }

        private string sign = "";
        /// <summary>
        /// ǩ��
        /// </summary>
        /// <value>The sign.</value>
        public string Sign
        {
            get { return sign; }
            set { sign = value; }
        }

        private string seller = "88881491";
        /// <summary>
        /// �տ�Ƹ�ͨ�˺�
        /// </summary>
        /// <value>The seller.</value>
        public string Seller
        {
            get { return seller; }
            set { seller = value; }
        }

        private string transport_desc = "none";
        /// <summary>
        /// ������˾��������ʽ˵��
        /// </summary>
        /// <value>The transport_desc.</value>
        public string Transport_desc
        {
            get { return transport_desc; }
            set { transport_desc = value; }
        }

        private string version = "2";
        /// <summary>
        /// �汾��
        /// </summary>
        /// <value>The version.</value>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private int total_fee = 1;
        /// <summary>
        /// �����ܼ�,��λΪ��
        /// </summary>
        /// <value>The total_fee.</value>
        public int Total_fee
        {
            get { return total_fee; }
            set { total_fee = value; }
        }

        private int trade_price = 1;
        /// <summary>
        /// ��Ʒ�ܼ۸�
        /// </summary>
        /// <value>The trade_price.</value>
        public int Trade_price
        {
            get { return trade_price; }
            set { trade_price = value; }
        }


        private int transport_fee = 0;

        /// <summary>
        ///�����۸�
        /// </summary>
        /// <value>The transport_fee.</value>
        public int Transport_fee
        {
            get { return transport_fee; }
            set { transport_fee = value; }
        }



        //�̻�������,ֻ��Ϊ����
        private string mch_vno = "1122334455";
        /// <summary>
        /// �̼Ҷ����ţ��˲������ڶ���ʱ�ṩ��YYYYMMDDXXXX
        /// </summary>
        /// <value>The mch_vno.</value>
        public string Mch_vno
        {
            get { return mch_vno; }
            set { mch_vno = value; }
        }

        private int retcode = 0;
        /// <summary>
        /// 0,���׳ɹ�������ֵ����ʶ����ʧ�ܵ�״̬��
        /// </summary>
        /// <value>The retcode.</value>
        public int Retcode
        {
            get { return retcode; }
            set { retcode = value; }
        }

        private string status = "3";
        /// <summary>
        /// ����״̬��1���״��� 2�ջ��ַ��д��� 3��Ҹ���ɹ� 4���ҷ����ɹ�
        /// </summary>
        /// <value>The status.</value>
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string buyer_id = "40669760";
        /// <summary>
        /// ��ҲƸ�ͨ�ʺ�
        /// </summary>
        /// <value>The buyer_id.</value>
        public string Buyer_id
        {
            get { return buyer_id; }
            set { buyer_id = value; }
        }

        private string cft_tid = "";
        /// <summary>
        /// �Ƹ�ͨ���׵���
        /// </summary>
        /// <value>The cft_tid.</value>
        public string Cft_tid
        {
            get { return cft_tid; }
            set { cft_tid = value; }
        }

        private string key = "88881491";
        /// <summary>
        /// �̻�KEY���滻Ϊ���ѵ�KEY��
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private string paygateurl = "https://www.tenpay.com/cgi-bin/med/show_opentrans.cgi";/// �Ƹ�֧ͨ������URL

        private string querygateurl = "https://www.tenpay.com/cgi-bin/med/query_opentrans.cgi";/// �Ƹ�ͨ��ѯ����URL

        /// <summary>
        /// ֧���������ҳ��
        /// �Ƽ�ʹ��ip��ַ�ķ�ʽ(�255���ַ�)
        /// ����ʹ����Ե�ַ������,��ʹ��ǰƴװȫ��ַ����.�������㲿��.
        /// </summary>
        private string mch_returl = "";

        /// <summary>
        /// ֧���������ҳ��
        /// </summary>
        public string Mch_returl {
            get { return mch_returl; }
            set { mch_returl = value; }
        }

        /// <summary>
        /// ��ѯ�������ҳ��
        /// �Ƽ�ʹ��ip��ַ�ķ�ʽ(�255���ַ�)
        /// ����ʹ����Ե�ַ������,��ʹ��ǰƴװȫ��ַ����.�������㲿��.
        /// </summary>
        private string show_url = "";

        /// <summary>
        /// ��ѯ�������ҳ��
        /// </summary>
        public string Show_url {
            get { return show_url; }
            set { show_url = value; }
        }

        private const int querycmdno = 2;/// ��ѯ����.2

        private string date;//����

        #region �����ֶ�����,��ʽΪyyyyMMdd

        /// <summary>
        /// ֧������,yyyyMMdd
        /// </summary>
        public string Date
        {
            get
            {
                if (date == null)
                {
                    date = DateTime.Now.ToString("yyyyMMdd");
                }

                return date;
            }
            set
            {
                if (value == null || value.Trim().Length != 8)
                {
                    date = DateTime.Now.ToString("yyyyMMdd");
                }
                else
                {
                    try
                    {
                        string strTmp = value.Trim();
                        date = DateTime.Parse(strTmp.Substring(0, 4) + "-" + strTmp.Substring(4, 2) + "-"
                            + strTmp.Substring(6, 2)).ToString("yyyyMMdd");
                    }
                    catch
                    {
                        date = DateTime.Now.ToString("yyyyMMdd");
                    }

                }
            }
        }

        #endregion

        private string attach = "test11";
        /// <summary>
        /// ָ���ʶ,ÿ��ָ���������ֶ�,�Ƹ�ͨ�ڴ�����ɺ��ԭ������.
        /// </summary>
        public string Attach
        {
            get { return UrlDecode(attach); }
            set { attach = UrlEncode(value); }
        }

        private string payerrmsg = "";
        /// <summary>
        /// ���Ϊ֧��ʧ��ʱ,�Ƹ�ͨ���صĴ�����Ϣ
        /// </summary>
        public string PayErrMsg
        {
            get { return payerrmsg; }
        }

        /// <summary>
        /// ���ַ�������URL����
        /// </summary>
        /// <param name="instr">��������ַ���</param>
        /// <returns>������</returns>
        private static string UrlEncode(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {

                return instr.Replace("%", "%25").Replace("=", "%3d").Replace("&", "%26").
                       Replace("\"", "%22").Replace("?", "%3f").Replace("'", "%27").Replace(" ", "%20");
            }
        }

        /// <summary>
        /// ���ַ�������URL����
        /// </summary>
        /// <param name="instr">��������ַ���</param>
        /// <returns>������</returns>
        private static string UrlDecode(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                return instr.Replace("%3d", "=").Replace("%26", "&").Replace("%22", "\"").Replace("%3f", "?")
                    .Replace("%27", "'").Replace("%20", " ").Replace("%25", "%");
            }
        }

        /// <summary>
        /// ��ȡ��д��MD5ǩ�����
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        private static string GetMD5(string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //����md5����
            byte[] inputBye;
            byte[] outputBye;

            //ʹ��GB2312���뷽ʽ���ַ���ת��Ϊ�ֽ����飮
            inputBye = Encoding.GetEncoding("gb2312").GetBytes(encypStr);

            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        public tenpay()
        {
            //payment = (DY_Payment)(new config().bll_payment.GetEntity(DY_Payment._.pay_code == "TenPay"));

            //if (payment != null)
            //{
            //    this.Seller = (payment.pay_config + ",,").Split(',')[0];
            //    this.Key = (payment.pay_config + ",,").Split(',')[1];
            //}
        }

        /// <summary>
        /// ��Ӳ���,�ǲ���ֵ��Ϊ�մ�,����ӡ���֮,����ӡ�
        /// </summary>
        protected StringBuilder AddParameter(StringBuilder buf,
            String parameterName,
            String parameterValue)
        {

            if (null == parameterValue || "".Equals(parameterValue))
            {
                return buf;
            }

            if ("".Equals(buf.ToString()))
            {
                buf.Append(parameterName);
                buf.Append("=");
                buf.Append(parameterValue);
            }
            else
            {
                buf.Append("&");
                buf.Append(parameterName);
                buf.Append("=");
                buf.Append(parameterValue);
            }
            return buf;
        }

        /// <summary>
        /// ��ȡ֧��ǩ��
        /// </summary>
        /// <returns>���ݲ����õ�ǩ��</returns>
        public string GetPaySign()
        {
            StringBuilder buf = new StringBuilder();
            AddParameter(buf, "attach", attach);
            AddParameter(buf, "chnid", chnid);
            AddParameter(buf, "cmdno", (cmdno).ToString());
            AddParameter(buf, "encode_type", (encode_type).ToString());
            AddParameter(buf, "mch_desc", mch_desc);
            AddParameter(buf, "mch_name", mch_name);
            AddParameter(buf, "mch_price", (mch_price).ToString());
            AddParameter(buf, "mch_returl", mch_returl);
            AddParameter(buf, "mch_type", (mch_type).ToString());
            AddParameter(buf, "mch_vno", mch_vno);
            AddParameter(buf, "need_buyerinfo", (need_buyerinfo).ToString());
            AddParameter(buf, "seller", seller);
            AddParameter(buf, "show_url", show_url);
            AddParameter(buf, "transport_desc", transport_desc);
            AddParameter(buf, "transport_fee", (transport_fee).ToString());
            AddParameter(buf, "version", version);
            AddParameter(buf, "key", key);

            return GetMD5(buf.ToString());
        }

        /// <summary>
        /// ��ȡ֧�����ǩ��
        /// </summary>
        /// <returns>���ݲ����õ�ǩ��</returns>
        public string GetPayResultSign()
        {
            StringBuilder buf = new StringBuilder();
            AddParameter(buf, "attach", attach);
            AddParameter(buf, "buyer_id", buyer_id);
            AddParameter(buf, "cft_tid", (cft_tid).ToString());
            AddParameter(buf, "chnid", (chnid).ToString());
            AddParameter(buf, "cmdno", (cmdno).ToString());
            AddParameter(buf, "mch_vno", mch_vno);
            AddParameter(buf, "retcode", (retcode).ToString());
            AddParameter(buf, "seller", seller);
            AddParameter(buf, "status", (status).ToString());
            AddParameter(buf, "total_fee", (total_fee).ToString());
            AddParameter(buf, "trade_price", (trade_price).ToString());
            AddParameter(buf, "transport_fee", (transport_fee).ToString());
            AddParameter(buf, "version", version);
            AddParameter(buf, "key", key);

            return GetMD5(buf.ToString());
        }

        /// <summary>
        /// ��ȡ֧��ҳ��URL
        /// </summary>
        /// <param name="url">�������������,��֧��URL,����������ؼ�,�Ǵ�����Ϣ</param>
        /// <returns>����ִ���Ƿ�ɹ�</returns>
        public bool GetPayUrl(out string url)
        {
            try
            {
                string sign = GetPaySign();

                url = paygateurl + "?attach=" + attach + "&chnid=" + chnid + "&cmdno=" + cmdno + "&encode_type=" + encode_type + "&mch_desc=" + mch_desc
                + "&mch_name=" + mch_name + "&mch_price=" + mch_price + "&mch_returl="
                + mch_returl + "&mch_type=" + mch_type + "&mch_vno=" + mch_vno + "&need_buyerinfo=" + need_buyerinfo + "&seller=" + seller
                + "&show_url=" + show_url + "&transport_desc=" + transport_desc + "&transport_fee=" + transport_fee + "&version=" + version + "&sign=" + sign;

                return true;
            }
            catch (Exception err)
            {
                url = "����URLʱ����,������Ϣ:" + err.Message;
                return false;
            }
        }

        /// <summary>
        /// ��֧�����ҳ���URL��������л�ȡ�����Ϣ
        /// </summary>
        /// <param name="querystring">֧�����ҳ���URL�������</param>
        /// <param name="errmsg">����ִ�в��ɹ��Ļ�,���ش�����Ϣ</param>
        /// <returns>����ִ���Ƿ�ɹ�</returns>
        public bool GetPayValueFromUrl(NameValueCollection querystring, out string errmsg)
        {

            //���URL������������
            /*
            ?cmdno=1&pay_result=0&pay_info=OK&date=20070423&seller=1201143001&transaction_id=1201143001200704230000000013
            &sp_billno=13&mch_price=1&fee_type=1&attach=%D5%E2%CA%C7%D2%BB%B8%F6%B2%E2%CA%D4%BD%BB%D2%D7%B5%A5				
            &sign=ADD7475F2CAFA793A3FB35051869E301
            */
            #region ���в���У��

            if (querystring == null || querystring.Count == 0)
            {
                errmsg = "����Ϊ��";
                return false;
            }

            if (querystring["cmdno"] == null || querystring["cmdno"].ToString().Trim() != cmdno.ToString())
            {
                errmsg = "û��cmdno������cmdno��������ȷ";
                return false;
            }


            if (querystring["seller"] == null)
            {
                errmsg = "û��seller����";
                return false;
            }

            if (querystring["buyer_id"] == null)
            {
                errmsg = "û��buyer_id����";
                return false;
            }

            if (querystring["cft_tid"] == null)
            {
                errmsg = "û��cft_tid����";
                return false;
            }

            if (querystring["mch_vno"] == null)
            {
                errmsg = "û��mch_vno����";
                return false;
            }

            if (querystring["retcode"] == null)
            {
                errmsg = "û��retcode����";
                return false;
            }

            if (querystring["status"] == null)
            {
                errmsg = "û��status����";
                return false;
            }

            if (querystring["total_fee"] == null)
            {
                errmsg = "û��total_fee����";
                return false;
            }

            if (querystring["attach"] == null)
            {
                errmsg = "û��attach����";
                return false;
            }

            if (querystring["trade_price"] == null)
            {
                errmsg = "û��trade_price����";
                return false;
            }

            if (querystring["transport_fee"] == null)
            {
                errmsg = "û��transport_fee����";
                return false;
            }

            //			if(querystring["version"] == null)
            //			{
            //				errmsg = "û��version����";
            //				return false;
            //			}

            if (querystring["chnid"] == null)
            {
                errmsg = "û��chnid����";
                return false;
            }
            if (querystring["sign"] == null)
            {
                errmsg = "û��sign����";
                return false;
            }

            #endregion
            errmsg = "";
            try
            {
                attach = querystring["attach"];
                buyer_id = querystring["buyer_id"];
                cft_tid = querystring["cft_tid"];
                chnid = querystring["chnid"];
                cmdno = Int32.Parse(querystring["cmdno"]);
                mch_vno = querystring["mch_vno"];
                retcode = Int32.Parse(querystring["retcode"]);
                seller = querystring["seller"];
                status = querystring["status"];
                total_fee = Int32.Parse(querystring["total_fee"]);
                trade_price = Int32.Parse(querystring["trade_price"]);
                transport_fee = Int32.Parse(querystring["transport_fee"]);
                version = querystring["version"];

                string strsign = querystring["sign"];
                string sign = GetPayResultSign();


                if (sign != strsign)//��֤ǩ��
                {
                    errmsg = "��֤ǩ��ʧ��";
                    return false;
                }

                return true;
            }
            catch (Exception err)
            {
                errmsg = "������������:" + err.Message;
                return false;
            }
        }
    }
}
