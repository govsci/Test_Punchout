using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Test_Punchout.Classes;

namespace Test_Punchout
{
    public partial class Test_Punchout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GetUser() != null)
            {
                logoTd.Controls.Add(new LiteralControl("<a style='color:white' href='/testPunchout/logout/'>Logout</a>"));
            }
        }

        public User GetUser()
        {
            return (User)Session["user"];
        }
    }
}