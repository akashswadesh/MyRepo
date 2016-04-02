using Papaspizza.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Papaspizza.Models
{
    //--------------------------Data Layer Modified on 30-06-2015---------------------------------------
    public class Datalayer
    {
        public static byte[] pImage;

        public int Int_Process(String Storp, string[] parametername, string[] parametervalue)
        {
            int a = 0;
            Property p = new Property();
            SqlConnection con = new SqlConnection(p.Con);
            SqlCommand cmd = new SqlCommand(Storp, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < parametername.Length; i++)
            {
                if (parametername[i] == "@img")
                {
                    cmd.Parameters.AddWithValue(parametername[i], pImage);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parametername[i], parametervalue[i]);
                }
            }
            con.Open();

            a = cmd.ExecuteNonQuery();
            con.Dispose();
            return a;
        }
        public DataSet Ds_Process(String Storp, string[] parametername, string[] parametervalue)
        {
            try
            {
                Property p = new Property();
                SqlConnection con = new SqlConnection(p.Con);
                SqlCommand cmd = new SqlCommand(Storp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < parametername.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parametername[i], parametervalue[i]);
                }
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                con.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                DataSet ds = null;
                return ds;
            }

        }
        public DataSet MyDs_Process(String Storp)
        {

            Property p = new Property();
            SqlConnection con = new SqlConnection(p.Con);
            SqlCommand cmd = new SqlCommand(Storp, con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            da.Dispose();
            con.Dispose();
            return ds;

        }


        //----------------------Data Access Layer Work---------------------------


        EncryptDecrypt enc = new EncryptDecrypt();

        public DataSet FETCH_LOGIN_DETAILS(Property p)
        {
            try
            {
                string[] paname = { "@Emailid", "@Password" };
                string[] pvalue = { p.EmailID,p.Password};
                return Ds_Process("FETCH_LOGIN_DETAILS", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }

        public DataSet FETCH_CONDITIONAL_QUERY(Property p)
        {
            try
            {
                string[] paname = { "@Condition1", "@Condition2", "@Condition3", "@onTable" };
                string[] pvalue = { p.Condition1, p.Condition2, p.Condition3, p.onTable };
                return Ds_Process("FETCH_CONDITIONAL_QUERY", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }

        public DataSet INSERT_UPDATE_USER_REGISTRATION(Property p)
        {
            try
            {
                string[] paname = { "@id", "@Fname", "@Lname", "@ContactNo", "@EmailID", "@Password" };
                string[] pvalue = { p.id, p.FirstName, p.LastName, p.ContactNo, p.EmailID, p.Password};
                return Ds_Process("INSERT_UPDATE_USER_REGISTRATION", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }

        public int INSERT_UPDATE_CATEGORY(Property p)
        {
            try
            {
                string[] paname = { "@id","@Category","ImgURL","@status"};
                string[] pvalue = { p.id, p.Category,p.ImgURL,p.Status};
                return Int_Process("INSERT_UPDATE_CATEGORY", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }


        public int INSERT_UPDATE_SIZE(Property p)
        {
            try
            {
                string[] paname = { "@id", "@Size", "@status" };
                string[] pvalue = { p.id, p.Size,p.Status };
                return Int_Process("INSERT_UPDATE_SIZE", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }
        public int INSERT_UPDATE_DIP(Property p)
        {
            try
            {
                string[] paname = { "@id", "@Dip", "@status" };
                string[] pvalue = { p.id, p.Cus_ID, p.Status };
                return Int_Process("INSERT_UPDATE_DIP", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }


        public int INSERT_UPDATE_PRODUCT(Property p)
        {
            try
            {
                string[] paname = { "@id", "@Description", "@Title", "@Category", "@ImgURL", "@Type", "@Status", "@Size_Price", "@Price", "@Dip" };
                string[] pvalue = { p.id, p.Description, p.Title, p.Category, p.ImgURL, p.TypeName, p.Status, p.Size_Price, p.Price, p.Cus_ID };
                return Int_Process("INSERT_UPDATE_PRODUCT", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }
        public int INSERT_UPDATE_CUSTOMISE(Property p)
        {
            try
            {
                string[] paname = { "@id","@ProID", "@Cus_ID"};
                string[] pvalue = { p.id, p.ProductName, p.Cus_ID};
                return Int_Process("INSERT_UPDATE_CUSTOMISE", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }
        public int INSERT_MENU(Property p)
        {
            try
            {
                string[] paname = { "@id", "@Title","@Price","@Description", "@ImgURL", "@Status", "@Menu_Item","@Type"};
                string[] pvalue = { p.id, p.Title,p.Price,p.Description, p.ImgURL, p.Status, p.Menu_Item,p.TypeName};
                return Int_Process("INSERT_MENU", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }
        public int INSERT_FREE(Property p)
        {
            try
            {
                string[] paname = { "@id", "@Title","@Description", "@ImgURL", "@Status", "@Item_Offered","@Qty","@PayQty","@Type","@Price" };
                string[] pvalue = { p.id, p.Title, p.Description, p.ImgURL, p.Status,p.Category,p.Qty,p.Item_Free,p.TypeName,"0"};
                return Int_Process("INSERT_FREE", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }
        public int INSER_UPDATE_POSTCODE(Property p)
        {
            try
            {
                string[] paname = { "@id", "@Postcode", "@Status" };
                string[] pvalue = { p.id, p.Page, p.Status };
                return Int_Process("INSER_UPDATE_POSTCODE", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////Cart//////////////////////////////////////////////////
        public int InsertUpdateCart(Property p)
        {
            try
            {
                string[] paname = { "@Id", "@ProductName", "@SmallDesc", "@Size", "@price", "@Qty", "@GroupID", "@CartID", "@Cus", "@PriceType", "@DealDesc" ,"@Dip","@Message"};
                string[] pvalue = { p.id, p.ProductName, p.Description, p.Size, p.Price, p.Qty, p.InstanceID, p.CartID, p.Item_Offered, p.TypeName, p.DealDesc,p.Title,p.Message};
                return Int_Process("InsertUpdateCart", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }

        public int hm_InsertUpdateCart_Combo(Property p)
        {
            try
            {
                string[] paname = { "@Id", "@ProductName", "@SmallDesc", "@Size", "@price", "@Qty", "@GroupID", "@CartID", "@Cus" };
                string[] pvalue = { p.id, p.ProductName, p.Description, p.Size, p.Price, p.Qty, p.InstanceID, p.CartID, p.Item_Offered };
                return Int_Process("hm_InsertUpdateCart_Combo", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }

        public DataSet FillCart(Property p)
        {
            try
            {
                string[] paname = { "@cartId" };
                string[] pvalue = { p.CartID };
                return Ds_Process("FillCart_sp", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }

        public int DeleteFromCart(Property p)
        {
            try
            {
                string[] paname = { "@id", "@CartId" };
                string[] pvalue = { p.id, p.CartID };
                return Int_Process("DeleteFromCart", paname, pvalue);
            }
            catch (Exception ex)
            {
                //  DataSet ds = new DataSet();
                return 0;
            }
        }

        public DataSet CHECK_FREE_ITEM_DATA(Property p)
        {
            try
            {
                string[] paname = { "@GID", "@Qty" };
                string[] pvalue = { p.id, p.Qty };
                return Ds_Process("CHECK_FREE_ITEM_DATA", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }

        public int INSERT_UPDATE_CUSTOMER_REGISTRATION(Property p)
        {
            try
            {
                string[] paname = { "@id", "@FirstName","@LastName", "@EmailID", "@PostCode","@Country", "@Phone", "@DAddress", "@UserType", "@CARTID" };
                string[] pvalue = { p.id, p.FirstName,p.LastName,p.EmailID,p.Page,p.Country, p.ContactNo, p.ShippingAddress, p.userid, p.CartID };
                return Int_Process("INSERT_UPDATE_CUSTOMER_REGISTRATION", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }
    }
}