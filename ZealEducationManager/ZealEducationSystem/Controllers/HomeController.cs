using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;
using ZealEducationSystem.Data;
using ZealEducationSystem.Models;
using System.Linq;
using Microsoft.AspNetCore.Routing.Matching;

namespace ZealEducationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ZealEducationTestContext db;
        private readonly IHttpContextAccessor contx;
        public HomeController(ILogger<HomeController> logger , ZealEducationTestContext db, IHttpContextAccessor contx)
        {
            _logger = logger;
            this.db = db;
            this.contx = contx;
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
            return RedirectToAction("Login");
        }
        //Login work
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(SignupDetail logincheck)
        {
            var finduser = db.SignupDetails.FirstOrDefault(x => x.UserEmail == logincheck.UserEmail);

            var frontEmail = logincheck.UserEmail;
            var frontPass = logincheck.UserPassword;
            var frontRole = logincheck.UserRole;

            var dbEmail = finduser?.UserEmail;
            var dbPass = finduser?.UserPassword;
            var dbRole = finduser?.UserRole;
            var dbID = finduser.UserId;

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

                if (frontRole == "admin")
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (frontRole == "faculty")
                {
                    return RedirectToAction("Index", "Facultypanel");
                }
            }


            return View("Login");
        }


        public IActionResult Logout()
        {

            contx.HttpContext.Session.Clear();



            return RedirectToAction("Login", "Home");

        }

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

                return RedirectToAction("Profile", "Home"); // Redirect to another action after successful update
            }

            // Handle validation errors
            return View("ProfileEdit", model);
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

            return RedirectToAction("Index","Home");
        }
        public IActionResult Privacy()
        {
            return View();
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

            return RedirectToAction("Show_Student","Home");
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

                return RedirectToAction("Show_Student", "Home");
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
           if(candidateimage != null && candidateimage.Length > 0)
            {
                var fileExt = System.IO.Path.GetExtension(candidateimage.FileName).Substring(1);
                var random = Path.GetFileName(candidateimage.FileName);
                var FileName = Guid.NewGuid() + random;

                string imgFolder = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/candidateimage");

                if(!Directory.Exists(imgFolder))
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

            return RedirectToAction("Add_Student");

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


            //Batchess batchDelete = db.Batchesses.Find(id);
            //db.Batchesses.Remove(batchDelete);
            //db.SaveChanges();

            return RedirectToAction("Show_Batch");
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

                return RedirectToAction("Show_Batch", "Home");
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
            return RedirectToAction("Add_Batch");
        }
        [HttpGet]
        public IActionResult Show_Faculty()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            return View(db.Faculties.ToList());
        }

        public IActionResult Delete(int id)
        {
            Faculty Delete = db.Faculties.Find(id);
            db.Faculties.Remove(Delete);
            db.SaveChanges();



          
            //db.SaveChanges();

            return RedirectToAction("Show_Faculty");
        }





        //update record
        public IActionResult Edit(int id)
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            var record = db.Faculties.Find(id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        // Action method to handle the form submission and update the record
        [HttpPost]
        public IActionResult Update(Faculty model)
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

                return RedirectToAction("Show_Faculty", "Home"); // Redirect to another action after successful update
            }

            // Handle validation errors
            return View("Edit", model);
        }




        [HttpGet]
        public IActionResult Add_Faculty()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            return View();
        }
        [HttpPost]
        public IActionResult Add_Faculty(Faculty addFaculty)
        {
            var newfaculty = new Faculty()
            {
                FirstName = addFaculty.FirstName,
                LastName = addFaculty.LastName,
                FacultyEmail = addFaculty.FacultyEmail,
                FacultyPassword = addFaculty.FacultyPassword
            };
            db.Faculties.Add(newfaculty);
            db.SaveChanges();
            return RedirectToAction("Add_Faculty");
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

           
            return RedirectToAction("Add_Course");
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
            return RedirectToAction("Show_Course");
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

                return RedirectToAction("Show_Course", "Home"); // Redirect to another action after successful update
            }

            // Handle validation errors
            return View("CourseEdit", model);
        }



        [HttpGet]
        public IActionResult Add_Fee()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            var students = db.Candidates.ToList();
            var studentsList = new SelectList(students, "CandidateId", "FirstName");
            ViewBag.CandidateId = studentsList;

            var courses = db.Courses.ToList();
            var courseList = new SelectList(courses, "CourseId", "CourseName");
            ViewBag.CourseId = courseList;


            return View();
        }
        [HttpPost]
        public IActionResult Add_Fee(FeesDetail feecollection)
        {
           

            var newfeesCollection = new FeesDetail()
            {
                CollectedAmount = feecollection.CollectedAmount,
                FeeMonth = feecollection.FeeMonth,
                CandidateId = feecollection.CandidateId,
            };
            db.FeesDetails.Add(newfeesCollection);
            db.SaveChanges();
            return RedirectToAction("Add_Fee");
        }



        [HttpGet]
        public IActionResult Show_Fee()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            return View(db.FeesDetails.Include(x => x.Candidate).ToList());
        }
        public IActionResult FeesDelete(int id)
        {
            FeesDetail Delete = db.FeesDetails.Find(id);
            db.FeesDetails.Remove(Delete);
            db.SaveChanges();
            return RedirectToAction("Show_Fee");
        }





        //update record
        public IActionResult FeesEdit(int id)
        {




            var students = db.Candidates.ToList();
            var studentsList = new SelectList(students, "CandidateId", "FirstName");
            ViewBag.CandidateId = studentsList;





            var record = db.FeesDetails.Find(id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        // Action method to handle the form submission and update the record
        [HttpPost]
        public IActionResult FeesUpdate(FeesDetail model)
        {
          

            if (ModelState.IsValid)
            {
                db.Update(model); // Update the record in the database
                db.SaveChanges(); // Save changes

                return RedirectToAction("Show_Fee", "Home"); // Redirect to another action after successful update
            }

            // Handle validation errors
            return View("FeesEdit", model);
        }







        [HttpGet]
        public IActionResult Add_Examtype()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;
            return View();
        }
        [HttpPost]
        public IActionResult Add_Examtype(ExamType exammtype)
        {
            var typeexammm = new ExamType()
            {
                ExamType1 = exammtype.ExamType1
            }
            ;
            db.ExamTypes.Add(typeexammm);
            db.SaveChanges();
            return RedirectToAction("Add_Examtype");
        }


        [HttpGet]
        public IActionResult Show_Examtype()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            return View(db.ExamTypes.ToList());

        }

        public IActionResult ExamTypeDelete(int id)
        {

            ExamType Delete = db.ExamTypes.Find(id);
            db.ExamTypes.Remove(Delete);
            db.SaveChanges();
            return RedirectToAction("Show_Examtype");
        }





        //update record
        public IActionResult ExamTypeEdit(int id)
        {

            var record = db.ExamTypes.Find(id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        [HttpPost]
        public IActionResult ExamTypeUpdate(ExamType model)
        {


            if (ModelState.IsValid)
            {
                db.Update(model); // Update the record in the database
                db.SaveChanges(); // Save changes

                return RedirectToAction("Show_Examtype", "Home"); // Redirect to another action after successful update
            }

            // Handle validation errors
            return View("ExamTypeEdit", model);
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
            return RedirectToAction("Add_Exam");
        }
        [HttpGet]
        public IActionResult Show_Exam()
        {
            var emailcheck = contx.HttpContext.Session.GetString("UserEmail");
            var passcheck = contx.HttpContext.Session.GetString("UserPassword");
            var rolecheck = contx.HttpContext.Session.GetString("UserRole");

            ViewBag.Role = rolecheck;
            ViewBag.Email = emailcheck;

            return View(db.Exams.Include(x => x.Course).Include(x => x.Examtype).Include(X=> X.Batch).ToList());
        }

        public IActionResult ExamDelete(int id)
        {

            Exam Delete = db.Exams.Find(id);
            db.Exams.Remove(Delete);
            db.SaveChanges();
            return RedirectToAction("Show_Exam");
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

                return RedirectToAction("Show_Exam", "Home"); // Redirect to another action after successful update
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
            return RedirectToAction("Add_ExamMark");
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
            return RedirectToAction("Show_ExamMark");
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

                return RedirectToAction("Show_ExamMark", "Home"); // Redirect to another action after successful update
            }

            // Handle validation errors
            return View("ExamMarkEdit", model);
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}