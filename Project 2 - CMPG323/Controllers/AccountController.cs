using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_2___CMPG323.Models;

namespace Project_2___CMPG323.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(UserDetail user)
		{
            bool Status = false;
            string message = String.Empty;

            //Model validation
            if(ModelState.IsValid)
			{
                //Email already exist
                var isExist = IsEmailExist(user.Email);
                if(isExist)
				{
                    ModelState.AddModelError("Email", "Email already exist");
                    return View(user);
				}

            }
            else
			{
                message = "Invalid request!";
			}


            //Password hashing
            user.Password = Crypto.Hash(user.Password);
            user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);

            //Save data to DB

            using(AlbumEntities1 DB = new AlbumEntities1())
			{
                DB.UserDetails.Add(user);
                DB.SaveChanges();
			}


            return View(user);
		}

        [NonAction]
        public Boolean IsEmailExist(string email)
		{
            using(AlbumEntities1 DB = new AlbumEntities1())
			{
                var tmp = DB.UserDetails.Where(a => a.Email == email).FirstOrDefault();
                return tmp == null ? false : true; 
			}
		}
    }
}