using Papaspizza.Helpers;
using Papaspizza.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Papaspizza.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        Datalayer dl = new Datalayer();
        public ActionResult Index(string id)
        {
            try
            {
                TempData["Category"] = id;
                ListCart();
                Property p1 = new Property();
                DataSet ds = new DataSet();

                p1.Condition1 = id;

                p1.onTable = "PRODUCT_BY_CAT";
                ds = dl.FETCH_CONDITIONAL_QUERY(p1);
                List<Property> Pro = new List<Property>();
                List<Property> Cus = new List<Property>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        Property p = new Property();
                        p.id = item["id"].ToString();
                        p.Title = item["Title"].ToString();
                        p.Cus_ID = item["Dip"].ToString();
                        p.Description = item["Description"].ToString();
                        p.ImgURL = item["ImgURL"].ToString().Replace("~", "");
                        Pro.Add(p);
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[2].Rows)
                    {
                        Property p = new Property();
                        p.id = item["id"].ToString();
                        p.Title = item["Title"].ToString();
                        p.Price = item["Price"].ToString();
                        Cus.Add(p);
                    }
                }

                ViewBag.ProductList = Pro;
                ViewBag.CustomiseList = Cus;
                DataView _View = new DataView(ds.Tables[1]);
                ViewBag.DV = _View;
                DataView _CUST = new DataView(ds.Tables[3]);
                ViewBag.CB = _CUST;

                List<SelectListItem> Dip = new List<SelectListItem>();

                Dip.Add(new SelectListItem { Text = "-SELECT-", Value = "0" });
                foreach (DataRow item in ds.Tables[4].Rows)
                {
                    Dip.Add(new SelectListItem { Text = item["Dip"].ToString(), Value = item["Dip"].ToString() });
                }

                ViewBag.DipList = new SelectList(Dip, "Value", "Text");
                ListCategoryMenu();
                return View();
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
                return Redirect("/Home");
            }
        }

        public ActionResult Items()
        {
            ListCart();
            ListCategoryMenu();
            Property p1 = new Property();
            DataSet ds = new DataSet();

            p1.onTable = "SPECIAL_PRODUCT_LIST_ALL";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            List<Property> Item = new List<Property>();
            List<Property> Free = new List<Property>();
            
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Property p = new Property();
                    p.id = item["id"].ToString();                   
                    p.Title = item["Title"].ToString();
                    p.Description = item["Description"].ToString();
                    p.Price = item["Price"].ToString();
                    p.ImgURL = item["ImgURL"].ToString().Replace("~", "");

                    Item.Add(p);
                }

            }
           
            ViewBag.ItemList = Item;
            return View();
        }
       
        public ActionResult Details(String id)
        {
            ListCart();
            ListCategoryMenu();
            Property p1 = new Property();
            DataSet ds = new DataSet();
            p1.Condition1 = id;
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "SPECIAL_PRODUCT_DETAILS";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            var info = new Papaspizza.Models.Property();
            List<Property> Item = new List<Property>();
            List<Property> Cus = new List<Property>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Property p = new Property();
                    p.CatId = item["id"].ToString();
                    p.ProId = item["MID"].ToString();
                    p.id = item["ItemSize"].ToString();
                    p.Title = item["Category"].ToString();
                    p.Qty = item["ItemQty"].ToString();
                    p.Size = item["Size"].ToString();
                    p.ImgURL = item["ImgURL"].ToString().Replace("~", "");
                    //p.TypeName = item["Type"].ToString();
                    Item.Add(p);
                }

            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                info = new Papaspizza.Models.Property()
                {
                    id = ds.Tables[2].Rows[0]["id"].ToString(),
                    Title = ds.Tables[2].Rows[0]["Title"].ToString(),
                    Price = ds.Tables[2].Rows[0]["Price"].ToString(),                  
                    Description = ds.Tables[2].Rows[0]["Description"].ToString(),
                    ImgURL = ds.Tables[2].Rows[0]["ImgURL"].ToString().Replace("~", ""),
                };
            }
            else
            {
                info = new Property()
                {
                    id = "0",
                    Title = "None",
                    Description = "None",
                    Price="0",
                   
                };
            }

            ViewBag.ItemList = Item;
            DataView _View = new DataView(ds.Tables[1]);
            ViewBag.PN = _View;

            if (ds.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[3].Rows)
                {
                    Property p = new Property();
                    p.id = item["id"].ToString();
                    p.Title = item["Title"].ToString();
                    p.Price=item["Price"].ToString();
                    Cus.Add(p);
                }
            }

          
            ViewBag.CustomiseList = Cus;
            return View(info);
        }

        [AllowAnonymous]
        public string ChoosenProduct(string ID, string Size)
        {
            string retPth = "";
            //string type = TempData["Pro"].ToString();
            Property p = new Property();
            SqlConnection con = new SqlConnection(p.Con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("select p.id, p.Title+'('+Si.Size+')' Title, S.Price, p.ImgURL, p.Description,Si.Size from dbo.tbl_Product P inner join dbo.tbl_Pro_Size_Price S on p.id=s.ProID inner join [dbo].[tbl_Size] Si on S.SizeID=Si.id where p.id='" + ID + "'and S.SizeID='"+Size+"'", con);


            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                retPth = Convert.ToString(ds.Tables[0].Rows[0]["id"]).ToString() + "^" + Convert.ToString(ds.Tables[0].Rows[0]["Title"]).ToString() + "^" + Convert.ToString(ds.Tables[0].Rows[0]["ImgURL"]).Split('~')[1].ToString() + "^" + Convert.ToString(ds.Tables[0].Rows[0]["Description"]).ToString() + "^" + Convert.ToString(ds.Tables[0].Rows[0]["Price"]).ToString() + "^" + Convert.ToString(ds.Tables[0].Rows[0]["Size"]).ToString();
            }
            return retPth;
        }

        public ActionResult FreeDetails(String id)
        {
            ListCategoryMenu();
            Property p1 = new Property();
            DataSet ds = new DataSet();
            p1.Condition1 = id;
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "FREE_PRODUCT_DETAILS";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            List<SelectListItem> Product = new List<SelectListItem>();

            Product.Add(new SelectListItem { Text = "-SELECT-", Value = "0" });
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Product.Add(new SelectListItem { Text = item["Title"].ToString(), Value = item["id"].ToString() });
            }

            ViewBag.ProductList =new SelectList(Product,"Value","Text");           
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
                    p.ImgURL = ds.Tables[0].Rows[i]["ImgURL"].ToString().Replace("~", "");
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
        //----------------------------Product Details--------------------------//
        public ActionResult Add(string id)
        {
            if (id == null)
            {
                id = "0";
            }
            var info = new Papaspizza.Models.Property();
            Property p1 = new Property();
            DataSet ds = new DataSet();
            p1.onTable = "PRODUCT_ADD";
            p1.Condition1 = id;
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            List<SelectListItem> Category = new List<SelectListItem>();
            List<Property> Size = new List<Property>();
            List<Property> SSize = new List<Property>();
            Category.Add(new SelectListItem { Text = "-SELECT-", Value = "0" });
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Category.Add(new SelectListItem { Text = item["Category"].ToString(), Value = item["id"].ToString() });
            }
            foreach (DataRow item in ds.Tables[1].Rows)
            {
                Property p = new Property();
                p.id = item["id"].ToString();
                p.Size = item["Size"].ToString();
                Size.Add(p);
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                info = new Papaspizza.Models.Property()
                {
                    id = ds.Tables[2].Rows[0]["id"].ToString(),
                    Title = ds.Tables[2].Rows[0]["Title"].ToString(),
                    Price = ds.Tables[2].Rows[0]["Price"].ToString(),
                    Category = ds.Tables[2].Rows[0]["Category"].ToString(),
                    TypeName = ds.Tables[2].Rows[0]["Type"].ToString(),
                    Description = ds.Tables[2].Rows[0]["Description"].ToString(),
                    Cus_ID = ds.Tables[2].Rows[0]["Dip"].ToString(),
                    ImgURL = ds.Tables[2].Rows[0]["ImgURL"].ToString().Replace("~", ""),
                    Status = ds.Tables[2].Rows[0]["Status"].ToString(),

                };
            }
            else
            {
                info = new Property()
                {
                    id = "0",
                    Title = "None",
                    Category = "None",
                    TypeName = "None",
                    Description = "None",
                    Status = "None",
                    ImgURL = "/images/photo_default.png",

                };
            }
            foreach (DataRow item in ds.Tables[3].Rows)
            {
                Property p = new Property();
                p.Size = item["SizeID"].ToString();
                p.Price = item["Price"].ToString();
                SSize.Add(p);
            }
            ViewBag.selectedSizeList = SSize;
            ViewBag.CategoryList = new SelectList(Category, "Value", "Text");
            ViewBag.SizeList = Size;
            return View(info);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Property p, List<HttpPostedFileBase> photos)
        {
            try
            {
                if (String.IsNullOrEmpty(p.id))
                {
                    p.id = "0";
                }
                if (String.IsNullOrEmpty(p.Size_Price)) { p.Size_Price = "0|0"; }
                else
                {
                    p.Size_Price = p.Size_Price.TrimEnd(',');
                }
                string ThumbfileLocation = "";

                string ThumbItemUploadFolderPath = "~/DataImages/Product/";

                foreach (HttpPostedFileBase file in photos)
                {
                    try
                    {
                        if (file.ContentLength == 0)
                            continue;

                        if (file.ContentLength > 0)
                        {

                            ThumbfileLocation = HelperFunctions.ThumbrenameUploadFile(file, ThumbItemUploadFolderPath, 1); //Thumbnail

                            p.ImgURL = ThumbfileLocation;


                        }
                    }
                    catch (Exception ex)
                    {
                        p.ImgURL = "";

                        p.ThumbURL = "";
                        TempData["MSG"] = ex.ToString();
                    }
                }

                if (dl.INSERT_UPDATE_PRODUCT(p) > 0)
                {
                    TempData["MSG"] = "Record Saved Successfully!!";
                    ModelState.Clear();
                }
                else
                {
                    TempData["MSG"] = "Record not Saved";
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = "The data entered already exists!";
            }
            // ListCategory();
            Add(p.id);
            return View();
        }

        [ValidateInput(false)]
        public ActionResult List()
        {

            Property p1 = new Property();
            DataSet ds = new DataSet();
            p1.onTable = "PRODUCT_DETAILS";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            List<Property> Detail = new List<Property>();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Property p = new Property();

                p.id = item["id"].ToString();
                p.Title = item["Title"].ToString();
                p.Category = item["Category"].ToString();
                p.Description = item["Description"].ToString();
                p.ImgURL = item["ImgURL"].ToString().Replace("~", "");
                p.Status = item["Status"].ToString();

                Detail.Add(p);
            }

            ViewBag.DetailList = Detail;

            return View();
        }

        public ActionResult Customise(string Id)
        {
            if (Id == null)
            {
                Id = "0";
            }
          
            Property p1 = new Property();
            DataSet ds = new DataSet();
            p1.onTable = "CUSTOMISE_ADD";
            p1.Condition1 = Id;
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            List<SelectListItem> Product = new List<SelectListItem>();
            List<Property> Item = new List<Property>();
            List<Property> SItem = new List<Property>();

            var info = new Papaspizza.Models.Property();

            Product.Add(new SelectListItem { Text = "-SELECT-", Value = "0" });
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Product.Add(new SelectListItem { Text = item["Title"].ToString(), Value = item["id"].ToString() });
            }
            foreach (DataRow item in ds.Tables[1].Rows)
            {
                Property p = new Property();
                p.id = item["id"].ToString();
                p.ProductName = item["Title"].ToString();
                Item.Add(p);
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                info = new Papaspizza.Models.Property()
               {
                   id = Id,
                  
                   ProductName = ds.Tables[2].Rows[0]["id"].ToString(),
               };
            }
            else
            {
                info = new Property()
                {

                    Title = "0",

                };
            }
            foreach (DataRow item in ds.Tables[3].Rows)
            {
                Property p = new Property();
                p.id = item["CusID"].ToString();
                p.ProductName = item["Title"].ToString();
                SItem.Add(p);
            }
            ViewBag.selectedItemList = SItem;
            ViewBag.ProductList = new SelectList(Product, "Value", "Text", info.Title);
            ViewBag.ItemList = Item;
            return View(info);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Customise(Property p)
        {
            try
            {
                if (String.IsNullOrEmpty(p.id))
                {
                    p.id = "0";
                }
                if (String.IsNullOrEmpty(p.Cus_ID)) { p.Cus_ID = "0"; }
                else
                {
                    p.Cus_ID = p.Cus_ID.TrimEnd(',');
                }
                if (dl.INSERT_UPDATE_CUSTOMISE(p) > 0)
                {
                    TempData["MSG"] = "Record Saved Successfully!!";
                    ModelState.Clear();
                }
                else
                {
                    TempData["MSG"] = "Record not Saved";
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = "The data entered already exists!";
            }

            Customise(p.id);
            return View();
        }
        public ActionResult CusList()
        {

            Property p1 = new Property();
            DataSet ds = new DataSet();
            p1.onTable = "CUSTOMISE_LIST";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            List<Property> Detail = new List<Property>();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Property p = new Property();

                p.id = item["id"].ToString();
                p.Title = item["Title"].ToString();
                p.ProductName = item["Customise"].ToString();

                Detail.Add(p);
            }

            ViewBag.ProductList = Detail;

            return View();
        }
       
     
        //---------Category-------------//

        public ActionResult Category(string id)
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();


            p1.onTable = "CATEGORY_LIST_TABLE";

            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            List<Property> Category = new List<Property>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property p = new Property();

                    p.id = ds.Tables[0].Rows[i]["id"].ToString();
                    p.Category = ds.Tables[0].Rows[i]["Category"].ToString();
                    p.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                    p.ImgURL = ds.Tables[0].Rows[i]["ImgURL"].ToString().Replace("~", "");
                    Category.Add(p);
                }
            }
            else
            {
                Property p = new Property();
                p.id = "0";
                p.Category = "NONE";
                p.Status = "NONE";
                p.ImgURL = "/images/photo_default.png";
                Category.Add(p);
            }

            @ViewBag.CategoryList = Category;

            TempData["brandAdd1"] = "in active";
            TempData["brandAdd11"] = "active";
            if (id != null)
            {
                p1.Condition1 = id;
                p1.Condition2 = "";
                p1.onTable = "CATEGORY_ID";
                DataSet dsbrand = dl.FETCH_CONDITIONAL_QUERY(p1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var info = new Property()
                    {
                        id = dsbrand.Tables[0].Rows[0]["Id"].ToString(),
                        Category = dsbrand.Tables[0].Rows[0]["Category"].ToString(),
                        ImgURL = dsbrand.Tables[0].Rows[0]["ImgURL"].ToString().Replace("~", ""),
                        Status = dsbrand.Tables[0].Rows[0]["Status"].ToString(),
                    };
                    return View(info);
                }
            }
            else
            {
                p1.id = "0";
            }
            return View(p1);
        }


        [HttpPost]
        public ActionResult Category(Property p, List<HttpPostedFileBase> fileUpload)
        {

            Datalayer dl = new Datalayer();
            try
            {
                string fileLocation = "";
                string ItemUploadFolderPath = "~/DataImages/Category/";
                foreach (HttpPostedFileBase file in fileUpload)
                {
                    try
                    {
                        if (file.ContentLength == 0)
                            continue;

                        if (file.ContentLength > 0)
                        {
                            fileLocation = HelperFunctions.ThumbrenameUploadFile(file, ItemUploadFolderPath, 1);
                            p.ImgURL = fileLocation;
                        }
                    }
                    catch (Exception ex)
                    {
                        p.ImgURL = "";
                        TempData["MSG"] = ex.ToString();
                    }
                }
                if (dl.INSERT_UPDATE_CATEGORY(p) > 0)
                {
                    TempData["MSG"] = "Record Saved Successfully!";
                    ModelState.Clear();
                    return Redirect("/Product/Category");
                }
                else
                {
                    TempData["MSG"] = "OOps something went wrong!!..Menu order exits!!";
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = "The data entered already exists!";
            }

            return RedirectToAction("Category");
        }
        //---------Size-----------------------------------------------//

        public ActionResult Size(string id)
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();


            p1.onTable = "SIZE_LIST_TABLE";

            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            List<Property> Size = new List<Property>();


            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Property p = new Property();

                p.id = item["id"].ToString();
                p.Size = item["Size"].ToString();
                p.Status = item["Status"].ToString();

                Size.Add(p);
            }


            @ViewBag.SizeList = Size;

            TempData["brandAdd1"] = "in active";
            TempData["brandAdd11"] = "active";
            if (id != null)
            {
                p1.Condition1 = id;
                p1.Condition2 = "";
                p1.onTable = "SIZE_ID";
                DataSet dsbrand = dl.FETCH_CONDITIONAL_QUERY(p1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var info = new Property()
                    {
                        id = dsbrand.Tables[0].Rows[0]["Id"].ToString(),
                        Size = dsbrand.Tables[0].Rows[0]["Size"].ToString(),
                        Status = dsbrand.Tables[0].Rows[0]["Status"].ToString(),
                    };
                    return View(info);
                }
            }
            else
            {
                p1.id = "0";
            }
            return View(p1);
        }


        [HttpPost]
        public ActionResult Size(Property p)
        {

            Datalayer dl = new Datalayer();
            try
            {

                if (dl.INSERT_UPDATE_SIZE(p) > 0)
                {
                    TempData["MSG"] = "Record Saved Successfully!";
                    ModelState.Clear();
                    return Redirect("/Product/Size");
                }
                else
                {
                    TempData["MSG"] = "OOps something went wrong!!..Menu order exits!!";
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = "The data entered already exists!";
            }

            return RedirectToAction("Size");
        }


        //---------Dip-----------------------------------------------//

        public ActionResult Dip(string id)
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();


            p1.onTable = "DIP_LIST_TABLE";

            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            List<Property> Dip = new List<Property>();


            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Property p = new Property();

                p.id = item["id"].ToString();
                p.Cus_ID = item["Dip"].ToString();
                p.Status = item["Status"].ToString();

                Dip.Add(p);
            }


            @ViewBag.DipList = Dip;

            TempData["brandAdd1"] = "in active";
            TempData["brandAdd11"] = "active";
            if (id != null)
            {
                p1.Condition1 = id;
                p1.Condition2 = "";
                p1.onTable = "DIP_ID";
                DataSet dsbrand = dl.FETCH_CONDITIONAL_QUERY(p1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var info = new Property()
                    {
                        id = dsbrand.Tables[0].Rows[0]["Id"].ToString(),
                        Cus_ID = dsbrand.Tables[0].Rows[0]["Dip"].ToString(),
                        Status = dsbrand.Tables[0].Rows[0]["Status"].ToString(),
                    };
                    return View(info);
                }
            }
            else
            {
                p1.id = "0";
            }
            return View(p1);
        }


        [HttpPost]
        public ActionResult Dip(Property p)
        {

            Datalayer dl = new Datalayer();
            try
            {

                if (dl.INSERT_UPDATE_DIP(p) > 0)
                {
                    TempData["MSG"] = "Record Saved Successfully!";
                    ModelState.Clear();
                    return Redirect("/Product/Dip");
                }
                else
                {
                    TempData["MSG"] = "OOps something went wrong!!..Menu order exits!!";
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = "The data entered already exists!";
            }

            return RedirectToAction("Dip");
        }
        //-----------------------------------------------Free Items---------------------------------------------//
        public ActionResult Free(String Id)
        {
            if (Id == null)
            {
                Id = "0";
            }
            var info = new Papaspizza.Models.Property();
            Property p1 = new Property();
            DataSet ds = new DataSet();
           
            p1.onTable = "FREE_PRODUCT_ADD";
            p1.Condition1 = Id;
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            List<SelectListItem> Category = new List<SelectListItem>();

            Category.Add(new SelectListItem { Text = "-SELECT-", Value = "0" });
            foreach (DataRow item in ds.Tables[1].Rows)
            {
                Category.Add(new SelectListItem { Text = item["Category"].ToString(), Value = item["id"].ToString() });
            }
            ViewBag.CategoryList = new SelectList(Category, "Value", "Text");

            if (ds.Tables[2].Rows.Count > 0)
            {
                info = new Papaspizza.Models.Property()
                {
                    id = ds.Tables[2].Rows[0]["id"].ToString(),
                    Title = ds.Tables[2].Rows[0]["Title"].ToString(),
                   
                    Category = ds.Tables[2].Rows[0]["ItemID"].ToString(),
                    Qty = ds.Tables[2].Rows[0]["Qty"].ToString(),
                    Item_Free = ds.Tables[2].Rows[0]["FreeQty"].ToString(),
                    Description = ds.Tables[2].Rows[0]["Description"].ToString(),
                    ImgURL = ds.Tables[2].Rows[0]["ImgURL"].ToString().Replace("~", ""),
                    Status = ds.Tables[2].Rows[0]["Status"].ToString(),

                };
            }
            else
            {
                info = new Property()
                {
                    id = "0",
                    Title = "None",
                    Category = "0",
                  Qty="1",
                  Item_Free="1",
                
                    Description = "None",
                    Status = "ACTIVE",
                    ImgURL = "/images/photo_default.png",

                };
            }
          
            return View(info);
        }


        [HttpPost]
        public ActionResult Free(Property p, List<HttpPostedFileBase> photos)
        {

            Datalayer dl = new Datalayer();
            try
            {
                p.TypeName = "FREE";
                if (String.IsNullOrEmpty(p.id))
                {
                    p.id = "0";
                }
               
                string fileLocation = "";
                string ItemUploadFolderPath = "~/DataImages/Free/";
                foreach (HttpPostedFileBase file in photos)
                {
                    try
                    {
                        if (file.ContentLength == 0)
                            continue;

                        if (file.ContentLength > 0)
                        {
                            fileLocation = HelperFunctions.ThumbrenameUploadFile(file, ItemUploadFolderPath, 1);
                            p.ImgURL = fileLocation;
                        }
                    }
                    catch (Exception ex)
                    {
                        p.ImgURL = "";
                        TempData["MSG"] = ex.ToString();
                    }
                }
                if (dl.INSERT_FREE(p) > 0)
                {
                    TempData["MSG"] = "Record Saved Successfully!!";
                    ModelState.Clear();
                }
                else
                {
                    TempData["MSG"] = "Record not Saved";
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = "The data entered already exists!";
            }

            return Redirect("/Product/Free");
        }
        public ActionResult FreeList()
        {
            Property p1 = new Property();
            DataSet ds = new DataSet();
            p1.onTable = "FREE_PRODUCT_LIST";

            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            List<Property> Menu = new List<Property>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Property p = new Property();

                    p.id = item["id"].ToString();
                    p.Title = item["Title"].ToString();
                   
                    p.ImgURL = item["ImgURL"].ToString().Replace("~", "");
                    p.Description = item["Description"].ToString();
                    
                    Menu.Add(p);
                }
            }
            ViewBag.ProductList = Menu;
            return View();
        }
        public ActionResult Delete()
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();

            string str = "";


            p1.id = Request.QueryString["id"].ToString();
            p1.Condition1 = Request.QueryString["id"].ToString();
            p1.Condition2 = "";
            p1.Condition3 = "";

            if (Request.QueryString["type"].ToString() == "Category")
            {


                str = "DELETE FROM tbl_Category WHERE id='" + p1.id + "' ";

            }
            if (Request.QueryString["type"].ToString() == "Size")
            {


                str = "DELETE FROM tbl_Size WHERE id='" + p1.id + "' ";

            }
            if (Request.QueryString["type"].ToString() == "Dip")
            {


                str = "DELETE FROM tbl_Dip WHERE id='" + p1.id + "' ";

            }
            else if (Request.QueryString["type"].ToString() == "Product")
            {

                str = "DELETE FROM tbl_Product WHERE id='" + p1.id + "'";


            }
            else if (Request.QueryString["type"].ToString() == "Spe")
            {

                str = "DELETE FROM tbl_Menu WHERE id='" + p1.id + "'";


            }
            else if (Request.QueryString["type"].ToString() == "Free")
            {

                str = "DELETE FROM tbl_Menu WHERE id='" + p1.id + "'";


            }
            else
            {
                p1.onTable = "";
                str = "";

            }

            SqlConnection con = new SqlConnection(p1.Con);
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(str, con);
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    TempData["MSG"] = "Data Deleted Successfully!!!";
                }
                else
                {
                    TempData["MSG"] = "Record Not Deleted.";
                }

            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }
            con.Close();


            if (Request.QueryString["type"].ToString() == "Category")
            {
                return RedirectToAction("Category", "Product");
            }
            if (Request.QueryString["type"].ToString() == "Size")
            {
                return RedirectToAction("Size", "Product");
            }
            if (Request.QueryString["type"].ToString() == "Dip")
            {
                return RedirectToAction("Dip", "Product");
            }
            else if (Request.QueryString["type"].ToString() == "Product")
            {
                return RedirectToAction("List", "Product");

            }
            else if (Request.QueryString["type"].ToString() == "Spe")
            {
                return RedirectToAction("SpecialList", "Product");

            }
            else if (Request.QueryString["type"].ToString() == "Free")
            {
                return RedirectToAction("FreeList", "Product");

            }

            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }

        //-------------------------------------------------------------------------Special Product------------------------------------------//
        public ActionResult Special(string id)
        {
            try
            {
               
                if (id == null)
                {
                    id = "0";
                }

                var info = new Papaspizza.Models.Property();
                Property p1 = new Property();
                DataSet ds = new DataSet();
                p1.onTable = "SPECIAL_PRODUCT_ADD";
                p1.Condition1 = id;
                ds = dl.FETCH_CONDITIONAL_QUERY(p1);
                List<SelectListItem> Size = new List<SelectListItem>();
                List<Property> Category = new List<Property>();
                List<Property> SCategory = new List<Property>();
                Size.Add(new SelectListItem { Text = "-SELECT-", Value = "0" });
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Size.Add(new SelectListItem { Text = item["Size"].ToString(), Value = item["id"].ToString() });
                }
                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    Property p = new Property();
                    p.id = item["id"].ToString();
                    p.Category = item["Category"].ToString();
                    Category.Add(p);
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    info = new Papaspizza.Models.Property()
                    {
                        id = ds.Tables[2].Rows[0]["id"].ToString(),
                        Title = ds.Tables[2].Rows[0]["Title"].ToString(),
                        Price = ds.Tables[2].Rows[0]["Price"].ToString(),
                        Description = ds.Tables[2].Rows[0]["Description"].ToString(),
                        ImgURL = ds.Tables[2].Rows[0]["ImgURL"].ToString().Replace("~", ""),
                        Status = ds.Tables[2].Rows[0]["Status"].ToString(),

                    };
                }
                else
                {
                    info = new Property()
                    {
                        id = "0",
                        Title = "None",
                        Description = "None",
                        Status = "None",
                        ImgURL = "/images/photo_default.png",

                    };
                }
                foreach (DataRow item in ds.Tables[3].Rows)
                {
                    Property p = new Property();
                    p.Category = item["ItemID"].ToString();
                    p.Size = item["ItemSize"].ToString();
                    p.Qty = item["ItemQty"].ToString();
                    SCategory.Add(p);
                }
                ViewBag.selectedCategoryList = SCategory;
                ViewBag.SizeList = new SelectList(Size, "Value", "Text");
                ViewBag.CategoryList = Category;
                return View(info);
            }
            catch (Exception ex)
            {
                TempData["MSG"] = "The data entered already exists!";
            }
            return View();
        }       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Special(Property p, List<HttpPostedFileBase> photos)
        {
            try
            {
                p.TypeName = "SPECIAL";
                if (String.IsNullOrEmpty(p.id))
                {
                    p.id = "0";
                }
                if (String.IsNullOrEmpty(p.Menu_Item)) { p.Menu_Item = "0,0,0"; }
                else
                {
                    p.Menu_Item = p.Menu_Item.TrimEnd('|');
                }
               
              
                string ThumbfileLocation = "";
                string ThumbItemUploadFolderPath = "~/DataImages/Menu/";
                foreach (HttpPostedFileBase file in photos)
                {
                    try
                    {
                        if (file.ContentLength == 0)
                            continue;

                        if (file.ContentLength > 0)
                        {

                            ThumbfileLocation = HelperFunctions.ThumbrenameUploadFile(file, ThumbItemUploadFolderPath, 1); //Thumbnail
                            p.ImgURL = ThumbfileLocation;
                        }
                    }
                    catch (Exception ex)
                    {
                        p.ImgURL = "";
                        p.ThumbURL = "";
                        TempData["MSG"] = ex.ToString();
                    }
                }
                if (p.MID == null)
                {
                    p.MID = "0";
                }
                if (dl.INSERT_MENU(p) > 0)
                {
                    TempData["MSG"] = "Record Saved Successfully!!";
                    ModelState.Clear();
                }
                else
                {
                    TempData["MSG"] = "Record not Saved";
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = "The data entered already exists!";
            }

            return Redirect("/Product/Special");
        }


        public ActionResult SpecialList()
        {
            Property p1 = new Property();
            DataSet ds = new DataSet();
            p1.onTable = "SPECIAL_PRODUCT_LIST";

            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            List<Property> Menu = new List<Property>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Property p = new Property();

                    p.id = item["id"].ToString();
                    p.Title =item["Title"].ToString();                   
                    p.Price = item["Price"].ToString();
                    p.ImgURL = item["ImgURL"].ToString().Replace("~", "");
                    p.Description = item["Description"].ToString();
                    p.ProductName = item["Item"].ToString();
                    Menu.Add(p);
                }
            }           
            ViewBag.ProductList = Menu;
            return View();
        }






        [AllowAnonymous]
        public string CustomProduct(string ID)
        {
            string retPth = "Not Customise";
            Property p = new Property();
            SqlConnection con = new SqlConnection(p.Con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("Select id,ProID,CusID from [dbo].[tbl_Cus_Pro] where ProID='" + ID + "'", con);


            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                retPth = "Customise";
            }
            return retPth;
        }
        
    }
}
