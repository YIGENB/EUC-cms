/////////////////////////////////////////////////////////////////////////////////
//
//  File:           Payment_success.aspx.cs
//
//  Facility:       The unit contains the Payment_success class
//
//  Abstract:       This class is intended for processing of requests that come
//                  from the PayPal server during testing. Besides, this class
//                  creates payment reports and records the process of interaction
//                  with the PayPal server.
//
//  Environment:    VC 8.0
//
//  Author:         KB_Soft Group Ltd.
//
/////////////////////////////////////////////////////////////////////////////////

using KBSoft;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Xml;

public partial class Payment_success : Page, IRequiresSessionState
{
    private string business;
    private string currency_code;
    private static DataSet requests = new DataSet();
    private static DataSet responses = new DataSet();

    public Payment_success()
    {
        base.Load += new EventHandler(this.Page_Load);
        this.business = ConfigurationManager.AppSettings["BusinessEmail"];
        this.currency_code = ConfigurationManager.AppSettings["CurrencyCode"];
    }

    /// <summary>    
    /// creating a record about the response to the payment/ request Paypal成功返回页面
    /// </summary>
    /// <param name="txn_id">A unique transaction number 一个独特的交易数</param>
    /// <param name="payment_price">The total cost of the cart 该车的总成本</param>
    /// <param name="email">buyer's email 买方的电子邮件</param>
    /// <param name="first_name">buyer's name 买方的名称</param>
    /// <param name="last_name">buyer's last name 买家的姓名</param>
    /// <param name="street">buyer's street 买方的街</param>
    /// <param name="city">buyer's city 买方的市</param>
    /// <param name="state">buyer's state 买方的状态</param>
    /// <param name="zip">buyer's ZIP 买方的拉链</param>
    /// <param name="country">buyer's country 买方国家</param>
    /// <param name="request_id">an identifier of the payment request 付款请求标识符</param>
    /// <param name="is_success">a flag indicating whether the payment was successfully performed 指示付款成功标志</param>
    /// <param name="reason_fault">a possible reason of the payment failure 对支付失败的可能原因</param>
    public void CreatePaymentResponses(string txn_id, decimal payment_price, string email, string first_name, string last_name, string street, string city, string state, string zip, string country, int request_id, bool is_success, string reason_fault)
    {
        try
        {
            int payment_id;
            XmlTextReader reader;
            CultureInfo provider = new CultureInfo("en-us");
            string path = this.Server.MapPath("~/App_Data/PaymentResponses.xml");
            XmlDocument document = new XmlDocument();
            if (System.IO.File.Exists(path))
            {
                reader = new XmlTextReader(path);
                reader.Read();
            }
            else
            {
                Carts.CreateXml(path, "Responses");
                reader = new XmlTextReader(path);
                reader.Read();
            }
            document.Load(reader);
            reader.Close();

            // getting a unique identifier of the payment_id payment得到的payment_id支付的唯一标识符
            XmlNodeList nodes = document.GetElementsByTagName("Response");
            if (nodes.Count != 0)
            {
                payment_id = Carts.GetIdentity(nodes, "payment_id");
            }
            else
            {
                payment_id = 0;
            }

            // creating a new element containing information about the payment
            XmlElement newChild = document.CreateElement("Response");
            newChild.SetAttribute("payment_id", payment_id.ToString());
            newChild.SetAttribute("txn_id", txn_id);
            newChild.SetAttribute("payment_date", DateTime.Now.ToString(provider));
            newChild.SetAttribute("payment_price", payment_price.ToString(provider));
            newChild.SetAttribute("email", email);
            newChild.SetAttribute("first_name", first_name);
            newChild.SetAttribute("last_name", last_name);
            newChild.SetAttribute("street", street);
            newChild.SetAttribute("city", city);
            newChild.SetAttribute("state", state);
            newChild.SetAttribute("zip", zip);
            newChild.SetAttribute("country", country);
            newChild.SetAttribute("request_id", request_id.ToString());
            newChild.SetAttribute("is_success", is_success.ToString());
            newChild.SetAttribute("reason_fault", reason_fault);
            document.DocumentElement.AppendChild(newChild);
            document.Save(path);
        }
        catch (Exception exception)
        {
            Carts.WriteFile("Error in payment_success.CreatePaymentResponses(): " + exception.Message);
        }
    }

