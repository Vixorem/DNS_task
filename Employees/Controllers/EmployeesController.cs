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
        private readonly int fetchSiz = 10;

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult GetEmployees()
        {
            var employeeContext = _context.FetchEmployeesRange(fetchSiz, 1);
            return Json(employeeContext);
        }

        public IActionResult Edit(int? id)
        {

            var employee = _context.FetchEmployeeById(id);
            if (employee == null)
            {
                return null;
            }
            return Json(employee);
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
            //TODO: доделать это, когда разберусь с пагинацией
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
                created = emp
            }); ;
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            //TODO
            return null;
        }

        private bool EmployeeExists(int id)
        {
            return _context.EmployeeExists(id);
        }
    }
}
