using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Test_Punchout.Classes;
using System.DirectoryServices.Protocols;

namespace Test_Punchout.login
{
    public partial class _default : System.Web.UI.Page
    {
        Test_Punchout master;
        protected void Page_Load(object sender, EventArgs e)
        {
            master = (Test_Punchout)Page.Master;
            if (master.GetUser() != null)
                Response.Redirect("~/");
        }

        protected bool ValidateForm()
        {
            bool errors = false;

            if(userNameTextbox.Text.Length==0)
            {
                error.InnerHtml = "Username is required.<br>";
                errors = true;
            }

            if (passwordTextBox.Text.Length == 0)
            {
                error.InnerHtml = "Password is required.";
            }

            return errors;
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                try
                {
                    LdapConnection ldap = new LdapConnection(new LdapDirectoryIdentifier(Constants.LDAPServer, Constants.LDAPPort));
                    ldap.AuthType = AuthType.Basic;
                    ldap.Bind(new System.Net.NetworkCredential(userNameTextbox.Text, passwordTextBox.Text, Constants.Domain));
                    Session.Clear();
                    User user = new User(userNameTextbox.Text);
                    Session["user"] = user;
                    Response.Redirect("~/");
                }
                catch(LdapException)
                {
                    error.InnerHtml = "Authentication Failure. Please try again.";
                }
                catch(Exception ex)
                {
                    error.InnerText = ex.ToString();
                }
            }
        }
    }
}