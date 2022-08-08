using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanetZ.Areas.CompanyManager.Models.ViewModels.EmployeeViewModels;
using PlanetZ.Areas.EmployeePage.Models;
using PlanetZ.Data;
using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;

namespace PlanetZ.Areas.EmployeePage.Controllers
{
    [Area("EmployeePage")]
    [Authorize]
    [EmployeeAuthorizationAttribute]
    public class DashboardController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Employee> _userManager;

        public DashboardController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext db, UserManager<Employee> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var user = _userManager.Users.Include(x => x.Company).FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));

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
                vm.listEmployee = _userManager.Users.Where(x => x.ActivityStatus == EnumActivityStatus.Active && x.Role == EnumRoles.Employee && x.CompanyId == user.CompanyId)
                    .Select(x => new ListEmployeeVM()
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        Title = x.Title,
                        PhotoPath = x.PhotoPath,
                        ActivityStatus = x.ActivityStatus
                    }).ToList();
                vm.Permissions = _db.Permissions.Where(x => x.EmployeeId == user.Id && x.PermissionStatus == EnumPermissionStatus.Pending).OrderBy(x => x.PermitStartDate)
                    .Select(x => new ListPermissionVM()
                    {
                        Duration = x.Duration,
                        LeaveExpiryDate = x.LeaveExpiryDate,
                        PermitStartDate = x.PermitStartDate,
                        PermitType = x.PermitType,
                        PermissionStatus = x.PermissionStatus
                    }).ToList();
                vm.Expenses = _db.Expenses.Where(x => x.ExpenseStatus == EnumExpenseStatus.Pending).
                    OrderBy(x => x.RequestDate).Select(x => new ListExpenseVM()
                    {
                        ExpenseAmount = x.ExpenseAmount,
                        ExpenseType = x.ExpenseType,
                        RequestDate = x.RequestDate,
                        ExpenseDescription = x.ExpenseDescription,
                        FilePath = x.FilePath
                    }).ToList();
                vm.Advances = _db.Advances.Where(x => x.AdvanceStatus == EnumAdvanceStatus.Pending)
                    .OrderBy(x => x.RequestDate).Select(x => new ListAdvanceVM()
                    {
                        AdvanceAmount = x.AdvanceAmount,
                        AdvanceCurrency = x.AdvanceCurrency,
                        AdvanceDescription = x.AdvanceDescription,
                        RequestDate = x.RequestDate
                    }).ToList();
                var message = HttpContext.Session.GetString("message");
                if (message != null)
                {
                    TempData["message"] = message;
                    HttpContext.Session.Remove("message");
                }
                return View(vm);
            }
            return View();
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

        public IActionResult CreatePermission()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreatePermission(CreatePermissionVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
                if (vm.PermitType == Data.Enums.EnumPermission.AnnualPermit)
                {
                    if (vm.PermitStartDate >= vm.LeaveExpiryDate)
                    {
                        ViewBag.LeaveDayError = "error";
                        return View(vm);
                    }

                    if (user != null)
                    {
                        double totalDay = ((DateTime)vm.LeaveExpiryDate - vm.PermitStartDate).TotalDays;
                        if (totalDay > user.AnnualLeave)
                        {
                            ViewBag.InsufficientDay = user.AnnualLeave.ToString();
                            return View(vm);
                        }
                        Permission permission = new Permission()
                        {
                            Duration = (int)(((DateTime)vm.LeaveExpiryDate - vm.PermitStartDate).TotalDays),
                            LeaveExpiryDate = vm.LeaveExpiryDate,
                            EmployeeId = user.Id,
                            PermitStartDate = vm.PermitStartDate,
                            PermissionStatus = Data.Enums.EnumPermissionStatus.Pending,
                            PermitType = vm.PermitType
                        };
                        _db.Permissions.Add(permission);
                        _db.SaveChanges();
                        var message = "Permission requested successfully";
                        HttpContext.Session.SetString("message", message);
                        return RedirectToAction("Index");
                    }
                    else
                        return BadRequest();
                }
                else
                {
                    if (user != null)
                    {
                        if (vm.PermitType == Data.Enums.EnumPermission.ExcuseLeave)
                            vm.Duration = 3;
                        else if (vm.PermitType == Data.Enums.EnumPermission.AdministrativeLeave)
                            vm.Duration = 2;
                        else if (vm.PermitType == Data.Enums.EnumPermission.PaternityLeave)
                            vm.Duration = 5;
                        else if (vm.PermitType == Data.Enums.EnumPermission.MarriagePermission)
                            vm.Duration = 2;

                        Permission permission = new Permission()
                        {
                            PermitStartDate = vm.PermitStartDate,
                            Duration = vm.Duration,
                            EmployeeId = user.Id,
                            PermissionStatus = Data.Enums.EnumPermissionStatus.Pending,
                            PermitType = vm.PermitType,
                            LeaveExpiryDate = vm.PermitStartDate.AddDays((double)vm.Duration)
                        };
                        _db.Permissions.Add(permission);
                        _db.SaveChanges();
                        var message = "Permission requested successfully";
                        HttpContext.Session.SetString("message", message);
                        return RedirectToAction("Index");
                    }
                    return BadRequest();
                }
            }
            return View(vm);
        }

        public IActionResult PermissionList()
        {
            var list = _db.Permissions.Where(x => x.EmployeeId == HttpContext.Session.GetString("userId"))
                .OrderByDescending(x => x.PermissionStatus)
                .ThenBy(x => x.PermitStartDate)
                .Include(x => x.Employee)
                .Select(x => new ListPermissionVM()
                {
                    Duration = x.Duration,
                    EmployeeFullName = x.Employee.FirstName + " " + x.Employee.LastName,
                    LeaveExpiryDate = x.LeaveExpiryDate,
                    PermissionStatus = x.PermissionStatus,
                    PermitStartDate = x.PermitStartDate,
                    PermitType = x.PermitType,
                    ReplyDate = x.ReplyDate
                }).ToList();
            return View(list);
        }
        public IActionResult CreateExpense()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateExpense(CreateExpenseVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
                if (user != null)
                {
                    Expense expense = new Expense()
                    {
                        ExpenseAmount = vm.ExpenseAmount,
                        EmployeeId = user.Id,
                        ExpenseDescription = vm.ExpenseDescription,
                        RequestDate = vm.RequestDate,
                        ExpenseType = vm.ExpenseType,
                        ExpenseStatus = Data.Enums.EnumExpenseStatus.Pending

                    };


                    if (vm.FilePath == null)
                    {
                        ViewBag.FileError = "error";
                        return View(vm);
                    }


                    var extension = Path.GetExtension(vm.FilePath.FileName);
                    if (extension == ".jpg" || extension == ".png" || extension == ".jpeg" || extension == ".pdf")
                    {
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "files");
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        using (var fileStream = new FileStream(Path.Combine(uploadPath, vm.FilePath.FileName), FileMode.Create))
                        {
                            vm.FilePath.CopyTo(fileStream);
                        }
                        expense.FilePath = vm.FilePath.FileName;
                    }
                    else
                    {
                        ViewBag.FileError = "file extension must be .pdf, .jpg,.png or .jpeg";
                        return View(vm);
                    }


                    _db.Expenses.Add(expense);
                    _db.SaveChanges();
                    var message = "Expense requested successfully";
                    HttpContext.Session.SetString("message", message);
                    return RedirectToAction("Index");
                }
                return BadRequest();
            }
            return View(vm);
        }

        public IActionResult ExpenseList()
        {
            var list = _db.Expenses.Where(x => x.EmployeeId == HttpContext.Session.GetString("userId"))
                .OrderByDescending(x => x.ExpenseStatus)
                .ThenBy(x => x.RequestDate)
                .Include(x => x.Employee)
                .Select(x => new ListExpenseVM()
                {
                    Id = x.Id,
                    ExpenseAmount = x.ExpenseAmount,
                    EmployeeFullName = x.Employee.FirstName + " " + x.Employee.LastName,
                    ExpenseDescription = x.ExpenseDescription,
                    RequestDate = x.RequestDate,
                    ExpenseStatus = x.ExpenseStatus,
                    ExpenseType = x.ExpenseType,
                    ReplyDate = x.ReplyDate,
                    FilePath = x.FilePath,
                    ReturnResponse = x.ReturnResponse
                }).ToList();
            return View(list);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ChangeDocument(CreateExpenseVM vm, Expense expense)
        {
            if (vm.FilePath == null)
            {
                ViewBag.FileError = "error";
                return View(vm);
            }


            var extension = Path.GetExtension(vm.FilePath.FileName);
            if (extension == ".jpg" || extension == ".png" || extension == ".jpeg" || extension == ".pdf")
            {
                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "files");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                using (var fileStream = new FileStream(Path.Combine(uploadPath, vm.FilePath.FileName), FileMode.Create))
                {
                    vm.FilePath.CopyTo(fileStream);
                }
                expense.FilePath = vm.FilePath.FileName;
            }
            else
            {
                ViewBag.FileError = "file extension must be .pdf, .jpg,.png or .jpeg";
                return View(vm);
            }
            return RedirectToAction("ExpenseList");
        }

        [HttpGet]
        public IActionResult EditExpense(int id)
        {
            Expense expense = _db.Expenses.FirstOrDefault(x => x.Id == id);

            if (expense != null)
            {
                EditExpenseVM vm = new EditExpenseVM()
                {
                    Id = expense.Id,
                    ExpenseAmount = expense.ExpenseAmount,
                    ExpenseType = expense.ExpenseType,
                    FilePath = expense.FilePath
                };
                return View(vm);
            }

            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditExpense(EditExpenseVM vm)
        {
            if (ModelState.IsValid)
            {
                Expense expense = _db.Expenses.FirstOrDefault(x => x.Id == vm.Id);
                if (vm.File != null)
                {
                    string deleteFileName = Path.Combine(_webHostEnvironment.WebRootPath, "/files", expense.FilePath);

                    if (System.IO.File.Exists(deleteFileName))
                        System.IO.File.Delete(deleteFileName);

                    string fileName = Guid.NewGuid() + vm.File.FileName;
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "files", fileName);

                    using (var stream = System.IO.File.Create(uploadPath))
                    {
                        vm.File.CopyTo(stream);
                    }

                    expense.FilePath = fileName;
                }

                expense.ExpenseAmount = vm.ExpenseAmount;
                expense.ExpenseType = vm.ExpenseType;

                _db.SaveChanges();
                var message = "Expense updated successfully";
                HttpContext.Session.SetString("message", message);
                return RedirectToAction("Index");
            }

            return View(vm);
        }

        public IActionResult CreateAdvance()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateAdvance(AdvanceRequestVM vm)
        {
            if (vm.AdvanceType == EnumAdvanceType.TravelExpense)
            {
                if (vm.AdvanceDescription != null)
                {
                    var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
                    if (user != null)
                    {
                        Advance advance = new Advance()
                        {
                            EmployeeId = user.Id,
                            AdvanceAmount = vm.AdvanceAmount,
                            AdvanceDescription = vm.AdvanceDescription,
                            RequestDate = DateTime.Now,
                            AdvanceStatus = Data.Enums.EnumAdvanceStatus.Pending,
                            AdvanceCurrency = vm.AdvanceCurrency,
                            AdvanceType = vm.AdvanceType,
                        };
                        _db.Advances.Add(advance);
                        _db.SaveChanges();
                        var message = "Advance requested successfully";
                        HttpContext.Session.SetString("message", message);
                        return RedirectToAction("Index");
                    }
                }
            }
            else if (vm.AdvanceType == EnumAdvanceType.AdvancePayment)
            {
                if (ModelState.IsValid)
                {
                    var user = _userManager.Users.FirstOrDefault(x => x.Id == HttpContext.Session.GetString("userId"));
                    if (user != null)
                    {
                        Advance advance = new Advance()
                        {
                            EmployeeId = user.Id,
                            AdvanceAmount = vm.AdvanceAmount,
                            AdvanceDescription = vm.AdvanceDescription,
                            RequestDate = DateTime.Now,
                            AdvanceStatus = Data.Enums.EnumAdvanceStatus.Pending,
                            AdvanceCurrency = vm.AdvanceCurrency,
                            AdvanceType = vm.AdvanceType,
                        };
                        _db.Advances.Add(advance);
                        _db.SaveChanges();
                        var message = "Advance requested successfully";
                        HttpContext.Session.SetString("message", message);
                        return RedirectToAction("Index");
                    }
                    return BadRequest();
                }
            }
            return View(vm);
        }
        public IActionResult AdvanceList()
        {
            var list = _db.Advances.Include(x => x.Employee).Where(x => x.EmployeeId == HttpContext.Session.GetString("userId"))
                .OrderByDescending(x => x.AdvanceStatus)
                .ThenBy(x => x.RequestDate)
                .Select(x => new ListAdvanceVM()
                {
                    EmployeeFullName = x.Employee.FirstName + " " + x.Employee.LastName,
                    AdvanceAmount = x.AdvanceAmount,
                    AdvanceStatus = x.AdvanceStatus,
                    AdvanceDescription = x.AdvanceDescription,
                    RequestDate = x.RequestDate,
                    ReplyDate = x.ReplyDate,
                    ReturnResponse = x.ReturnResponse,
                    AdvanceCurrency = x.AdvanceCurrency,
                    AdvanceType = x.AdvanceType
                }).ToList();
            return View(list);
        }
    }

}
