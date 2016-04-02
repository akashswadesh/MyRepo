using Papaspizza.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KikaKollectionmvc.Controllers
{
    public class PayPalController : Controller
    {
        //
        // GET: /PayPal/

        Datalayer dl = new Datalayer();

        public ActionResult Index()
        {
            return View();
        }

        public void ListCart()
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();

            List<Property> Item = new List<Property>();
            List<Property> Cus = new List<Property>();
            HttpCookie cartCookie = Request.Cookies["cart"];
            if (cartCookie == null)
            {
                p1.CartID = "0";
            }
            else
            {
                p1.CartID = cartCookie["cartid"];
            }
            p1.onTable = "CART_LIST";
            p1.Condition1 = p1.CartID;

            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Property p = new Property();

                    //p.id =item["id"].ToString();
                    //p.Title = item["ProductName"].ToString();
                    //p.Size = item["Size"].ToString();
                    //p.TypeName = item["PriceType"].ToString();
                    //p.Price = item["Price"].ToString();
                    //p.Qty = item["Qty"].ToString();
                    p.InstanceID = item["GroupID"].ToString();
                    Item.Add(p);
                }
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    Property p = new Property();

                    p.id = item["id"].ToString();
                    p.Title = item["ProductName"].ToString();
                    p.Size = item["Size"].ToString();
                    p.TypeName = item["PriceType"].ToString();
                    p.Price = item["Price"].ToString();
                    p.Qty = item["Qty"].ToString();
                    p.InstanceID = item["GroupID"].ToString();
                    Cus.Add(p);
                }
            }

            @ViewBag.CartItemList = Item;
            @ViewBag.CartCusList = Cus;
        }
        public void ListCategoryMenu()
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();

            List<Property> Category = new List<Property>();

            p1.onTable = "CATEGORY_LIST_TABLE";

            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property p = new Property();

                    p.id = ds.Tables[0].Rows[i]["id"].ToString();
                    p.Category = ds.Tables[0].Rows[i]["Category"].ToString();
                    p.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                    Category.Add(p);
                }
            }
            else
            {
                Property p = new Property();
                p.id = "0";
                p.Category = "NONE";


                p.Status = "NONE";

                Category.Add(p);
            }
            @ViewBag.CategoryList = Category;
        }

        public ActionResult NotifyFromPaypal()
        {
            ListCart();
            ListCategoryMenu();
            Property p = new Property();
           
            HttpCookie cartCookie = Request.Cookies["cart"];
            if (cartCookie != null)
            {
                p.CartID = cartCookie["cartid"];
            }
            else
            {
                p.CartID = "";
            }

            SqlConnection con = new SqlConnection(p.Con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            string str = "update tbl_Cart set PaymentStatus='SUCCESS' where cartid='" + p.CartID + "'";
            SqlCommand cmd = new SqlCommand(str, con);
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {               
                TempData["status"] = "  You have successfully made a payment to us via PayPal. We will contact you shortly .PayPal will email a receipt to you for your payment.";
            }
            else
            {
                TempData["status"] = "Sorry Something Going Wrong your Payment may be successful. Login  to your account and check payment Status";
            }

            if (Request.Cookies["cart"] != null)
            {
                Response.Cookies["cart"].Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies["DCharges"].Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies["DeliveryCharges"].Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies["Emailid"].Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies["FirstCharges"].Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies["Postalcode"].Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies["TotDiscount"].Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies["TotalAmount"].Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies["UserType"].Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies["netAmount"].Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies["CopCode"].Expires = DateTime.Now.AddDays(-1);
                // Response.Cookies.Add(CopCode);
            }
            return View();
        }



    }
}
