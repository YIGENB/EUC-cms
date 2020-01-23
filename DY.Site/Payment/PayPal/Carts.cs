/////////////////////////////////////////////////////////////////////////////////
//
//  File:           Carts.cs
//
//  Facility:       The unit contains the Carts class
//
//  Abstract:       This class is responsible for work with carts and provides
//                  additional functions that are used in the entire site.
//
//  Environment:    VC 8.0
//
//  Author:         KB_Soft Group Ltd.
//
/////////////////////////////////////////////////////////////////////////////////



namespace KBSoft
{
    using System;
    using System.Web;
    using System.Data;
    using System.Xml;
    using System.IO;
    using System.Globalization;

 public class Carts
    {
        private static DataSet carts = new DataSet();
        private static DataSet goods = new DataSet();

        /// <summary>        
        /// crating different XML files using a file name and a root element
        /// </summary>
        /// <param name="xmlFile">the file name</param>
        /// <param name="element">a name of the root element</param>
        public static void CreateXml(string xmlFile, string element)
        {
            XmlDocument document = new XmlDocument();
            string xmlData = "<" + element + "></" + element + ">";
            document.Load(new StringReader(xmlData));
            if (String.Compare(element, "Goods", false) == 0)
            {
                XmlElement newChild = document.CreateElement("Good");
                newChild.SetAttribute("id", "0");
                newChild.SetAttribute("name", "Sample of good");
                newChild.SetAttribute("price", "10.99");
                document.DocumentElement.AppendChild(newChild);
            }
            try
            {
                document.Save(xmlFile);
            }
            catch (Exception exception)
            {
                WriteFile("Error in Carts.CreateXml(): " + exception.Message);
            }
        }

        /// <summary>
        /// removing an item with rec_id from cart
        /// </summary>
        /// <param name="rec_id">the record identifier</param>
        public static void Delete(int rec_id)
        {
            try
            {
                string fileName = HttpContext.Current.Server.MapPath("~/App_Data/Carts.xml");

                //An expression for searching for an item for its record identifier in the Carts.xml file
                string filterExpression = "rec_id = '" + rec_id.ToString() + "'";
                DataRow[] rowArray = carts.Tables[0].Select(filterExpression);
                if (rowArray.Length == 1)
                {
                    rowArray[0].Delete();
                    carts.WriteXml(fileName);
                }
            }
            catch (Exception exception)
            {
                WriteFile("Error in Carts.Delete(): " + exception.Message);
                HttpContext.Current.Response.Redirect("~/Default.aspx");
            }
        }

        /// <summary>        
        /// getting a unique identification number
        /// </summary>
        /// <param name="nodes">a list of existing nodes</param>
        /// <param name="columnName">a name of the column where the unique identifier is searched</param>
        /// <returns>the unque record identifier for the specified column</returns>
        /// <remarks></remarks>
        public static int GetIdentity(XmlNodeList nodes, string columnName)
        {
            try
            {
                int max_rec = 0;
                foreach (XmlNode node in nodes)
                {
                    int currentRec = int.Parse(node.Attributes[columnName].InnerText);
                    if (currentRec > max_rec)
                    {
                        max_rec = currentRec;
                    }
                }
                return max_rec + 1;
            }
            catch (Exception exception)
            {
                WriteFile("Error in Carts.GetIdentity(): " + exception.Message);
                return 0;
            }
        }
        /// <summary>
        /// Loads the full list of goods from an XML file into DataSet
        /// </summary>
        /// <returns>The DataSet that contains the list of goods.</returns>
        public static DataSet LoadAllGoods()
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Goods.xml");
            goods.Clear();
            if (File.Exists(path))
            {
                goods.ReadXml(path);
            }
            else
            {
                CreateXml(path, "Goods");
                goods.ReadXml(path);
            }
            return goods;
        }

        /// <summary>
        /// Loads the list of goods in a cart with a definite cart_id from an XML file into DataSet
        /// </summary>
        /// <param name="cart_id">the cart idetntifier</param>
        /// <returns>The DataSet with the list of goods in the cart with the cart_id.</returns>
        public static DataSet LoadCart(int cart_id)
        {
            //DataSet set2;
            DataSet currentCart = new DataSet();
            string cartsFile = HttpContext.Current.Server.MapPath("~/App_Data/Carts.xml");
            carts.Clear();
            if (File.Exists(cartsFile))
            {
                carts.ReadXml(cartsFile);
            }
            else
            {
                CreateXml(cartsFile, "Carts");
                carts.ReadXml(cartsFile);
            }
            string goodsFile = HttpContext.Current.Server.MapPath("~/App_Data/Goods.xml");
            goods.Clear();
            if (File.Exists(goodsFile))
            {
                goods.ReadXml(goodsFile);
            }
            else
            {
                CreateXml(goodsFile, "Goods");
                goods.ReadXml(goodsFile);
            }
            currentCart = carts.Copy();
            string filterExpression = "cart_id <> '" + cart_id.ToString() + "'";
            if (currentCart.Tables.Count == 0)
            {
                WriteFile("Error in Carts.LoadCart(): currentCart.Tables.Count = 0");
                HttpContext.Current.Response.Redirect("~/Default.aspx");
            }
            if (currentCart.Tables[0].Rows.Count == 0)
            {
                WriteFile("Error in Carts.LoadCart(): currentCart.Rows.Count = 0");
                HttpContext.Current.Response.Redirect("~/Default.aspx");
            }
            try
            {
                //remove all goods from the current cart with the cart_id that does not coincide with the specified one
                DataRow[] rowArray = currentCart.Tables[0].Select(filterExpression);
                int upperBound = rowArray.GetUpperBound(0);
                for (int i = 0; i <= upperBound; i++)
                {
                    rowArray[i].Delete();
                }
                DataColumn column = currentCart.Tables[0].Columns["item_id"];
                currentCart.Tables[0].Columns.Add("Name", Type.GetType("System.String"));
                foreach (DataRow row in currentCart.Tables[0].Rows)
                {
                    String name = SearchIDGoods(row["item_id"].ToString());
                    if (name != "")
                    {
                        row["Name"] = name;
                    }
                }
                return currentCart;
            }
            catch (Exception exception)
            {
                currentCart.Clear();
                WriteFile("Error in Carts.LoadCart(): " + exception.Message);
                HttpContext.Current.Response.Redirect("~/Default.aspx");
                return null;
            }
        }

        /// <summary>
        /// searching an item name for its item_id
        /// </summary>
        /// <param name="item_id">the item identifier</param>
        /// <returns>the item name</returns>
        public static string SearchIDGoods(string item_id)
        {
            try
            {
                string filterExpression = "id = '" + item_id + "'";
                DataRow[] rowArray = goods.Tables[0].Select(filterExpression);
                if (rowArray.Length == 1)
                {
                    return rowArray[0]["name"].ToString();
                }
            }
            catch (Exception exception)
            {
                WriteFile("Error in Carts.SearchIDGoods(): " + exception.Message);
            }
            return "";
        }

        /// <summary>
        /// creating a Log file
        /// </summary>
        /// <param name="text">a text to be written in the Log file</param>
        public static void WriteFile(string text)
        {
            CultureInfo provider = new CultureInfo("en-us");
            StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath("Log") + @"\Log.txt", true, System.Text.Encoding.ASCII);
            writer.Write(DateTime.Now.ToString(provider));
            writer.Write(": ");
            writer.Write(text);
            writer.Write(Environment.NewLine);
            writer.Close();
        }
    }
}