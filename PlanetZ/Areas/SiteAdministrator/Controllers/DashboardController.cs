using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlanetZ.Areas.SiteAdministrator.Models;
using PlanetZ.Data;
using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PlanetZ.Areas.SiteAdministrator.Controllers
{
    [Area("SiteAdministrator")]
    [Authorize]
    [SiteManagerAuthorization]
    public class DashboardController : Controller
    {
        public IWebHostEnvironment _webHostEnvironment { get; }
        public ApplicationDbContext _db { get; }
        public UserManager<Employee> _userManager { get; }

        public DashboardController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext db, UserManager<Employee> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            var user = _db.Users.Include(x => x.Company).FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));

            if (user != null)
            {
                IndexVM vm = new IndexVM()
                {
                    Id = HttpContext.Session.GetString("userId"),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Title = user.Title,
                    PhotoPath = user.PhotoPath,
                    WorkUnit = user.WorkUnit,
                    Profession = user.Profession,
                    Company = user.Company,
                    Role = user.Role,
                    ActivityStatus = user.ActivityStatus

                };
                vm.Companies = _db.Companies.Select(x => new ListCompanyVM()
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    CompanyType = x.CompanyType,
                    Email = x.Email,
                    Sector = x.Sector,
                    LogoPath = x.CompanyLogoPath
                }).ToList();

                vm.Packages = _db.Packages.Where(x => x.ActivityStatus == EnumActivityStatus.Active).Select(x => new PackagesCardVM()
                {
                    PackageName = x.PackageName,
                    Description = x.Description,
                    Amount = x.Amount,
                    PhotoPath = x.PhotoPath

                }).ToList();

                var list = _db.Packages.Select(x => x.Id).ToList();
                int numberofcards = list.Count();

                ViewBag.Numberofcards = numberofcards;

                string message = HttpContext.Session.GetString("message");
                if (message != null)
                {
                    TempData["message"] = message;
                    HttpContext.Session.Remove("message");
                }
                return View(vm);
            }
            return View();

        }
        public IActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateCompany(CreateCompanyVM vm)
        {
            if (ModelState.IsValid)
            {
                Company company = new Company();
                company.CompanyName = vm.CompanyName.Trim(); ;
                company.Email = vm.Email.Trim();
                company.PhoneNumber = vm.PhoneNumber;
                company.MersisNo = vm.MersisNo.ToString();
                company.DateOfContract = vm.DateOfContract;
                company.EndOfContract = vm.EndOfContract;
                company.CompanyType = vm.CompanyType;
                company.FoundedDate = vm.FoundedDate;
                company.Address = vm.Address;
                company.Sector = vm.Sector;

                Wallet wallet = new Wallet() { Balance = 0.00m };
                _db.Wallets.Add(wallet);

                company.WalletId = wallet.Id;

                if (vm.PhotoFile != null)
                {
                    string fileName = Guid.NewGuid() + vm.PhotoFile.FileName;
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img/companyLogos", fileName);

                    using (var stream = System.IO.File.Create(uploadPath))
                    {
                        vm.PhotoFile.CopyTo(stream);
                    }

                    company.CompanyLogoPath = fileName;
                }

                else company.CompanyLogoPath = "companyWithoutPhoto.jpg";

                _db.Companies.Add(company);
                _db.SaveChanges();
                var message = "Company created successfully";
                HttpContext.Session.SetString("message", message);
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        public IActionResult ListCompanies()
        {
            var list = _db.Companies.Select(x => new ListCompanyVM()
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                CompanyType = x.CompanyType,
                Email = x.Email,
                Sector = x.Sector,
                LogoPath = x.CompanyLogoPath

            }).ToList();

            return View(list);
        }
        public IActionResult DetailsCompany(int id)
        {
            if (id != null)
            {
                var company = _db.Companies.FirstOrDefault(x => x.Id == id);
                if (company != null)
                {
                    CompanyDetailsVM vm = new CompanyDetailsVM()
                    {
                        Id = company.Id,
                        CompanyName = company.CompanyName,
                        CompanyType = company.CompanyType,
                        //Sector = company.Sector,
                        Address = company.Address,
                        DateOfContract = company.DateOfContract,
                        EndOfContract = company.EndOfContract,
                        FoundedDate = company.FoundedDate,
                        Email = company.Email,
                        PhoneNumber = company.PhoneNumber,
                        CompanyLogoPath = company.CompanyLogoPath,
                        MersisNo = company.MersisNo
                    };

                    return View(vm);
                }
            }
            return RedirectToAction("ListCompanies");
        }
        public IActionResult EditCompany(int id)
        {
            if (id == null)
                return NotFound();

            Company company = _db.Companies.FirstOrDefault(x => x.Id == id);

            if (company != null)
            {
                EditCompanyVM vm = new EditCompanyVM();
                vm.CompanyName = company.CompanyName;
                vm.Address = company.Address;
                vm.Email = company.Email;
                vm.ContractEndDate = company.EndOfContract;
                vm.CompanyType = company.CompanyType;
                vm.PhoneNumber = company.PhoneNumber;
                vm.Sector = company.Sector;
                vm.CompanyLogoPath = company.CompanyLogoPath;
                return View(vm);
            }

            return NotFound();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditCompany(int id, EditCompanyVM vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                Company company = _db.Companies.FirstOrDefault(x => x.Id == vm.Id);

                if (company != null)
                {
                    company.CompanyName = vm.CompanyName;
                    company.Email = vm.Email;
                    company.Address = vm.Address;
                    company.PhoneNumber = vm.PhoneNumber;
                    company.Sector = vm.Sector;
                    company.CompanyType = vm.CompanyType;
                    company.EndOfContract = vm.ContractEndDate;

                    if (vm.PhotoFile != null)
                    {
                        string deleteFileName = Path.Combine(_webHostEnvironment.WebRootPath, "companyLogos", company.CompanyLogoPath);

                        if (System.IO.File.Exists(deleteFileName))
                            System.IO.File.Delete(deleteFileName);

                        string fileName = Guid.NewGuid() + vm.PhotoFile.FileName;
                        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "companyLogos", fileName);

                        using (var stream = System.IO.File.Create(uploadPath))
                        {
                            vm.PhotoFile.CopyTo(stream);
                        }

                        company.CompanyLogoPath = fileName;
                    }
                    _db.SaveChanges();
                    var message = "Company updated successfully";
                    HttpContext.Session.SetString("message", message);
                    return RedirectToAction("Index");
                }
                else return NotFound();
            }
            return View(vm);
        }

        public IActionResult Profile()
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            if (user != null)
            {
                ProfilePageVM vm = new ProfilePageVM()
                {
                    Id = user.Id,
                    FullName = user.FirstName + " " + user.LastName,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Birthday = user.Birthday,
                    DismissalDate = user.DismissalDate,
                    JobStartDate = user.JobStartDate,
                    NewJobStartDate = user.NewJobStartDate,
                    PhotoPath = user.PhotoPath,
                    Profession = user.Profession,
                    Title = user.Title,
                    WorkUnit = user.WorkUnit,
                    Email = user.Email
                };
                return View(vm);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult EditProfile(string Id)
        {
            if (Id != HttpContext.Session.GetString("userId"))
                return BadRequest();

            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            ProfilePageVM vm = new ProfilePageVM()
            {
                Id = user.Id,
                FullName = user.FirstName + " " + user.LastName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Birthday = user.Birthday,
                DismissalDate = user.DismissalDate,
                JobStartDate = user.JobStartDate,
                NewJobStartDate = user.NewJobStartDate,
                PhotoPath = user.PhotoPath,
                Profession = user.Profession,
                Title = user.Title,
                WorkUnit = user.WorkUnit,
                Email = user.Email
            };

            if (user != null)
                return View(vm);

            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditProfile(ProfilePageVM vm)
        {
            if (vm.Id != HttpContext.Session.GetString("userId"))
                return BadRequest();

            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
                user.PhoneNumber = vm.PhoneNumber;
                user.Address = vm.Address;
                if (vm.ProfilePhotoFile != null)
                {
                    string deleteFileName = Path.Combine(_webHostEnvironment.WebRootPath, "img/employeePhotos", user.PhotoPath);

                    if (System.IO.File.Exists(deleteFileName))
                        System.IO.File.Delete(deleteFileName);

                    string fileName = Guid.NewGuid() + vm.ProfilePhotoFile.FileName;
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img/employeePhotos", fileName);

                    using (var stream = System.IO.File.Create(uploadPath))
                    {
                        vm.ProfilePhotoFile.CopyTo(stream);
                    }

                    user.PhotoPath = fileName;
                }
                _db.SaveChanges();
                var message = "Profile updated successfully";
                HttpContext.Session.SetString("message", message);
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult PasswordChange(string id)
        {
            if (id != null)
            {
                if (id != HttpContext.Session.GetString("userId"))
                {
                    return BadRequest();
                }
            }
            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));

            if (user != null)
            {
                ChangePasswordVM vm = new ChangePasswordVM()
                {
                    Id = user.Id,
                };
                return View(vm);
            }
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordChange(string id, ChangePasswordVM vm)
        {
            if (id != HttpContext.Session.GetString("userId"))
                return BadRequest();

            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == id);

                if (user != null)
                {
                    if (vm.NewPassword == vm.ConfirmPassword)
                    {
                        var changePasswordResult = await _userManager.ChangePasswordAsync(user, vm.OldPassword, vm.NewPassword);
                        if (!changePasswordResult.Succeeded)
                        {
                            ViewBag.Message = "";
                            return View();
                        }
                        user.DidPasswordChange = true;
                        _db.SaveChanges();
                        var message = "Password changed successfully";
                        HttpContext.Session.SetString("message", message);
                        return RedirectToAction("Index");
                    }
                    ViewBag.WrongPassword = "message";
                    return View();
                }
            }

            ViewData["Error"] = "Password must contain at least a number, a capital letter and a custom mark.";
            return View();
        }

        public IActionResult CreatePackage()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreatePackage(CreatePackageVM vm)
        {
            if (ModelState.IsValid)
            {
                Package package = new Package()
                {
                    PackageName = vm.PackageName,
                    Amount = vm.Amount.Value,
                    UsageTime = vm.UsageTime.Value,
                    PublishStartDay = vm.PublishStartDay.Value,
                    PublishEndDay = vm.PublishEndDay.Value,
                    NumberofUsers = vm.NumberofUsers.Value,
                    CurrencyUnit = vm.CurrencyUnit,
                    Description = vm.Description,

                };

                if (vm.PhotoFile != null)
                {
                    string fileName = Guid.NewGuid() + vm.PhotoFile.FileName;
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img/packagesImages", fileName);

                    using (var stream = System.IO.File.Create(uploadPath))
                    {
                        vm.PhotoFile.CopyTo(stream);
                    }

                    package.PhotoPath = fileName;

                }
                else package.PhotoPath = "no-image.jpg";


                _db.Packages.Add(package);
                _db.SaveChanges();
                return RedirectToAction("ListPackage");
            }
            return View(vm);
        }

        public IActionResult ListPackage()
        {
            var list = _db.Packages.Select(x => new ListPackageVM()
            {
                PackageName = x.PackageName,
                Amount = x.Amount,
                UsageTime = x.UsageTime,
                PublishStartDay = x.PublishStartDay,
                PublishEndDay = x.PublishEndDay,
                NumberofUsers = x.NumberofUsers,
                ActivityStatus = x.ActivityStatus,
                CurrencyUnit = x.CurrencyUnit,
                PhotoPath = x.PhotoPath
            }).ToList();

            return View(list);

        }

        public IActionResult DetailsPackage(int id)
        {
            if (id != null)
            {
                var package = _db.Packages.FirstOrDefault(x => x.Id == id);
                if (package != null)
                {
                    DetailsPackageVM vm = new DetailsPackageVM()
                    {
                        Id = package.Id,
                        PackageName = package.PackageName,
                        Amount = package.Amount,
                        UsageTime = package.UsageTime,
                        PublishStartDay = package.PublishStartDay,
                        PublishEndDay = package.PublishEndDay,
                        NumberofUsers = package.NumberofUsers,
                        PhotoPath = package.PhotoPath,
                        ActivityStatus = package.ActivityStatus
                    };
                    return View(vm);
                };
            }
            return RedirectToAction("ListPackage");
        }

        public IActionResult CreateCompanyManager()
        {
            CreateCompanyManagerVM vm = new CreateCompanyManagerVM()
            {
                Companies = _db.Companies.Select(x => new SelectListItem()
                {
                    Text = x.CompanyName,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCompanyManager(CreateCompanyManagerVM vm)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee()
                {
                    Address = vm.Address,
                    Birthday = vm.Birthday,
                    BloodGroup = vm.BloodGroup,
                    CompanyId = vm.CompanyId,
                    Email = vm.Email,
                    Role = EnumRoles.CompanyManager,
                    FirstName = vm.FirstName,
                    Salary = vm.Salary,
                    UserName = vm.Email,
                    LastName = vm.LastName,
                    Identity = vm.Identity,
                    Profession = vm.Profession,
                    Title = vm.Title,
                    WorkUnit = vm.WorkUnit,
                    JobStartDate = vm.JobStartDate,
                    Gender = vm.Gender,
                    MaritalStatus = vm.MaritalStatus,
                    EmailConfirmed = true,
                    AnnualLeave = (DateTime.Now - vm.JobStartDate).TotalDays > 1825 ? 21 : (DateTime.Now - vm.JobStartDate).TotalDays > 365 ? 14 : 0
                };

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

                var password = GeneratePassword();
                var company = _db.Companies.FirstOrDefault(x => x.Id == vm.CompanyId);

                await _userManager.CreateAsync(employee, password);
                _db.SaveChanges();
                SendMail(employee.Email, "Welcome, You have been added to the system.", $"<h3>Hello {employee.FirstName}  {employee.LastName} Your Password :  {password}</h3><p>You have been registered in the Planet-Hr System by your Administrator.<a href='http://planethr.azurewebsites.net//Identity/Account/Login'>You can login to the system by clicking here</a></p>");
                string message = $"{vm.FirstName} {vm.LastName} assigned to {company.CompanyName} successfully";
                HttpContext.Session.SetString("message", message);
                return RedirectToAction("Index");
            }
            vm.Companies = _db.Companies.Select(x => new SelectListItem()
            {
                Text = x.CompanyName,
                Value = x.Id.ToString()
            }).ToList();
            return View(vm);
        }

        private static string SendMail(string email, string title, string content)
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

        private string GeneratePassword()
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



    }
}


