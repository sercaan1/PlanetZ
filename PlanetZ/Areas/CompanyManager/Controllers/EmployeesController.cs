using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlanetZ.Areas.CompanyManager.Models.ViewModels.EmployeeViewModels;
using PlanetZ.Data;
using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PlanetZ.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager")]
    [Authorize]
    [CompanyManagerAuthorization]

    public class EmployeesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Employee> _userManager;

        public EmployeesController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext db, UserManager<Employee> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = db;
            _userManager = userManager;
        }

        public IActionResult ListEmployees()
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            var list = _userManager.Users.Where(x => x.Role == EnumRoles.Employee && x.CompanyId == user.CompanyId).Select(x => new ListEmployeeVM()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhotoPath = x.PhotoPath,
                ActivityStatus = x.ActivityStatus
            }).ToList();
            string message = HttpContext.Session.GetString("message");
            if (message != null)
            {
                TempData["message"] = message;
                HttpContext.Session.Remove("message");
            }

            return View(list);
        }

        public IActionResult CreateEmployee()
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
                var mail = user.Email.Substring(user.Email.IndexOf('@'));
                Employee employee = new Employee();
                employee.UserName = vm.FirstName.ToLower().Trim() + "." + vm.LastName.ToLower().Trim() + mail;
                employee.FirstName = vm.FirstName;
                employee.LastName = vm.LastName;
                employee.Address = vm.Address;
                employee.Profession = vm.Profession;
                employee.Title = vm.Title;
                employee.WorkUnit = vm.WorkUnit;
                employee.Birthday = vm.Birthday;
                employee.BloodGroup = vm.BloodGroup;
                employee.Email = vm.FirstName.ToLower().Trim() + "." + vm.LastName.ToLower().Trim() + mail;
                employee.Gender = vm.Gender;
                employee.Identity = vm.Identity;
                employee.JobStartDate = vm.JobStartDate;
                employee.PhoneNumber = vm.PhoneNumber;
                employee.MaritalStatus = vm.MaritalStatus;
                employee.EmailConfirmed = true;
                employee.CompanyId = user.CompanyId;
                employee.Salary = vm.Salary;
                employee.AnnualLeave = (DateTime.Now - employee.JobStartDate).TotalDays > 1825 ? 21 : (DateTime.Now - employee.JobStartDate).TotalDays > 365 ? 14 : 0;
                var password = GeneratePassword();

                if (vm.PhotoFile != null)
                {
                    string fileName = Guid.NewGuid() + vm.PhotoFile.FileName;
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img/employeePhotos", fileName);

                    using (var stream = System.IO.File.Create(uploadPath))
                    {
                        vm.PhotoFile.CopyTo(stream);
                    }

                    employee.PhotoPath = fileName;

                }

                else employee.PhotoPath = "employeeWithoutPhoto.jpg";
                employee.Role = EnumRoles.Employee;

                await _userManager.CreateAsync(employee, password);
                _db.SaveChanges();
                SendMail(employee.Email, "Welcome, You have been added to the system.", $"<h3>Hello {employee.FirstName}  {employee.LastName} Your Password :  {password}</h3><p>You have been registered in the Planet-Hr System by your Administrator.<a href='http://planethr.azurewebsites.net//Identity/Account/Login'>You can login to the system by clicking here</a></p>");
                string message = "Employee created successfully";
                HttpContext.Session.SetString("message", message);
                return RedirectToAction("ListEmployees");
            }
            return View(vm);
        }

        public IActionResult EditEmployee(string id)
        {
            if (id == null)
                return NotFound();

            Employee employee = _userManager.Users.FirstOrDefault(x => x.Id == id);

            if (employee != null)
            {
                EditEmployeeVM vm = new EditEmployeeVM();
                vm.Identity = employee.Identity;
                vm.Address = employee.Address;
                vm.Gender = employee.Gender;
                vm.JobStartDate = employee.JobStartDate;
                vm.MaritalStatus = employee.MaritalStatus;
                vm.Birthday = employee.Birthday;
                vm.Profession = employee.Profession;
                vm.Title = employee.Title;
                vm.NewJobStartDate = employee.NewJobStartDate;
                vm.WorkUnit = employee.WorkUnit;
                vm.BloodGroup = employee.BloodGroup;
                vm.FirstName = employee.FirstName;
                vm.Salary = employee.Salary;
                vm.LastName = employee.LastName;
                vm.PhoneNumber = employee.PhoneNumber;
                vm.PhotoPath = employee.PhotoPath;
                vm.Role = employee.Role;
                vm.DismissalDate = employee.DismissalDate;
                return View(vm);
            }

            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditEmployee(string id, EditEmployeeVM vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (vm.NewJobStartDate == null && vm.DismissalDate < vm.JobStartDate)
            {
                ViewBag.DateError = "Dismissal date cannot exceed the start date";
                return View(vm);
            }
            else if (vm.NewJobStartDate != null && vm.NewJobStartDate < vm.JobStartDate)
            {
                ViewBag.DateError = "Return date cannot exceed the start date";
                return View(vm);
            }
            else if (vm.NewJobStartDate != null && vm.DismissalDate == null)
            {
                ViewBag.DateError = "Employee cannot return before dismissed";
                return View(vm);
            }

            if (ModelState.IsValid)
            {
                Employee employee = _userManager.Users.FirstOrDefault(x => x.Id == vm.Id);

                if (employee != null)
                {
                    employee.FirstName = vm.FirstName;
                    employee.LastName = vm.LastName;
                    employee.Address = vm.Address;
                    employee.Birthday = vm.Birthday;
                    employee.BloodGroup = vm.BloodGroup;
                    employee.Profession = vm.Profession;
                    employee.Title = vm.Title;
                    employee.WorkUnit = vm.WorkUnit;
                    employee.Gender = vm.Gender;
                    employee.Identity = vm.Identity;
                    employee.Salary = vm.Salary;
                    employee.PhoneNumber = vm.PhoneNumber;
                    employee.Role = vm.Role;
                    employee.MaritalStatus = vm.MaritalStatus;
                    employee.DismissalDate = vm.DismissalDate;
                    employee.NewJobStartDate = vm.NewJobStartDate;

                    if (vm.PhotoFile != null)
                    {
                        string deleteFileName = Path.Combine(_webHostEnvironment.WebRootPath, "img/employeePhotos", employee.PhotoPath);

                        if (System.IO.File.Exists(deleteFileName))
                            System.IO.File.Delete(deleteFileName);

                        string fileName = Guid.NewGuid() + vm.PhotoFile.FileName;
                        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img/employeePhotos", fileName);

                        using (var stream = System.IO.File.Create(uploadPath))
                        {
                            vm.PhotoFile.CopyTo(stream);
                        }

                        employee.PhotoPath = fileName;
                    }

                    _db.SaveChanges();
                    string message = "Employee updated successfully";
                    HttpContext.Session.SetString("message", message);
                    return RedirectToAction("ListEmployees");
                }
                else return NotFound();
            }
            return View(vm);
        }


        public IActionResult DetailsEmployee(string id)
        {

            if (id != null)
            {
                var employee = _userManager.Users.FirstOrDefault(x => x.Id == id);
                if (employee != null)
                {
                    DetailsEmployeeVM vm = new DetailsEmployeeVM()
                    {
                        Id = employee.Id,
                        Address = employee.Address,
                        Birthday = employee.Birthday,
                        BloodGroup = employee.BloodGroup,
                        Email = employee.Email,
                        FirstName = employee.FirstName,
                        Gender = employee.Gender,
                        Title = employee.Title,
                        Profession = employee.Profession,
                        WorkUnit = employee.WorkUnit,
                        Identity = employee.Identity,
                        JobStartDate = employee.JobStartDate,
                        LastName = employee.LastName,
                        MaritalStatus = employee.MaritalStatus,
                        PhoneNumber = employee.PhoneNumber,
                        PhotoPath = employee.PhotoPath,
                        Role = employee.Role
                    };

                    if (employee.DismissalDate != null)
                        vm.DismissalDate = employee.DismissalDate;
                    if (employee.NewJobStartDate != null)
                        vm.NewJobStartDate = employee.NewJobStartDate;

                    return View(vm);
                }
            }
            return RedirectToAction("ListEmployees");
        }
        public string GeneratePassword()
        {
            string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rnd = new Random();
            string res = rnd.Next(1, 10).ToString();
            for (int i = 1; i < 7; i++)
            {
                res += (valid[rnd.Next(valid.Length)]);
            }
            res += ".";
            return res.ToString();
        }

        public static string SendMail(string email, string title, string content)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.sercank.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("info@sercank.com", "V1qoqmw2h.");
            smtp.EnableSsl = true;
            smtp.Timeout = 10000;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("info@sercank.com", "Planet-HR Support");
            mail.To.Add(email);

            mail.Subject = title;
            mail.Body = content;

            mail.IsBodyHtml = true;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;

            try
            {
                smtp.Send(mail);
                mail.Dispose();
                return "Email sent successfully.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
