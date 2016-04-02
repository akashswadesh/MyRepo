using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Papaspizza.Models
{
    public class Property
    {

      private string con = "data source=166.62.42.160,1986;user id=PapasPizza49lu;password=12345;initial catalog=PapasPizza";
       // private string con = "data source=CHUMKI-PC;user id=sa;password=sql@2012;initial catalog=PapasPizza";
        //private string con = "data source=MUNNA-PC\\MUNNA2012;user id=sa;password=sql@2012;initial catalog=PapasPizza";    
       // private string con = "data source=HARRY-PC\\HARRY;user id=sa;password=sql@2012;initial catalog=PapasPizza";
        public string Con
        {
            get
            {
                return con;
            }
        }
        public string DealDesc { get; set; } 
        public string id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string CnfPassword { get; set; }
        public string FileLocation { get; set; }

        public string MID { get; set; }

        public string Cus_ID { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public string EmailID { get; set; }
        public string City { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string Size { get; set; }
        public string Size_Price { get; set; }
        public string Description { get; set; }

        public string ThumbURL { get; set; }
        public string ImgURL { get; set; }
        public string ImgURL2 { get; set; }

        public string Condition1 { get; set; }
        public string Condition2 { get; set; }
        public string Condition3 { get; set; }
        public string onTable { get; set; }
        public string InstanceID { get; set; }
        public string SubCatId { get; set; }
        public string SmallDesc { get; set; }
        public string FullDesc { get; set; }
        public string TypeName { get; set; }

        public string CatId { get; set; }
        public string Page { get; set; }
        public string Date { get; set; }

        public string LinkURL { get; set; }
        public string onDate { get; set; }

        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }

        public string Country { get; set; }
        public string ProductName { get; set; }
        public string ProId { get; set; }
        public string Company { get; set; }
        public string userid { get; set; }
        public string CartID { get; set; }
        public string Qty { get; set; }
        public string Amount{ get; set; }
        public string TotalAmount { get; set; }

        public string Menu_Item { get; set; }
        public string  Item_Offered { get; set; }
        public string Item_Free { get; set; }
        public bool IsGroup { get; set; }
      
    }

    public class SizeKV
    {
        public string key { get; set; }
        public string value { get; set; }


    }
}