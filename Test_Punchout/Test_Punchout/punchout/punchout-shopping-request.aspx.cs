using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Test_Punchout.punchout
{
    public partial class punchout_shopping_request : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("P3P", "CP='CAO COR CURa ADMa DEVa OUR IND ONL COM DEM PRE'");

            if (Request.QueryString["n"] != null)
                this.Title = Request.QueryString["n"];

            string item = "";
            if (Request.QueryString["it"] != null)
                item = Request.QueryString["it"];

            if (Request.QueryString["d"] != null && Request.QueryString["c"] != null)
                shopping.Attributes["src"] = $"punchout-shopping-request-vendor.aspx?d={Request.QueryString["d"]}&c={Request.QueryString["c"]}&it={item}";
            else
                Response.Redirect("~/cart/");
        }
    }
}