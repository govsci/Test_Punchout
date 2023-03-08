using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Test_Punchout.Classes;
using System.Xml;

namespace Test_Punchout.cart
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Test_Punchout master = (Test_Punchout)Page.Master;
            if (master.GetUser() == null)
                Response.Redirect("~/login/");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
              | SecurityProtocolType.Tls11
              | SecurityProtocolType.Tls12
              | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            if (Request.Form["cxml-urlencoded"] != null)
                Session["cxml-urlencoded"] = Request.Form["cxml-urlencoded"];

            if (!IsPostBack && Session["cxml-urlencoded"] != null)
            {
                string punchoutCxml = (string)Session["cxml-urlencoded"];
                List<ItemIn> items = GetCxmlCartContents(punchoutCxml);
                if (items != null)
                {
                    PrintCart(items);

                    PunchoutSetup setup = (PunchoutSetup)Session["punchout-setup"];
                    if (setup != null)
                    {
                        Session["items"] = items;
                        sendPoRequestButton.Text = $"Send {setup.DeploymentMode} PO";
                        sendPoRequestButton.Visible = true;
                    }
                }
            }
        }

        private List<ItemIn> GetCxmlCartContents(string punchoutcXML)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.XmlResolver = null;
                xml.LoadXml(punchoutcXML);
                return XML.ReadPoOrderMessage(xml);
            }
            catch(Exception ex)
            {
                error.InnerText = ex.ToString();
            }

            return null;
        }

        private void PrintCart(List<ItemIn> items)
        {
            foreach (ItemIn item in items)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();

                cell = new TableCell();
                cell.Text = item.ManufacturerName;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = item.ManufacturerPartId;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = item.SupplierPartId;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = item.SupplierPartAuxId;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = item.Description;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = item.UnitOfMeasure;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = item.Quantity.ToString("G29");
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = item.UnitPrice.ToString("C");
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = item.Classification;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = item.Extrinsics;
                row.Cells.Add(cell);

                cartContents.Rows.Add(row);
            }
        }

        protected void sendPoRequestButton_Click(object sender, EventArgs e)
        {
            PunchoutSetup setup = (PunchoutSetup)Session["punchout-setup"];
            List<ItemIn> items = (List<ItemIn>)Session["items"];
            PrintCart(items);

            if (setup != null && items != null)
            {
                XmlDocument xml = XML.CreatePoXml(setup, items);
                SendPO(xml, setup.DeploymentMode);
            }
        }

        private void SendPO(XmlDocument xml, string deployment)
        {
            try
            {
                string response = "";
                // Force bypassing SSL handshake
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                        | SecurityProtocolType.Tls11
                        | SecurityProtocolType.Tls12
                        | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                byte[] reqBytes = System.Text.Encoding.ASCII.GetBytes(xml.InnerXml);

                HttpWebRequest wreq;
                switch (deployment)
                {
                    case "Test": wreq = (HttpWebRequest)WebRequest.Create(Constants.TestPoURL); break;
                    case "Production": wreq = (HttpWebRequest)WebRequest.Create(Constants.ProdPoURL); break;
                    default: wreq = (HttpWebRequest)WebRequest.Create(Constants.TestPoURL); break;
                }
                wreq.Timeout = 300000;
                wreq.Method = "POST";
                wreq.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                wreq.ContentType = "text/xml";
                System.IO.Stream rs = wreq.GetRequestStream();
                rs.Write(reqBytes, 0, reqBytes.Length);
                rs.Close();

                HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                if (wresp.StatusCode == HttpStatusCode.OK)
                {
                    System.IO.Stream s = wresp.GetResponseStream();
                    System.IO.StreamReader readStream = new System.IO.StreamReader(s);
                    response = readStream.ReadToEnd();
                }

                wresp.Close();

                XmlDocument xmlResponse = new XmlDocument();
                xmlResponse.XmlResolver = null;
                xmlResponse.LoadXml(response);

                PunchoutResponse resp = XML.ReadPoResponse(xmlResponse);
                error.InnerHtml = $"Status::<br><div style='margin-left:25px;font-style:italic;'>Code: {resp.Code}<br>Text: {resp.Status}<br>Additional Text: {resp.StatusText}</div><br>";
            }
            catch(Exception ex)
            {
                error.InnerText = ex.ToString();
            }
        }
    }
}