    /// <summary>    
    /// getting a cart identifier for the identifier of the current request_id request
    /// </summary>
    /// <param name="request_id">the request identifier</param>
    /// <returns>the identifier of the cart for the current request</returns>
    public static string GetIDCart(string request_id)
    {
        try
        {
            string filterExpression = "request_id = '" + request_id + "'";
            DataRow[] rowArray = requests.Tables[0].Select(filterExpression);
            if (rowArray.Length == 1)
            {
                return rowArray[0]["cart_id"].ToString();
            }
        }
        catch (Exception exception)
        {
            Carts.WriteFile("Error in payment_success.GetIDCart(): " + exception.Message);
        }
        return "";
    }

    /// <summary>    
    /// getting the total cost for an identifier of the current request_id request
    /// </summary>
    /// <param name="request_id">the request identifier</param>
    /// <returns>the total cost of the identifier</returns>
    public static string GetRequestPrice(string request_id)
    {
        try
        {
            string filterExpression = "request_id = '" + request_id + "'";
            DataRow[] rowArray = requests.Tables[0].Select(filterExpression);
            if (rowArray.Length == 1)
            {
                return rowArray[0]["price"].ToString();
            }
        }
        catch (Exception exception)
        {
            Carts.WriteFile("Error in IPNHandler.GetRequestPrice(): " + exception.Message);
        }
        return "";
    }

    /// <summary>
    /// checking whether the current request is duplicated for the unique number of the txn_id transaction
    /// </summary>
    /// <param name="txn_id">the unique transaction number</param>
    /// <returns>true if the current request has already been processed</returns>
    public bool IsDuplicateID(string txn_id)
    {
        try
        {
            string filterExpression = "txn_id = '" + txn_id + "'";
            if (responses.Tables[0].Select(filterExpression).Length == 0)
            {
                return false;
            }
            return true;
        }
        catch (Exception exception)
        {
            Carts.WriteFile("Error in payment_success.IsDuplicateID(): " + exception.Message);
            return false;
        }

    }

    private void Page_Load(object sender, EventArgs e)
    {
        string requestUriString;
        CultureInfo provider = new CultureInfo("en-us");
        string requestsFile = this.Server.MapPath("~/App_Data/PaymentRequests.xml");
        requests.Clear();
        if (System.IO.File.Exists(requestsFile))
        {
            requests.ReadXml(requestsFile);
        }
        else
        {
            Carts.CreateXml(requestsFile, "Requests");
            requests.ReadXml(requestsFile);
        }
        string responseFile = this.Server.MapPath("~/App_Data/PaymentResponses.xml");
        responses.Clear();
        if (System.IO.File.Exists(responseFile))
        {
            responses.ReadXml(responseFile);
        }
        else
        {
            Carts.CreateXml(responseFile, "Responses");
            responses.ReadXml(responseFile);
        }
        string strFormValues = Encoding.ASCII.GetString(this.Request.BinaryRead(this.Request.ContentLength));

        // getting the URL to work with
        if (String.Compare(ConfigurationManager.AppSettings["UseSandbox"].ToString(), "true", false) == 0)
        {
            requestUriString = "https://www.sandbox.paypal.com/cgi-bin/webscr";
        }
        else
        {
            requestUriString = "https://www.paypal.com/cgi-bin/webscr";
        }

        // Create the request back
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);

        // Set values for the request back
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        string obj2 = strFormValues + "&cmd=_notify-validate";
        request.ContentLength = obj2.Length;

        // Write the request back IPN strings
        StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
        writer.Write(RuntimeHelpers.GetObjectValue(obj2));
        writer.Close();

        //send the request, read the response
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream responseStream = response.GetResponseStream();
        Encoding encoding = Encoding.GetEncoding("utf-8");
        StreamReader reader = new StreamReader(responseStream, encoding);

