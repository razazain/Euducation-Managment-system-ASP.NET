using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using ZealEducationSystem.Data;
using ZealEducationSystem.Models;

namespace ZealEducationSystem.Controllers
{
    public class FacultypanelController : Controller
    {
        private readonly ILogger<FacultypanelController> _logger;
        private readonly ZealEducationTestContext db;
        private readonly IHttpContextAccessor contx;
        public FacultypanelController(ILogger<FacultypanelController> logger, ZealEducationTestContext db, IHttpContextAccessor contx)
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
            return View(db.Enquiries.ToList());
        }
        public IActionResult EnqDelete(int id)
        {
            Enquiry userDelete = db.Enquiries.Find(id);
            db.Enquiries.Remove(userDelete);
            db.SaveChanges();

            return RedirectToAction("Index","FacultyPanel");
        }
        public IActionResult Profile()
        {

            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            var idcheck = contx.HttpContext.Session.GetInt32("UserId");
            ViewBag.ID = idcheck;
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

            var idcheck = contx.HttpContext.Session.GetInt32("UserId");
            ViewBag.ID = idcheck;
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

                return RedirectToAction("Profile", "Facultypanel"); // Redirect to another action after successful update
            }

            // Handle validation errors
            return View("ProfileEdit", model);
        }


        [HttpGet]
        public IActionResult Signup()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Signup(SignupDetail signup)
        {
            var newrole = new SignupDetail()
            {
                UserEmail = signup.UserEmail,
                UserPassword = signup.UserPassword,
                UserRole = signup.UserRole,
            };
            db.SignupDetails.Add(newrole);
            db.SaveChanges();
            return RedirectToAction("Home", "Login");
        }

        public IActionResult Logout()
        {

            contx.HttpContext.Session.Clear();



            return RedirectToAction("Login", "Home");

        }

        [HttpGet]
        public IActionResult Show_Student()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;


            return View(db.Candidates.Include(x => x.Course).Include(x => x.Batch).ToList());



        }
        public IActionResult CandidateDelete(int id)
        {
            Candidate userDelete = db.Candidates.Find(id);
            db.Candidates.Remove(userDelete);
            db.SaveChanges();

            return RedirectToAction("Show_Student", "Facultypanel");
        }
        public IActionResult CandidateEdit(int id)
        {

            var courses = db.Courses.ToList();
            var courseList = new SelectList(courses, "CourseId", "CourseName");
            ViewBag.CourseId = courseList;

            var batchess = db.Batchesses.ToList();
            var batchessList = new SelectList(batchess, "BatchId", "BatchCode");
            ViewBag.BatchId = batchessList;



            var record = db.Candidates.Find(id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }


        [HttpPost]
        public IActionResult CandidateUpdate(Candidate model)
        {
            if (ModelState.IsValid)
            {
                db.Update(model);
                db.SaveChanges();

                return RedirectToAction("Show_Student", "FacultyPanel");
            }

            return View("CandidateEdit", model);
        }




        [HttpGet]
        public IActionResult Add_Student()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            var courses = db.Courses.ToList();
            var courseList = new SelectList(courses, "CourseId", "CourseName");
            ViewBag.CourseId = courseList;


            var batches = db.Batchesses.ToList();
            var batchessList = new SelectList(batches, "BatchId", "BatchCode");
            ViewBag.BatchId = batchessList;



            return View();
        }
        [HttpPost]
        public IActionResult Add_Student(Candidate candidate, IFormFile candidateimage)
        {
            if (candidateimage != null && candidateimage.Length > 0)
            {
                var fileExt = System.IO.Path.GetExtension(candidateimage.FileName).Substring(1);
                var random = Path.GetFileName(candidateimage.FileName);
                var FileName = Guid.NewGuid() + random;

                string imgFolder = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/candidateimage");

                if (!Directory.Exists(imgFolder))
                {
                    Directory.CreateDirectory(imgFolder);
                }
                string filepath = Path.Combine(imgFolder, FileName);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    candidateimage.CopyTo(stream);
                }
                var dbAddress = Path.Combine("candidateimage", FileName);
                candidate.CandidateImage = dbAddress;
                db.Candidates.Add(candidate);
                db.SaveChanges();
            }

            return RedirectToAction("Add_Student","FacultyPanel");

        }




        [HttpGet]
        public IActionResult Show_Batch()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            return View(db.Batchesses.Include(x => x.Course).Include(x => x.Faculty).ToList());
        }

        public IActionResult BatchDelete(int id)
        {
            Batchess Delete = db.Batchesses.Find(id);
            db.Batchesses.Remove(Delete);
            db.SaveChanges();


            return RedirectToAction("Show_Batch","Facultypnale");
        }
        public IActionResult BatchEdit(int id)
        {

            var courses = db.Courses.ToList();
            var courseList = new SelectList(courses, "CourseId", "CourseName");
            ViewBag.CourseId = courseList;

            var faculty = db.Faculties.ToList();
            var facultyList = new SelectList(faculty, "FacultyId", "LastName");
            ViewBag.FacultyId = facultyList;


            var record = db.Batchesses.Find(id);
            //var record = db.Batchesses.Find(id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }


        [HttpPost]
        public IActionResult BatchUpdate(Batchess model)
        {
            if (ModelState.IsValid)
            {
                db.Update(model);
                db.SaveChanges();

                return RedirectToAction("Show_Batch", "Facultypanel");
            }

            return View("BatchEdit", model);
        }







        [HttpGet]
        public IActionResult Add_Batch()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            var courses = db.Courses.ToList();
            var courseList = new SelectList(courses, "CourseId", "CourseName");
            ViewBag.CourseId = courseList;



            var faculty = db.Faculties.ToList();
            var facultyList = new SelectList(faculty, "FacultyId", "LastName");
            ViewBag.FacultyId = facultyList;

            return View();
        }
        [HttpPost]
        public IActionResult Add_Batch(Batchess batch)
        {
            var addbatch = new Batchess()
            {
                BatchCode = batch.BatchCode,
                BatchTime = batch.BatchTime,
                BatchDays = batch.BatchDays,
                BatchStartDate = batch.BatchStartDate,
                BatchDuration = batch.BatchDuration,
                CourseId = batch.CourseId,
                FacultyId = batch.FacultyId
            };
            db.Batchesses.Add(addbatch);
            db.SaveChanges();
            return RedirectToAction("Add_Batch","Facultypanel");
        }


        [HttpGet]
        public IActionResult Add_Course()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            return View();
        }
        [HttpPost]
        public IActionResult Add_Course(Course coursee, IFormFile coursethumbnail)
        {

            if (coursethumbnail != null && coursethumbnail.Length > 0)
            {
                var fileExt = System.IO.Path.GetExtension(coursethumbnail.FileName).Substring(1);
                var random = Path.GetFileName(coursethumbnail.FileName);
                var FileName = Guid.NewGuid() + random;

                string imgFolder = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/coursethumbnail");

                if (!Directory.Exists(imgFolder))
                {
                    Directory.CreateDirectory(imgFolder);
                }
                string filepath = Path.Combine(imgFolder, FileName);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    coursethumbnail.CopyTo(stream);
                }
                var dbAddress = Path.Combine("coursethumbnail", FileName);

                coursee.CourseThumbnail = dbAddress;
                db.Courses.Add(coursee);
                db.SaveChanges();

            }


            return RedirectToAction("Add_Course", "Facultypanel");
        }

        [HttpGet]
        public IActionResult Show_Course()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;

            ViewBag.Email = emailcheck;
            return View(db.Courses.ToList());
        }

        public IActionResult CourseDelete(int id)
        {
            Course Delete = db.Courses.Find(id);
            db.Courses.Remove(Delete);
            db.SaveChanges();
            return RedirectToAction("Show_Course", "Facultypanel");
        }





        //update record
        public IActionResult CourseEdit(int id)
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            var record = db.Courses.Find(id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        // Action method to handle the form submission and update the record
        [HttpPost]
        public IActionResult CourseUpdate(Course model)
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            if (ModelState.IsValid)
            {
                db.Update(model); // Update the record in the database
                db.SaveChanges(); // Save changes

                return RedirectToAction("Show_Course", "Facultypanel"); // Redirect to another action after successful update
            }

            // Handle validation errors
            return View("CourseEdit", model);
        }



        [HttpGet]
        public IActionResult Add_Exam()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;


            var courses = db.Courses.ToList();
            var courseList = new SelectList(courses, "CourseId", "CourseName");
            ViewBag.CourseId = courseList;

            var typeexam = db.ExamTypes.ToList();
            var typeexamList = new SelectList(typeexam, "ExamtypeId", "ExamType1");
            ViewBag.ExamtypeId = typeexamList;

            var batches = db.Batchesses.ToList();
            var batchessList = new SelectList(batches, "BatchId", "BatchCode");
            ViewBag.BatchId = batchessList;
            return View();
        }
        [HttpPost]
        public IActionResult Add_Exam(Exam newexam)
        {
            var examadd = new Exam()
            {
                ExamDate = newexam.ExamDate,
                ExamTime = newexam.ExamTime,
                ExaxmName = newexam.ExaxmName,
                CourseId = newexam.CourseId,
                ExamtypeId = newexam.ExamtypeId,
                BatchId = newexam.BatchId
            };
            db.Exams.Add(examadd);
            db.SaveChanges();
            return RedirectToAction("Add_Exam", "Facultypanel");
        }
        [HttpGet]
        public IActionResult Show_Exam()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            return View(db.Exams.Include(x => x.Course).Include(x => x.Examtype).Include(X => X.Batch).ToList());
        }

        public IActionResult ExamDelete(int id)
        {

            Exam Delete = db.Exams.Find(id);
            db.Exams.Remove(Delete);
            db.SaveChanges();
            return RedirectToAction("Show_Exam", "Facultypanel");
        }





        //update record
        public IActionResult ExamEdit(int id)
        {
            var Examtypee = db.ExamTypes.ToList();
            var ExamtypeList = new SelectList(Examtypee, "ExamtypeId", "ExamType1");
            ViewBag.ExamtypeId = ExamtypeList;


            var courses = db.Courses.ToList();
            var courseList = new SelectList(courses, "CourseId", "CourseName");
            ViewBag.CourseId = courseList;

            var batchess = db.Batchesses.ToList();
            var batchessList = new SelectList(batchess, "BatchId", "BatchCode");
            ViewBag.BatchId = batchessList;



            var record = db.Exams.Find(id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        [HttpPost]
        public IActionResult ExamUpdate(Exam model)
        {


            if (ModelState.IsValid)
            {
                db.Update(model); // Update the record in the database
                db.SaveChanges(); // Save changes

                return RedirectToAction("Show_Exam", "Facultypanel"); // Redirect to another action after successful update
            }

            // Handle validation errors
            return View("ExamEdit", model);
        }








        [HttpGet]
        public IActionResult Add_ExamMark()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            var students = db.Candidates.ToList();
            var studentsList = new SelectList(students, "CandidateId", "FirstName");
            ViewBag.CandidateId = studentsList;


            var examss = db.Exams.ToList();
            var examssList = new SelectList(examss, "ExamId", "ExaxmName");
            ViewBag.ExamId = examssList;


            return View();
        }
        [HttpPost]
        public IActionResult Add_ExamMark(ExamMark marksexam)
        {
            var exammarkss = new ExamMark()
            {
                TotalMarks = marksexam.TotalMarks,
                ObtainedMarks = marksexam.ObtainedMarks,
                ExamId = marksexam.ExamId,
                CandidateId = marksexam.CandidateId

            };
            db.ExamMarks.Add(exammarkss);
            db.SaveChanges();
            return RedirectToAction("Add_ExamMark"  , "Facultypanel");
        }
        [HttpGet]
        public IActionResult Show_ExamMark()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            return View(db.ExamMarks.Include(x => x.Candidate).Include(x => x.Exam).ToList());
        }

        public IActionResult ExamMarkDelete(int id)
        {

            ExamMark Delete = db.ExamMarks.Find(id);
            db.ExamMarks.Remove(Delete);
            db.SaveChanges();
            return RedirectToAction("Show_ExamMark", "Facultypanel" );
        }





        //update record
        public IActionResult ExamMarkEdit(int id)
        {

            var students = db.Candidates.ToList();
            var studentsList = new SelectList(students, "CandidateId", "FirstName");
            ViewBag.CandidateId = studentsList;

            var Exammm = db.Exams.ToList();
            var exammList = new SelectList(Exammm, "ExamId", "ExaxmName");
            ViewBag.ExamId = exammList;



            var record = db.ExamMarks.Find(id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        [HttpPost]
        public IActionResult ExamMarkUpdate(ExamMark model)
        {


            if (ModelState.IsValid)
            {
                db.Update(model); // Update the record in the database
                db.SaveChanges(); // Save changes

                return RedirectToAction("Show_ExamMark", "Facultypanel"); // Redirect to another action after successful update
            }

            // Handle validation errors
            return View("ExamMarkEdit", model);
        }





    }
}
