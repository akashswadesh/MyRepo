using Papaspizza.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Papaspizza.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
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
                    //p.id = item["id"].ToString();
                    //p.Title = item["ProductName"].ToString();
                    //p.Size = item["Size"].ToString();
                    //p.Price = item["Price"].ToString();
                    //p.TypeName = item["PriceType"].ToString();
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
                    p.Price = item["Price"].ToString();
                    p.TypeName = item["PriceType"].ToString();
                    p.Qty = item["Qty"].ToString();
                    p.InstanceID = item["GroupID"].ToString();
                    Cus.Add(p);
                }
            }

            @ViewBag.CartItemList = Item;
            @ViewBag.CartCusList = Cus;
        }
        public ActionResult Index()
        {
            ListCart();
            ListCategoryMenu();
            return View();
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
                    p.ImgURL = ds.Tables[0].Rows[i]["ImgURL"].ToString().Replace("~","");
                    Category.Add(p);
                }
            }
            else
            {
                Property p = new Property();
                p.id = "0";
                p.Category = "NONE";

                p.ImgURL = "/images/photo_default.png";
                p.Status = "NONE";

                Category.Add(p);
            }
            @ViewBag.CategoryList = Category;
        }
    }
}
