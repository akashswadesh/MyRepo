using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Papaspizza.Helpers;
using Papaspizza.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;

namespace Papaspizza.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        EncryptDecrypt enc = new EncryptDecrypt();
        Datalayer dl = new Datalayer();

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            HttpCookie loginCookie = Request.Cookies["login_cookie"];
            if (loginCookie != null)
            {
                loginCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(loginCookie);
            }
            HttpCookie lockingCookie = Request.Cookies["locking_cookie"];
            if (lockingCookie != null)
            {
                lockingCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(lockingCookie);
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Property p)
        {
            DataSet ds = new DataSet();

            ds = dl.FETCH_LOGIN_DETAILS(p);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if(ds.Tables[0].Rows[0]["UserType"].ToString() == "ADMIN")
                {

                    HttpCookie loginCookie = Request.Cookies["login_cookie"];
                    if (loginCookie == null)
                    {
                        loginCookie = new HttpCookie("login_cookie");
                        
                        loginCookie["UserType"] = ds.Tables[0].Rows[0]["UserType"].ToString();// "ADMIN";
                        loginCookie.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Add(loginCookie);
                    }
                    else
                    {
                        loginCookie.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(loginCookie);
                        loginCookie = new HttpCookie("login_cookie");
                        
                        loginCookie["UserType"] = ds.Tables[0].Rows[0]["UserType"].ToString();// "ADMIN";
                        loginCookie.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Add(loginCookie);
                    }

                    return Redirect("/Admin");

                }

              
                else
                {
                    TempData["MSG"] = "Your Account is not activated!";
                    return View();
                }
            }
            else
            {
                TempData["MSG"] = "Email ID or Password is incorrect!";
                return View();
            }
        }


        public ActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Register(Property p)
        {
            DataSet ds = new DataSet();
            try
            {
               
                p.id = "0";
                ds = dl.INSERT_UPDATE_USER_REGISTRATION(p);

                if (ds.Tables[0].Rows.Count > 0)
                {
                   
                    TempData["MSG"] = "Registration Successfull!";
                    return Redirect("/Account");
                }
                else
                {
                    TempData["MSG"] = "Problem in registration! Please Visit Again.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
                return View();
            }
        }


        


      

        public ActionResult Forget_Password()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forget_Password(Property p)
        {
            DataSet ds = new DataSet();
            p.Condition1 = p.EmailID;
            p.Condition2 = "";
            p.onTable = "FORGET_PASS";
            ds = dl.FETCH_CONDITIONAL_QUERY(p);

            if (ds.Tables[0].Rows.Count > 0)
            {
                sendPasswordMail(ds.Tables[0].Rows[0]["FullName"].ToString(), ds.Tables[0].Rows[0]["EmailID"].ToString(),ds.Tables[0].Rows[0]["Password"].ToString());
                //TempData["MSG"] = "Check your mail. Your password send to your E-mail ID.";
            }
            else
            {
                TempData["MSG"] = "Email ID does not exists.";
            }
            return View();
        }

        public void sendPasswordMail(string fName, string Email, string pass)
        {
            try
            {
                MailAddress mailfrom = new MailAddress("enquiry@hbitm.in", "E-MotionYoga.com");
                MailAddress mailto = new MailAddress(Email);

                MailMessage newmsg = new MailMessage(mailfrom, mailto);

                newmsg.Subject = "Your Login Details";
                newmsg.Body = "Dear " + fName + Environment.NewLine + Environment.NewLine + "Your Login details for E-MotionYoga is:" + Environment.NewLine + Environment.NewLine + " Email ID : " + Email + Environment.NewLine + Environment.NewLine + "Password : " + pass + Environment.NewLine + Environment.NewLine + "For login, Click this link http://demo19.gowebbi.in/Account";
                SmtpClient smtp = new SmtpClient("smtpout.secureserver.net", 25);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("enquiry@hbitm.in", "Gowebbi@123");
                smtp.EnableSsl = false;
                smtp.Send(newmsg);

                TempData["MSG"] = "Check your mail. Your password send to your E-mail ID.";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }
        }
    }
}
