using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Test_Punchout.Classes;
using System.Xml;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Test_Punchout
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Test_Punchout master = (Test_Punchout)Page.Master;
            if (master.GetUser() == null)
                Response.Redirect("login");

            if (!IsPostBack)
            {
                deploymentDropDown.SelectedIndex = 0;
                GetCustomerList();
                GetItemList();
                if (Constants.ProdAuthUsers.Contains(";" + master.GetUser().UserName + ";"))
                    deploymentDropDown.Items.Add(new ListItem("Production", "Production"));
            }
            else
                GetCustomerList();
        }

        private void GetCustomerList()
        {
            string selected = "";
            if (customerDropDown.Items.Count > 0)
                selected = customerDropDown.SelectedValue;

            customerDropDown.Items.Clear();
            try
            {
                List<PunchoutSetup> setups = Database.GetPunchoutSetups(deploymentDropDown.SelectedValue);
                if (setups.Count > 0)
                {
                    foreach (PunchoutSetup setup in setups)
                    {
                        ListItem item = new ListItem(setup.MatrixName.Replace(" Disc", "") + " (" + setup.CustomerID + ")", setup.ID.ToString());
                        item.Attributes.Add("producttable", setup.ItemTable);
                        customerDropDown.Items.Add(item);
                    }
                }
            }
            catch(Exception ex)
            {
                error.InnerText = ex.ToString();
            }

            if (selected.Length > 0)
                customerDropDown.SelectedValue = selected;
        }

        protected void deploymentDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCustomerList();
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            Response.Redirect($"punchout/punchout-shopping-request.aspx?d={deploymentDropDown.SelectedValue}&c={customerDropDown.SelectedValue}&n={customerDropDown.SelectedItem.Text}&it={itemDropDown.SelectedValue}");
        }

        protected void customerDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetItemList();
        }

        protected void GetItemList()
        {
            itemDropDown.Items.Clear();
            try
            {
                ListItem selectedItem = customerDropDown.SelectedItem;
                string table = selectedItem.Attributes["producttable"];
                if (table != null && table.Length > 0)
                {
                    List<string[]> items = Database.GetItems(deploymentDropDown.SelectedValue, table);
                    foreach (string[] item in items)
                    {
                        ListItem it = new ListItem($"{item[0]}, {item[1]}", $"{item[0]}|{item[1]}");
                        itemDropDown.Items.Add(it);
                    }
                }
            }
            catch (Exception ex)
            {
                error.InnerText = ex.ToString();
            }
        }
    }
}