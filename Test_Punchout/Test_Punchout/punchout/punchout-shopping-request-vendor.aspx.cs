using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Test_Punchout.Classes;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Test_Punchout.punchout
{
    public partial class punchout_shopping_request_vendor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Test_Punchout master = (Test_Punchout)Page.Master;
            if (master.GetUser() == null)
                Response.Redirect("login");

            string deployment = Request.QueryString["d"];
            string customer = Request.QueryString["c"];
            string item = Request.QueryString["it"];
            int customerNumber = 0;

            try { customerNumber = int.Parse(customer); }
            catch { }

            if (deployment != null && deployment.Length > 0 && customer != null && customerNumber > 0)
                TestPunchout(deployment, int.Parse(customer), item);
        }

        private void TestPunchout(string deployment, int customer, string item)
        {
            try
            {
                XmlDocument xml = null, response = null;
                PunchoutResponse poResponse = null;

                PunchoutSetup setup = Database.GetPunchoutSetup(deployment, customer);

                if (setup != null)
                    xml = XML.CreatePosrXml(setup, item);

                if (xml != null)
                    response = SendRequest(setup, xml);

                if (response != null)
                    poResponse = XML.ReadPosrResponse(response);

                if (poResponse != null)
                {
                    if (poResponse.Code.CompareTo("200") == 0)
                    {
                        Session["punchout-setup"] = setup;
                        Response.Redirect(System.Web.HttpUtility.HtmlDecode(poResponse.StartPageUrl));
                    }
                    else
                        error.InnerHtml = poResponse.Code + ": " + poResponse.Status + "<br><br>" + System.Web.HttpUtility.HtmlEncode(poResponse.XML);
                }
            }
            catch (Exception ex)
            {
                error.InnerText = ex.ToString();
            }
        }

        private XmlDocument SendRequest(PunchoutSetup setup, XmlDocument xml)
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

                Response.AddHeader("Set-Cookie", "HttpOnly;Secure;SameSite=Strict");

                byte[] reqBytes = System.Text.Encoding.ASCII.GetBytes(xml.InnerXml);

                HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(setup.SupplierURL);
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
                return xmlResponse;
            }
            catch (Exception ex)
            {
                error.InnerText = ex.ToString();
                return null;
            }
        }
    }
}