        // Reads 256 characters at a time.
        char[] buffer = new char[0x101];
        int length = reader.Read(buffer, 0, 0x100);
        while (length > 0)
        {
            // Dumps the 256 characters to a string
            string requestPrice;
            string IPNResponse = new string(buffer, 0, length);
            length = reader.Read(buffer, 0, 0x100);
            try
            {
                // getting the total cost of the goods in cart for an identifier of the request stored 
                // in the "custom" variable
                requestPrice = GetRequestPrice(this.Request["custom"].ToString());
                if (String.Compare(requestPrice, "", false) == 0)
                {
                    Carts.WriteFile("Error in payment_success: amount = \"");
                    reader.Close();
                    response.Close();
                    return;
                }
            }
            catch (Exception exception)
            {
                Carts.WriteFile("Error in payment_success: " + exception.Message);
                reader.Close();
                response.Close();
                return;
            }

            NumberFormatInfo info2 = new NumberFormatInfo();
            info2.NumberDecimalSeparator = ".";
            info2.NumberGroupSeparator = ",";
            info2.NumberGroupSizes = new int[] { 3 };

            // if the request is verified
            if (String.Compare(IPNResponse, "VERIFIED", false) == 0)
            {
                // check the receiver's e-mail (login is user's identifier in PayPal) and the transaction type
                if ((String.Compare(this.Request["receiver_email"], this.business, false) != 0) || (String.Compare(this.Request["txn_type"], "web_accept", false) != 0))
                {
                    try
                    {
                        // parameters are not correct. Write a response from PayPal and create a record in the Log file.
                        this.CreatePaymentResponses(this.Request["txn_id"], Convert.ToDecimal(this.Request["mc_gross"], info2), this.Request["payer_email"], this.Request["first_name"], this.Request["last_name"], this.Request["address_street"], this.Request["address_city"], this.Request["address_state"], this.Request["address_zip"], this.Request["address_country"], Convert.ToInt32(this.Request["custom"]), false, "INVALID paymetn's parameters (receiver_email or txn_type)");
                        Carts.WriteFile("Error in payment_success: INVALID paymetn's parameters (receiver_email or txn_type)");
                    }
                    catch (Exception exception)
                    {
                        Carts.WriteFile("Error in payment_success: " + exception.Message);
                    }
                    reader.Close();
                    response.Close();
                    return;
                }

                // check whether this request was performed earlier for its identifier
                if (this.IsDuplicateID(this.Request["txn_id"]))
                {
                    // the current request is processed. Write a response from PayPal and create a record in the Log file.
                    this.CreatePaymentResponses(this.Request["txn_id"], Convert.ToDecimal(this.Request["mc_gross"], info2), this.Request["payer_email"], this.Request["first_name"], this.Request["last_name"], this.Request["address_street"], this.Request["address_city"], this.Request["address_state"], this.Request["address_zip"], this.Request["address_country"], Convert.ToInt32(this.Request["custom"]), false, "Duplicate txn_id found");
                    Carts.WriteFile("Error in payment_success: Duplicate txn_id found");
                    reader.Close();
                    response.Close();
                    return;
                }

                // the amount of payment, the status of the payment, amd a possible reason of delay
                // The fact that Getting txn_type=web_accept or txn_type=subscr_payment are got odes not mean that
                // seller will receive the payment.
                // That's why we check payment_status=completed. The single exception is when the seller's account in
                // not American and pending_reason=intl
                if (((String.Compare(this.Request["mc_gross"].ToString(provider), requestPrice, false) != 0) || (String.Compare(this.Request["mc_currency"], this.currency_code, false) != 0)) || ((String.Compare(this.Request["payment_status"], "Completed", false) != 0) && (String.Compare(this.Request["pending_reason"], "intl", false) != 0)))
                {
                    // parameters are incorrect or the payment was delayed. A response from PayPal should not be
                    // written to DB of an XML file
                    // because it may lead to a failure of uniqueness check of the request identifier.
                    // Create a record in the Log file with information about the request.
                    Carts.WriteFile("Error in payment_success: INVALID paymetn's parameters. Request: " + strFormValues);
                    reader.Close();
                    response.Close();
                    return;
                }
                try
                {
                    // write a response from PayPal
                    this.CreatePaymentResponses(this.Request["txn_id"], Convert.ToDecimal(this.Request["mc_gross"], info2), this.Request["payer_email"], this.Request["first_name"], this.Request["last_name"], this.Request["address_street"], this.Request["address_city"], this.Request["address_state"], this.Request["address_zip"], this.Request["address_country"], Convert.ToInt32(this.Request["custom"]), true, "");
                    Carts.WriteFile("Success in payment_success: PaymentResponses created");
                    //这里是成功返回
                    ///////////////////////////////////////////////////////////////////////////////////////////
                    // Here we notify the person responsible for goods delivery that 
                    // the payment was performed and providing him with all needed information about
                    // the payment. Some flags informing that user paid for a services can be also set here.
                    // For example, if user paid for registration on the site, then the flag should be set 
                    // allowing the user who paid to access the site
                    ///////////////////////////////////////////////////////////////////////////////////////////
                }
                catch (Exception exception)
                {
                    Carts.WriteFile("Error in payment_success: " + exception.Message);
                }
            }
            else
            {
                Carts.WriteFile("Error in payment_success. IPNResponse = 'INVALID'");
            }
        }
        reader.Close();
        response.Close();
    }
}

