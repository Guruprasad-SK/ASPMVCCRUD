using ASPMVCCRUD.Data;
using ASPMVCCRUD.Models;
using ASPMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ASPMVCCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        //ctot 2 time tab
        private readonly MVCDemoDbContext mvcDemoDbContext;
        //ctrl+.
        public EmployeeController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employee = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employee);

        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployViewModel addEmployViewModel)
        {

            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployViewModel.Name,
                Salary = addEmployViewModel.Salary,
                Email = addEmployViewModel.Email,
                DateOfBirth = addEmployViewModel.DateOfBirth,
                Department = addEmployViewModel.Department

            };

            await mvcDemoDbContext.Employees.AddAsync(employee);
            await mvcDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {

                var viewModel = new UpdateEmployeeViewModel()
                {

                    Id = employee.Id,
                    Name = employee.Name,
                    Salary = employee.Salary,
                    Email = employee.Email,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department

                };
                return await Task.Run(() => View("View",viewModel));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel Model)
        {

            var employee = await mvcDemoDbContext.Employees.FindAsync(Model.Id);
            if (employee != null)
            {

                employee.Name = Model.Name;
                employee.Salary = Model.Salary;
                employee.Email = Model.Email;
                employee.DateOfBirth = Model.DateOfBirth;
                employee.Department = Model.Department;

                await mvcDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");

            };
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee=await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }


    }

}


