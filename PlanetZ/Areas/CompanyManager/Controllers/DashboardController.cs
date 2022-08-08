using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlanetZ.Areas.CompanyManager.Models.ViewModels;
using PlanetZ.Areas.CompanyManager.Models.ViewModels.EmployeeViewModels;
using PlanetZ.Areas.EmployeePage.Models;
using PlanetZ.Areas.SiteAdministrator.Models;
using PlanetZ.Data;
using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using PlanetZ.Extensions;
using ChangeManagerPasswordVM = PlanetZ.Areas.CompanyManager.Models.ViewModels.EmployeeViewModels.ChangeManagerPasswordVM;

namespace PlanetZ.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager")]
    [Authorize]
    [CompanyManagerAuthorization]
    public class DashboardController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Employee> _userManager;

        public DashboardController(UserManager<Employee> userManager, IWebHostEnvironment webHostEnvironment, ApplicationDbContext db)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }

        public IActionResult Index()
        {
            var list = _db.Companies.ToList();
            var user = _db.Users.Include(x => x.Company).FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));

            if (user != null)
            {
                ShowEmployeeVM vm = new ShowEmployeeVM()
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
                vm.listEmployee = _userManager.Users.Where(x => x.ActivityStatus == EnumActivityStatus.Active && x.Role == EnumRoles.Employee && x.CompanyId == user.CompanyId).Select(x => new ListEmployeeVM()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Title = x.Title,
                    PhotoPath = x.PhotoPath,
                    ActivityStatus = x.ActivityStatus
                }).ToList();
                vm.Permissions = _db.Permissions.Include(x => x.Employee).Where(x => x.PermissionStatus == EnumPermissionStatus.Pending && x.Employee.CompanyId == user.CompanyId).
                    OrderBy(x => x.PermitStartDate).Include(x => x.Employee).Select(x => new ListPermissionVM()
                    {
                        Duration = x.Duration,
                        EmployeeFullName = x.Employee.FirstName + " " + x.Employee.LastName,
                        LeaveExpiryDate = x.LeaveExpiryDate,
                        PermissionStatus = x.PermissionStatus,
                        PermitStartDate = x.PermitStartDate,
                        PermitType = x.PermitType
                    }).ToList();
                vm.Expenses = _db.Expenses.Include(x => x.Employee).Where(x => x.ExpenseStatus == EnumExpenseStatus.Pending && x.Employee.CompanyId == user.CompanyId).
                    OrderBy(x => x.RequestDate).Include(x => x.Employee).Select(x => new ListExpenseVM()
                    {
                        ExpenseAmount = x.ExpenseAmount,
                        EmployeeFullName = x.Employee.FirstName + " " + x.Employee.LastName,
                        ExpenseType = x.ExpenseType,
                        RequestDate = x.RequestDate,
                        ExpenseDescription = x.ExpenseDescription,
                        FilePath = x.FilePath
                    }).ToList();
                vm.Advances = _db.Advances.Include(x => x.Employee).Where(x => x.AdvanceStatus == EnumAdvanceStatus.Pending && x.Employee.CompanyId == user.CompanyId)
                    .OrderBy(x => x.RequestDate).Include(x => x.Employee).Select(x => new ListAdvanceVM()
                    {
                        AdvanceAmount = x.AdvanceAmount,
                        AdvanceCurrency = x.AdvanceCurrency,
                        AdvanceDescription = x.AdvanceDescription,
                        EmployeeFullName = x.Employee.FirstName + " " + x.Employee.LastName,
                        RequestDate = x.RequestDate
                    }).ToList();

                vm.Packages = _db.Packages.Where(x => x.ActivityStatus == EnumActivityStatus.Active).Select(x => new PackagesCardVM()
                {
                    PackageName = x.PackageName,
                    Description = x.Description,
                    Amount = x.Amount,
                    PhotoPath = x.PhotoPath,
                    Id = x.Id
                }).ToList();

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
        public IActionResult Profile()
        {
            var userId = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            if (userId != null)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == userId.Id);
                if (user != null)
                {
                    DetailsEmployeeVM vm = new DetailsEmployeeVM()
                    {
                        Id = user.Id,
                        Address = user.Address,
                        Birthday = user.Birthday,
                        BloodGroup = user.BloodGroup,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Gender = user.Gender,
                        Title = user.Title,
                        Profession = user.Profession,
                        WorkUnit = user.WorkUnit,
                        Identity = user.Identity,
                        JobStartDate = user.JobStartDate,
                        MaritalStatus = user.MaritalStatus,
                        PhoneNumber = user.PhoneNumber,
                        PhotoPath = user.PhotoPath,
                        Role = user.Role,
                        ActivityStatus = user.ActivityStatus,
                    };

                    if (user.DismissalDate != null)
                        vm.DismissalDate = user.DismissalDate;
                    if (user.NewJobStartDate != null)
                        vm.NewJobStartDate = user.NewJobStartDate;
                    return View(vm);
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult EditProfile(string Id)
        {
            if (Id != HttpContext.Session.GetString("userId"))
                return BadRequest();

            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            DetailsEmployeeVM vm = new DetailsEmployeeVM()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
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
        public IActionResult EditProfile(DetailsEmployeeVM vm)
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
                return RedirectToAction("Profile");
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
                ChangeManagerPasswordVM vm = new ChangeManagerPasswordVM()
                {
                    Id = user.Id,
                };
                return View(vm);
            }
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordChange(string id, ChangeManagerPasswordVM vm)
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

        public IActionResult PermissionList()
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            var list = _db.Permissions.Include(x => x.Employee).Where(x => x.Employee.CompanyId == user.CompanyId).OrderByDescending(x => x.PermissionStatus).ThenBy(x => x.PermitStartDate).ToList();
            return View(list);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ConfirmRequest(int confirmId, string employeeId)
        {
            var request = _db.Permissions.FirstOrDefault(x => x.Id == confirmId);
            if (request != null)
            {
                request.ReplyDate = DateTime.Now;
                request.PermissionStatus = EnumPermissionStatus.Approved;
                if (request.PermitType == EnumPermission.AnnualPermit)
                {
                    var user = _db.Users.FirstOrDefault(x => x.Id == employeeId);
                    user.AnnualLeave = user.AnnualLeave - (int)request.Duration;
                }
                _db.SaveChanges();
                return RedirectToAction("PermissionList");
            }
            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RejectRequest(int rejectId)
        {
            var request = _db.Permissions.FirstOrDefault(x => x.Id == rejectId);
            if (request != null)
            {
                request.ReplyDate = DateTime.Now;
                request.PermissionStatus = EnumPermissionStatus.Rejected;
                _db.SaveChanges();
                return RedirectToAction("PermissionList");
            }
            return NotFound();
        }

        public IActionResult ExpenseList()
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            var list = _db.Expenses.Include(x => x.Employee).Where(x => x.Employee.CompanyId == user.CompanyId).OrderByDescending(x => x.ExpenseStatus).ThenBy(x => x.RequestDate).ToList();
            return View(list);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ConfirmExpense(int confirmId, string employeeId)
        {
            var request = _db.Expenses.FirstOrDefault(x => x.Id == confirmId);
            if (request != null)
            {
                request.ReplyDate = DateTime.Now;
                request.ExpenseStatus = EnumExpenseStatus.Approved;
                _db.SaveChanges();
                return RedirectToAction("ExpenseList");
            }
            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RejectExpense(int rejectId)
        {
            var request = _db.Expenses.FirstOrDefault(x => x.Id == rejectId);
            if (request != null)
            {
                request.ReplyDate = DateTime.Now;
                request.ExpenseStatus = EnumExpenseStatus.Rejected;
                _db.SaveChanges();
                return RedirectToAction("ExpenseDescription", request);
            }
            return NotFound();
        }

        public IActionResult AdvanceList()
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            var list = _db.Advances.Include(x => x.Employee).Where(x => x.Employee.CompanyId == user.CompanyId).OrderByDescending(x => x.AdvanceStatus).ThenBy(x => x.RequestDate).ToList();
            return View(list);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ConfirmAdvanceRequest(int confirmId, string employeeId)
        {
            var request = _db.Advances.FirstOrDefault(x => x.Id == confirmId);
            if (request != null)
            {
                request.ReplyDate = DateTime.Now;
                request.AdvanceStatus = EnumAdvanceStatus.Approved;
                _db.SaveChanges();
                return RedirectToAction("AdvanceList");
            }
            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RejectAdvanceRequest(int rejectId)
        {
            var request = _db.Advances.FirstOrDefault(x => x.Id == rejectId);
            if (request != null)
            {
                request.ReplyDate = DateTime.Now;
                request.AdvanceStatus = EnumAdvanceStatus.Rejected;
                _db.SaveChanges();
                return RedirectToAction("AdvanceDescription", request);
            }
            return NotFound();
        }

        public IActionResult AdvanceDescription(Advance advance)
        {
            if (advance == null)
            {
                return BadRequest();
            }
            return View(advance);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult SaveDescription(int id, string returnResponse)
        {
            Advance advance = _db.Advances.FirstOrDefault(x => x.Id == id);
            if (advance != null)
            {
                if (returnResponse != null)
                {
                    advance.ReturnResponse = returnResponse;
                    _db.Update(advance);
                    _db.SaveChanges();
                    return RedirectToAction("AdvanceList");
                }
                return RedirectToAction("AdvanceList");
            }
            return NotFound();
        }
        public IActionResult ExpenseDescription(Expense expense)
        {
            if (expense == null)
            {
                return BadRequest();
            }
            return View(expense);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult SaveDescriptionExpense(int id, string returnResponse)
        {
            Expense expense = _db.Expenses.FirstOrDefault(x => x.Id == id);
            if (expense != null)
            {
                if (returnResponse != null)
                {
                    expense.ReturnResponse = returnResponse;
                    _db.Update(expense);
                    _db.SaveChanges();
                    return RedirectToAction("ExpenseList");
                }
                return RedirectToAction("ExpenseList");
            }
            return NotFound();
        }

        public IActionResult ShowWallet()
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            Wallet wallet = _db.Wallets.Where(x => x.CompanyId == user.CompanyId).FirstOrDefault();
            return View(wallet);
        }

        public IActionResult TransferBalance()
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            Wallet wallet = _db.Wallets.Include(x => x.CreditCards).Where(x => x.CompanyId == user.CompanyId).FirstOrDefault();

            if (wallet == null)
                return NotFound();
            if (!wallet.CreditCards.Any())
                return RedirectToAction("AddCreditCard");

            else
            {
                TransferBalanceVM vm = new TransferBalanceVM()
                {
                    WalletBalance = wallet.Balance,
                    WalletId = wallet.Id,
                    CreditCards = _db.CreditCards.Where(x => x.WalletId == wallet.Id).Select(x => new SelectListItem()
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }).ToList()
                };
                return View(vm);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult TransferBalance(TransferBalanceVM vm)
        {
            Wallet wallet = _db.Wallets.Find(vm.WalletId);
            if (ModelState.IsValid)
            {
                CreditCard creditCard = _db.CreditCards.Find(vm.CreditCardId);
                if (creditCard.Balance > vm.TransferAmount)
                {
                    wallet.Balance += vm.TransferAmount;
                    creditCard.Balance -= vm.TransferAmount;

                    _db.SaveChanges();
                    var message = $"₺{vm.TransferAmount} transferred to your wallet successfully";
                    HttpContext.Session.SetString("message", message);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = "You don't have enough balance in your credit card";
                    return View(vm);
                }
            }
            vm.CreditCards = _db.CreditCards.Where(x => x.WalletId == wallet.Id).Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            return View(vm);
        }

        public IActionResult AddCreditCard()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddCreditCard(CreditCard vm)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
                var company = _db.Companies.Where(x => x.Id == user.CompanyId).FirstOrDefault();
                if (user != null)
                {
                    CreditCard creditCard = new CreditCard()
                    {
                        Balance = 0,
                        CardNumber = vm.CardNumber,
                        CardExpire = vm.CardExpire,
                        CardCvv = vm.CardCvv,
                        WalletId = company.WalletId,
                        Name = vm.Name
                    };
                    _db.CreditCards.Add(creditCard);
                    _db.SaveChanges();
                    var message = "Your card has been successfully added";
                    HttpContext.Session.SetString("message", message);
                    return RedirectToAction("Index");
                }
                return BadRequest();
            }

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("CompanyManager/BuyPackage/{id?}")]
        public IActionResult BuyPackage(int? id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
            var company = _db.Companies.Include(x => x.Packages).Include(x => x.Wallet).ThenInclude(x => x.CreditCards).Where(x => x.Id == user.CompanyId).FirstOrDefault();

            if (!company.Wallet.CreditCards.Any())
            {
                return RedirectToAction("AddCreditCard");
            }
            else
            {
                Package package = _db.Packages.FirstOrDefault(x => x.Id == id);
                if (package != null)
                {
                    if (company.Wallet.Balance > package.Amount)
                    {
                        company.Packages.Add(package);
                        company.Wallet.Balance -= package.Amount;
                        var message = "Package bought successfully";
                        HttpContext.Session.SetString("message", message);
                        _db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("TransferBalance");
                }
                return NotFound();
            }
        }
    }
}
