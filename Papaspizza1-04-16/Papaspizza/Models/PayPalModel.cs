using System.Configuration;
namespace PaypalMVC.Models
{
    public class PayPalModel
    {
        public string cmd { get; set; }

        public string business { get; set; }
        public string no_shipping { get; set; }
        public string @return { get; set; }
        public string cancel_return { get; set; }
        public string notify_url { get; set; }
        public string currency_code { get; set; }
        public string item_name { get; set; }
        public string amount { get; set; }
        public string actionURL { get; set; }
        public string Paypal { get; set; }
        public string card { get; set; }
        public string subtotal { set; get; }
        //---------------------------------------------------
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string ContactNo { get; set; }
        //public string Email { get; set; }
        //public string BillingAddress { get; set; }
        //public string ShippingAddress { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string Zip { get; set; }
        //public string Country { get; set; }
        //public string Company { get; set; } 
        //public string Address { get; set; }
        //  public string EmailID { get; set; }
        //  public string Fax { get; set; }
        //  public string Contact { get; set; }

        //public string BFirstName { get; set; }
        //public string BLastName { get; set; }
        //public string CEmail { get; set; }
        //public string CContact { get; set; }
        //public string BCity { get; set; }
        //public string BState { get; set; }
        //public string BZip { get; set; }
        //public string BCountry { get; set; }
        //--------------------------------------------------
       // public string cmd { get; set; }
      //  public string business { get; set; }
        
        public PayPalModel(bool useSandbox)
        {
            this.cmd = "_xclick";
            this.business = ConfigurationManager.AppSettings["business"];
            this.cancel_return = ConfigurationManager.AppSettings["cancel_return"];
            this.@return = ConfigurationManager.AppSettings["return"];
            if (useSandbox)
            {
                this.actionURL = ConfigurationManager.AppSettings["test_url"];
            }
            else
            {
                this.actionURL = ConfigurationManager.AppSettings["Prod_url"];
            }
            // We can add parameters here, for example OrderId, CustomerId, etc....
            this.notify_url = ConfigurationManager.AppSettings["notify_url"];
            // We can add parameters here, for example OrderId, CustomerId, etc....
            this.currency_code = ConfigurationManager.AppSettings["currency_code"];
        }
    }
}