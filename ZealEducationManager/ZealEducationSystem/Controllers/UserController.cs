using Microsoft.AspNetCore.Mvc;
using ZealEducationSystem.Data;
using ZealEducationSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ZealEducationSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly ZealEducationTestContext db;
        private readonly IHttpContextAccessor contx;
        public UserController(ILogger<UserController> logger, ZealEducationTestContext db, IHttpContextAccessor contx)
        {
            _logger = logger;
            this.db = db;
            this.contx = contx;
        }
        public IActionResult Index()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;
            return View();
        }
        //Login work
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(SignupDetail usercheck)
        {
            var userfind = db.SignupDetails.FirstOrDefault(x => x.UserEmail == usercheck.UserEmail);

            var frontEmail = usercheck.UserEmail;
            var frontPass = usercheck.UserPassword;
            var frontRole = "user";

            var dbEmail = userfind?.UserEmail;
            var dbPass = userfind?.UserPassword;
            var dbRole = userfind?.UserRole;
            var dbID = userfind.UserId;

            contx.HttpContext.Session.SetString("UserEmail", dbEmail);
            contx.HttpContext.Session.SetString("UserPassword", dbPass);
            contx.HttpContext.Session.SetString("UserRole", dbRole);
            contx.HttpContext.Session.SetInt32("UserId", dbID);




            if
                (
                    frontEmail == dbEmail &&
                    frontPass == dbPass &&
                    frontRole == dbRole
                )
            {
                ViewBag.Role = frontRole;

                if (frontRole == "user")
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }


            return View("Login");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
        
       
        [HttpPost]
        public IActionResult Signup(SignupDetail user)
        {
            var userrole = new SignupDetail()
            {
                UserEmail = user.UserEmail,
                UserPassword = user.UserPassword,
                UserRole = user.UserRole,
            };
            db.SignupDetails.Add(userrole);
            db.SaveChanges();
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public IActionResult Enquiry()
        {

            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;
            return View();
        }


        [HttpPost]
        public IActionResult Enquiry(Enquiry Enquiryuser)
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            var enquser = new Enquiry()
            {
                FirstName = Enquiryuser.FirstName,
                LastName = Enquiryuser.LastName,
                EnqEmail = Enquiryuser.EnqEmail,
                EnqMessage=Enquiryuser.EnqMessage
            };
            db.Enquiries.Add(enquser);
            db.SaveChanges();
            return RedirectToAction("Enquiry");
        }
        public IActionResult Course(int courseId)
        {

            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            return View(db.Courses.ToList());
            
        }
        public IActionResult About()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            return View();

        }
        [HttpGet]
        public IActionResult Profile()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");
            var idcheck = contx.HttpContext.Session.GetInt32("UserId");
            ViewBag.Id = idcheck;

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;
            ViewBag.Pass = passcheck;
            return View();
        }
        public IActionResult ProfileEdit(int id)
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;
            ViewBag.Pass = passcheck;

            var record = db.SignupDetails.Find(id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        // Action method to handle the form submission and update the record
        [HttpPost]
        public IActionResult ProfileUpdate(SignupDetail model)
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;
            ViewBag.Pass = passcheck;

            if (ModelState.IsValid)
            {
                db.Update(model); // Update the record in the database
                db.SaveChanges(); // Save changes

                return RedirectToAction("Profile", "User"); // Redirect to another action after successful update
            }

            // Handle validation errors
            return View("ProfileEdit", model);
        }




        public IActionResult Logout()
        {

            contx.HttpContext.Session.Clear();



            return RedirectToAction("Login", "User");

        }
        public IActionResult CourseDetails()
        {
            return View(db.Courses.ToList());

        }

    }
}
