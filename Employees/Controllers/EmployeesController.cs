using Employees.Data;
using Employees.Models;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace Employees.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeContext _context = new EmployeeContext();
        private readonly int rowsOnPage = 10;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetEmployees([FromBody]int pageNum)
        {
            if (pageNum > rowsOnPage)
            {
                return Json(new
                {
                    success = false,
                    responseText = "The page doesn't exist"
                });
            }
            var employees = _context.FetchEmployeesRange(rowsOnPage, 1);
            return Json(new
            {
                success = true,
                totalPagesNum = Math.Ceiling(_context.EmployeesCount() / (double)rowsOnPage),
                employees
            });
        }

        [HttpGet]
        public IActionResult GetEmployee(int id)
        {
            if (EmployeeExists(id) == false)
            {
                return Json(new
                {
                    success = false,
                    responseText = "The employee never existed or already removed"
                });
            }

            var emp = _context.FetchEmployeeById(id);

            return Json(new
            {
                success = true,
                employee = emp
            });
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult EditConfirmed([FromBody]Employee e)
        {
            if (e.Id == null || EmployeeExists((int)e.Id) == false)
            {
                return Json(new
                {
                    success = true,
                    responseText = $"Employee with id {e.Id} doesn't exist"
                });
            }

            var emp = _context.UpdateEmployee(e);

            return Json(new
            {
                success = true,
                Employee = emp
            });
        }

        public IActionResult GetSelections()
        {
            var bosses = _context.FetchAllEmployees();
            var deps = _context.FetchAllDepartments();
            var poses = _context.FetchAllPositions();
            return Json(new
            {
                Bosses = bosses,
                Departments = deps,
                Positions = poses
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult Create([FromBody]  Employee employee)
        {
            if (employee == null)
            {
                return Json(new
                {
                    success = false,
                    responseText = "Parameter is null"
                });
            }

            _context.AddEmployee(employee);
            var emp = _context.FetchLastEmployee();
            return Json(new
            {
                success = true,
                employee = emp
            });
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (EmployeeExists(id) == false)
            {
                return Json(new
                {
                    success = true,
                    responseText = "The employee never existed or already removed"
                });
            }

            _context.DeleteEmployee(id);

            return Json(new
            {
                success = true,
            });
        }

        private bool EmployeeExists(int id)
        {
            return _context.EmployeeExists(id);
        }
    }
}
