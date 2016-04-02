using Papaspizza.Models;
using PaypalMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Papaspizza.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/
        Datalayer dl = new Datalayer();
        public ActionResult Index()
        {
            ListCategoryMenu();
            Property p = new Property();
            p = FillCart(p);

            return View(p);
        }

        public Property FillCart(Property p)
        {

            string value = "";
            decimal extracharges = 0;
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            try
            {
                //value = Session["menu"].ToString();
                HttpCookie cartCookie = Request.Cookies["cart"];
                if (cartCookie == null)
                {
                    p.CartID = "0";
                }
                else
                {
                    p.CartID = cartCookie["cartid"];
                }
                if (p.CartID != null)
                {
                    ds = dl.FillCart(p);


                }
            }
            catch (Exception ex)
            {

                RedirectToAction("Index", "Home");
            }
            List<Property> cartList = new List<Property>();
            List<Property> menuList = new List<Property>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property p1 = new Property();
                    p1.ProId = ds.Tables[0].Rows[i]["PrdId"].ToString();
                    p1.Price = ds.Tables[0].Rows[i]["Price"].ToString();
                    p1.Qty = ds.Tables[0].Rows[i]["Qty"].ToString();

                    p1.Title = ds.Tables[0].Rows[i]["ProductName"].ToString();
                    p1.ImgURL = ds.Tables[0].Rows[i]["ImgURL"].ToString().Replace("~", "");
                    p1.Amount = ds.Tables[0].Rows[i]["TotPrice"].ToString();




                    cartList.Add(p1);

                    p.TotalAmount = string.Format("{0:C}", ((Convert.ToDecimal(p.TotalAmount) + Convert.ToDecimal(p1.Amount)).ToString()));

                    menuList.Add(p1);
                }

                Session["TotalAmount"] = p.TotalAmount;
                TempData["Total"] = p.TotalAmount;
            }

            @ViewBag.menuList = cartList;
            return p;
        }


        public ActionResult Remove(string id)
        {
            Property p = new Property();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(p.Con);
            con.Open();

            SqlCommand cmd = new SqlCommand("Delete from [dbo].[tbl_Cart] where  GroupID='" + id + "'", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Dispose();



            return Redirect("/Home/Index");
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
       
        public ActionResult CheckOut()
        {
            ListCategoryMenu();
            ListCart();
            ValidateCommand();
            return View();
        }


        public void ValidateCommand()
        {
            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSandbox"]);
            var paypal = new PayPalModel(useSandbox);

            string Itm = "";
            DataSet ds = new DataSet();
            int x = 1;
            decimal extraprice = 0;
            TempData["CrtFrm"] = "";
            Property p = new Property();


            HttpCookie cartCookie = Request.Cookies["cart"];// new HttpCookie("Cart");
            if (cartCookie != null)
            {
                p.CartID = cartCookie["cartid"];
                ds = dl.FillCart(p);
            }

            string str = "";
            str += "<input type='hidden' name='cmd' value='_cart'>";
            str += "<input type='hidden' name='upload' value='1'>";
            str += "<input type='hidden' name='no_note' value='0'>";
            str += "<input type='hidden' name='cn' value=''>";
            str += " <input type='hidden' name='rm' value='2'>";
            str += "<input type='hidden' name='image_url' value='http://demo25.gowebbi.in/images/logo.png'>";
            str += "<input type='hidden' name='custom' value=''>";
            str += "<input type='hidden' name='cpp_header_image' value='http://demo25.gowebbi.in/images/logo.png'>";
            str += "<input type='hidden' name='business' value='" + ConfigurationManager.AppSettings["business"].ToString() + "'>";
            //str += "<input type='hidden' name='notify_url' value='" + ConfigurationManager.AppSettings["notify_url"].ToString() + "'>";
            str += "<input type='hidden' name='return' value='" + ConfigurationManager.AppSettings["return"].ToString() + "'>";
            str += "<input type='hidden' name='cancel_return' value='" + ConfigurationManager.AppSettings["cancel_return"].ToString() + "'>";
            str += "<input type='hidden' name='currency_code' value='" + ConfigurationManager.AppSettings["currency_code"].ToString() + "'>";

            List<Property> menuList = new List<Property>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Itm += "<input type='hidden' name='item_name_" + x + "' value='" + ds.Tables[1].Rows[i]["Item"].ToString() + "'/>";                   
                    Itm += "<input type='hidden' name='amount_" + x + "' value='" + ds.Tables[0].Rows[i]["Price"].ToString() + "'/>";
                    Itm += "<input type='hidden' name='quantity_" + x + "' value='" + ds.Tables[0].Rows[i]["Qty"].ToString() + "'/>";

                    x++;
                }
            }

            TempData["CrtFrm"] = str + Itm;
            TempData["actionurl"] = paypal.actionURL;
            //  return Content("<script>alert('" + AMTcookie["AMT"] + "'); window.location.herf='/paypal/ValidateCommand' </script>");
            // return View(paypal);
            //return Redirect(paypal.actionURL);
        }

        public string GuestCheckout(string fname, string lname, string emailid, string phno, string State, string zip, string Adress)
        {
            ListCategoryMenu();
            try
            {
                Property p = new Property();
                p.id = "0";
                p.userid = "CUSTOMER";

                p.FirstName = fname;
                p.LastName = lname;
                p.EmailID = emailid;
                p.Page = zip;
                p.ContactNo = phno;
                p.Country = State;
                p.ShippingAddress = Adress;

                HttpCookie cartCookie = Request.Cookies["cart"];
                if (cartCookie != null)
                {
                    p.CartID = cartCookie["cartid"];
                }


                if (dl.INSERT_UPDATE_CUSTOMER_REGISTRATION(p) > 0)
                {

                    return "success";

                }
                else
                {

                    TempData["Login"] = "Problem in registration! Please Visit Again.";
                    return "fail";
                }

            }
            catch (Exception ex)
            {
                TempData["msg1"] = "Email ID already exists.";
                TempData["Login"] = ex.ToString();
                return "fail";
            }

        }

        public ActionResult Order()
        {
            try
            {
               
                Datalayer dl = new Datalayer();
                DataSet ds = new DataSet();
                Property p1 = new Property();
               
                List<Property> Cart = new List<Property>();                
                p1.onTable = "CART_LIST_DETAILS";
              

                ds = dl.FETCH_CONDITIONAL_QUERY(p1);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        Property p = new Property();

                        p.CartID = item["CartID"].ToString();
                        p.FullName =item["Fname"].ToString() + " " + item["Lname"].ToString();
                        p.EmailID=item["EmailId"].ToString();
                        p.ContactNo = item["ContactNo"].ToString();
                        p.onDate =item["Date"].ToString();
                        p.Status =item["PaymentStatus"].ToString();
                        Cart.Add(p);
                    }
                }               
                @ViewBag.CartList = Cart;
                return View();
            }
            catch(Exception ex)
            {
                TempData["MSG"] = ex.ToString();
                return Redirect("/Admin");
            }
        }

        public ActionResult OrderDetails(string id)
        {
            try
            {
                TempData["id"] = id;
                if (String.IsNullOrEmpty(id))
                {
                    id = "0";
                }
                Datalayer dl = new Datalayer();
                DataSet ds = new DataSet();
                Property p1 = new Property();
                var Info = new Papaspizza.Models.Property();
                double total = 0;
                List<Property> Item = new List<Property>();
                List<Property> Cus = new List<Property>();
                p1.onTable = "ALL_CART_LIST_DETAILS";
                p1.Condition1 = id;

                ds = dl.FETCH_CONDITIONAL_QUERY(p1);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Info = new Papaspizza.Models.Property()
                    {

                        CartID = ds.Tables[0].Rows[0]["CartId"].ToString(),
                        FullName = ds.Tables[0].Rows[0]["Fname"].ToString() + " " + ds.Tables[0].Rows[0]["Lname"].ToString(),
                        EmailID = ds.Tables[0].Rows[0]["EmailID"].ToString(),
                        ContactNo = ds.Tables[0].Rows[0]["ContactNo"].ToString(),
                        ShippingAddress = ds.Tables[0].Rows[0]["DAddress"].ToString(),
                        Page = ds.Tables[0].Rows[0]["Pincode"].ToString(),
                        Country = ds.Tables[0].Rows[0]["Country"].ToString(),
                        onDate = ds.Tables[0].Rows[0]["Date"].ToString(),
                        Status = ds.Tables[0].Rows[0]["OrderStatus"].ToString(),
                    };
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        Property p = new Property();
                        p.InstanceID = item["GroupID"].ToString();
                        p.TypeName = item["PriceType"].ToString();
                        Item.Add(p);
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[2].Rows)
                    {
                        Property p = new Property();
                      
                        total = total + Convert.ToDouble(item["totPrice"]);
                        p.Title = item["ProductName"].ToString();
                        p.Price = item["Price"].ToString();
                        p.SmallDesc = item["SmallDesc"].ToString();
                        p.DealDesc = item["DealDesc"].ToString();
                        p.Message = item["Message"].ToString();
                        p.InstanceID = item["GroupID"].ToString();
                        p.Qty = item["Qty"].ToString();
                        p.Amount = item["totPrice"].ToString();
                        Cus.Add(p);
                    }
                }

                @ViewBag.CartItemList = Item;
                @ViewBag.CartCusList = Cus;
               
                @TempData["Total"] = total;
                return View(Info);
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
                return Redirect("/Admin");
            }
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
       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult StatusChange(Property p)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(p.Con);
            con.Open();

            SqlCommand cmd = new SqlCommand("update [dbo].[tbl_Cart] set OrderStatus='" + p.Status + "' where  CartId='" + p.CartID + "'", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Dispose();



            return Redirect("/Cart/Order");

        }




        //public string Header()
        //{
        //    string result = "";
        //    HttpCookie cartCookie = Request.Cookies["cart"];    // new HttpCookie("Cart");
        //    if (cartCookie != null)
        //    {
        //        Property p = new Property();
        //        if (cartCookie == null)
        //        {
        //            p.CartID = "0";
        //        }
        //        else
        //        {
        //            p.CartID = cartCookie["cartid"];
        //        }
        //        SqlConnection con = new SqlConnection(p.Con);
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("select qty,SUM(price*Qty)TotalAmt from tbl_Cart c where cartid='" + p.CartID + "' group by GroupID,Qty", con);
        //        cmd.Parameters.Add("@id", SqlDbType.NVarChar);
        //        cmd.Parameters["@id"].Value = cartCookie["cartid"];
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);
        //        da.Dispose();
        //        con.Dispose();
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            result = ds.Tables[0].Rows[0]["qty"].ToString() + "!" + ds.Tables[0].Rows[0]["TotalAmt"].ToString();
        //        }
        //        else
        //        {
        //            result = "0!0.00";
        //        }
        //    }
        //    else
        //    {
        //        result = "0!0.00";
        //    }
        //    return result;
        //}




        [HttpPost]
        public JsonResult addtoCart(Property prop)
        {
            Property p = prop;
            if (String.IsNullOrEmpty(p.id))
            {
                p.id = "0";
            }
            if (String.IsNullOrEmpty(p.Item_Offered)) { p.Item_Offered = "0|0"; }
            else
            {
                p.Item_Offered = p.Item_Offered.TrimEnd(',');
            }
            HttpCookie cartCookie = Request.Cookies["cart"];
            try
            {
                if (cartCookie == null)
                {

                    HttpCookie cartCookie1 = new HttpCookie("cart");
                    cartCookie1.Expires = DateTime.Now.AddDays(5);
                    Guid id = Guid.NewGuid();
                    cartCookie1["cartid"] = id.ToString();
                    p.CartID = id.ToString();                   
                  
                    int res = dl.InsertUpdateCart(p);

                    Response.Cookies.Add(cartCookie1);
                    return Json("Successfully Add To Cart!",JsonRequestBehavior.AllowGet);
                }
                else
                {

                    p.CartID = cartCookie["cartid"];

                    int res = dl.InsertUpdateCart(p);
                    return Json("Successfully Add To Cart!", JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { message = "this is going Wrong!" });
            }

            
        }

        [HttpPost]
        public JsonResult addtoCart_combo(Property prop)
        {
            Property p = prop;
            if (String.IsNullOrEmpty(p.id))
            {
                p.id = "0";
            }
            if (String.IsNullOrEmpty(p.Item_Offered)) { p.Item_Offered = "0|0"; }
            else
            {
                p.Item_Offered = p.Item_Offered.TrimEnd(',');
            }
            try
            {
                p.ProductName = p.ProductName.TrimEnd('|');
            }
            catch (Exception)
            {
            }
            HttpCookie cartCookie = Request.Cookies["cart"];
            try
            {
                if (cartCookie == null)
                {

                    HttpCookie cartCookie1 = new HttpCookie("cart");
                    cartCookie1.Expires = DateTime.Now.AddDays(5);
                    Guid id = Guid.NewGuid();
                    cartCookie1["cartid"] = id.ToString();
                    p.CartID = id.ToString();

                    int res = dl.hm_InsertUpdateCart_Combo(p);

                    Response.Cookies.Add(cartCookie1);
                    return Json("Successfully Add To Cart!", JsonRequestBehavior.AllowGet);
                }
                else
                {

                    p.CartID = cartCookie["cartid"];

                    int res = dl.hm_InsertUpdateCart_Combo(p);
                    return Json("Successfully Add To Cart!", JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { message = "this is going Wrong!" });
            }


        }

        [AllowAnonymous]
        public string check_free_item(string id, string Qty)
        {
            string retPth = "";
            Property p = new Property();
            p.id = id;
            p.Qty = Qty;
            DataSet ds = new DataSet();
            ds = dl.CHECK_FREE_ITEM_DATA(p);
            if (ds.Tables[0].Rows.Count > 0)
            {
                retPth = ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                retPth = "Error.";
            }
            return retPth;
        }
    }
}
