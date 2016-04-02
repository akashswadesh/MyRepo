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
    public class PostcodeController : Controller
    {
        //
        // GET: /Postcode/

        Datalayer dl = new Datalayer();
        public ActionResult Index(string id)
        {
            Property p = new Property();
            p.Condition1 = "";
            p.Condition2 = "";
            p.onTable = "FETCH_ALL_POSTCODE";
            DataSet ds = dl.FETCH_CONDITIONAL_QUERY(p);
            List<Property> brandlist = new List<Property>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Property pp = new Property();
                    pp.id = item["Id"].ToString();
                    pp.Page = item["Postcode"].ToString();
                    
                    pp.Status = item["Status"].ToString();
                    brandlist.Add(pp);
                }
            }
            ViewBag.Contentlist = brandlist;

            TempData["brandAdd1"] = "in active";
            TempData["brandAdd11"] = "active";

            if (id != null)
            {
                p.Condition1 = id;
                p.Condition2 = "";
                p.onTable = "POSTCODE_BY_ID";
                DataSet dsbrand = dl.FETCH_CONDITIONAL_QUERY(p);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var info = new Property()
                    {
                        id = dsbrand.Tables[0].Rows[0]["Id"].ToString(),
              Page = dsbrand.Tables[0].Rows[0]["Postcode"].ToString(),
                      
                        Status = dsbrand.Tables[0].Rows[0]["Status"].ToString(),
                    };
                    return View(info);
                }
            }
            else
            {
                p.id = "0";
            }
            return View(p);
        }

        [HttpPost]
        public ActionResult Index(Property p)
        {
            DataSet ds = new DataSet();


            if (dl.INSER_UPDATE_POSTCODE(p) > 0)
            {
                ModelState.Clear();
                TempData["MSG"] = "Data Submitted Successfully!!!";
                return Redirect("/Postcode/");

            }
            else
            {
                TempData["MSG"] = "Some thing Wrong !!!";
                return View("Index", p);
            }
        }
        public ActionResult DeletePos(string id)
        {

            DataSet ds = new DataSet();
            Property p1 = new Property();

            string str = "";


            str = "DELETE FROM Tbl_Postcode WHERE id='" + id + "'";



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

            return RedirectToAction("Index", "Postcode");
        }


        [HttpPost]
        public string Check(string value)
        {
           
                Property p = new Property();
                SqlConnection con = new SqlConnection(p.Con);

                SqlCommand cmd = new SqlCommand("select * from Tbl_Postcode where Postcode='" + value + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "Yes! We can Deliver.";
                }
                else { return "Sorry!We Can't Deliver"; }



        }
    }
}
