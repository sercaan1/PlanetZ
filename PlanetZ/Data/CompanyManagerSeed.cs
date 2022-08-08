using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PlanetZ.Data
{
    public static class CompanyManagerSeed
    {
        public static async Task Seed(UserManager<Employee> userManager, ApplicationDbContext db)
        {
            if (await userManager.Users.AnyAsync(x => x.UserName == "ali.can@planetik.com"))
                return;

            var employee = new Employee()
            {
                UserName = "ali.can@planetik.com",
                EmailConfirmed = true,
                FirstName = "Ali",
                LastName = "Can",
                Birthday = new DateTime(1990, 02, 02),
                JobStartDate = new DateTime(2020, 06, 06),
                Email = "ali.can@planetik.com",
                Address = "Mustafa Kemal Mahallesi 5. Cadde 5/10 Çankaya/Ankara",
                Identity = "45253459366",
                Role = Enums.EnumRoles.CompanyManager,
                PhoneNumber = "511 192 30 08",
                Title = "Company Manager",
                Profession = "Operation Engineer",
                WorkUnit = "Managing",
                BloodGroup = Enums.EnumBloodGroup.ABPos,
                Gender = Enums.EnumGender.Male,
                MaritalStatus = Enums.EnumMaritalStatus.Married,
                //ActivityStatus = Enums.EnumActivityStatus.Active,
                PhotoPath = "employeeWithoutPhoto.jpg",
                CompanyId = 1
            };

            if (!db.Companies.Any())
            {
                var company = new Company()
                {
                    CompanyName = "PlanetIK",
                    Address = "Ortadoğu Mah. Bilkent Plaza A3 Blok K:1 D:9, Bilkent, Çankaya, Ankara",
                    //Sector = "Human Resources",
                    MersisNo = 1111111111.ToString(),
                    Employees = new List<Employee>() { employee },
                    FoundedDate = new DateTime(2020, 06, 06),
                    CompanyLogoPath = "planetIKLogo.png"
                };
                db.Companies.Add(company);
                db.SaveChanges();
            }

            await userManager.CreateAsync(employee, "Ankara1.");
        }
    }
}
