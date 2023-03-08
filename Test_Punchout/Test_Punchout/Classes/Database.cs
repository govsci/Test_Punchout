using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Test_Punchout.Classes
{
    public static class Database
    {
        public static List<PunchoutSetup> GetPunchoutSetups(string deployment)
        {
            List<PunchoutSetup> punchoutSetups = new List<PunchoutSetup>();

            try
            {
                SqlConnection dbcon;

                switch (deployment)
                {
                    case "Test": dbcon = new SqlConnection(Constants.TstEcomDbConnection); break;
                    case "Production": dbcon = new SqlConnection(Constants.PrdEcomDbConnection); break;
                    default: dbcon = new SqlConnection(Constants.TstEcomDbConnection); break;
                }

                dbcon.Open();
                SqlCommand cmd = new SqlCommand("[Ecommerce.Test.Punchout.Control]", dbcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@method", "GET CUSTOMER PUNCHOUTS"));
                using (SqlDataReader rs = cmd.ExecuteReader())
                    while (rs.Read())
                        punchoutSetups.Add(new PunchoutSetup(int.Parse(rs["id"].ToString()), rs["customerID"].ToString(), rs["matrixName"].ToString(), rs["productTable"].ToString()));
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return punchoutSetups;
        }

        public static List<string[]> GetItems(string deployment, string table)
        {
            List<string[]> items = new List<string[]>();
            try
            {
                SqlConnection dbcon;

                switch (deployment)
                {
                    case "Test": dbcon = new SqlConnection(Constants.TstEcomDbConnection); break;
                    case "Production": dbcon = new SqlConnection(Constants.PrdEcomDbConnection); break;
                    default: dbcon = new SqlConnection(Constants.TstEcomDbConnection); break;
                }

                dbcon.Open();
                SqlCommand cmd = new SqlCommand($"SELECT TOP 1000 [VendorDesc],[Vendor Item No_] FROM [dbo].[{table}.Products] ORDER BY NEWID()", dbcon);
                using (SqlDataReader rs = cmd.ExecuteReader())
                    while (rs.Read())
                        items.Add(new string[] { rs["VendorDesc"].ToString(), rs["Vendor Item No_"].ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return items;
        }

        public static PunchoutSetup GetPunchoutSetup(string deployment, int id)
        {
            PunchoutSetup setup = null;

            try
            {
                string punchoutUrl = "";
                SqlConnection dbcon;

                switch (deployment)
                {
                    case "Test":
                        dbcon = new SqlConnection(Constants.TstEcomDbConnection);
                        punchoutUrl = Constants.TestPunchoutURL;
                        break;
                    case "Production":
                        dbcon = new SqlConnection(Constants.PrdEcomDbConnection);
                        punchoutUrl = Constants.ProductionPunchoutURL;
                        break;
                    default:
                        dbcon = new SqlConnection(Constants.TstEcomDbConnection);
                        punchoutUrl = Constants.TestPunchoutURL;
                        break;
                }

                dbcon.Open();
                SqlCommand cmd = new SqlCommand("[Ecommerce.Test.Punchout.Control]", dbcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@method", "GET PUNCHOUT SETUP"));
                cmd.Parameters.Add(new SqlParameter("@id", id));
                using (SqlDataReader rs = cmd.ExecuteReader())
                    if (rs.Read())
                        setup = new PunchoutSetup(deployment
                            , Constants.BrowserFormPost
                            , punchoutUrl
                            , int.Parse(rs["id"].ToString())
                            , int.Parse(rs["parent"].ToString())
                            , rs["customerID"].ToString()
                            , rs["DUNS"].ToString()
                            , rs["sharedSecret"].ToString()
                            , rs["matrixName"].ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return setup;
        }
    }
}