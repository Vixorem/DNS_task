using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Employees.Data;
using Employees.Models;

namespace Employees.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeContext _context = new EmployeeContext();
        private readonly int fetchSiz = 30;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employeeContext = _context.FetchEmployeesRange(fetchSiz, 1);
            return Json(employeeContext);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return PartialView();
        }

        public IActionResult GetEmployee(int id)
        {
            if (EmployeeExists(id) == false)
            {
                return Json(new
                {
                    success = true,
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
            /*
             Если на текущей страницы есть место, чтобы добавить запись,
             то, после подтверждения, добавляем ее сами на клиенте
             */
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
