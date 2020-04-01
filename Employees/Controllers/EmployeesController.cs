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


        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employeeContext = _context.FetchEmployeesRange(fetchSiz, 1);
            return View(employeeContext.ToList());
        }

        // GET: Employees/Details/5
        /*        public async Task<IActionResult> Details(int? id)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var employee = await _context.Employees
                        .Include(e => e.Boss)
                        .Include(e => e.Department)
                        .Include(e => e.Position)
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (employee == null)
                    {
                        return NotFound();
                    }

                    return View(employee);
                }*/

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["Boss"] = new SelectList(_context.FetchAllEmployees(), "Id", "Surname");
            ViewData["Department"] = new SelectList(_context.FetchAllDepartments(), "Id", "Name");
            ViewData["Position"] = new SelectList(_context.FetchAllPositions(), "Id", "Name");
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Secondname,Surname,BossId,PositionId,DepartmentId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.BossId == null)
                {
                    employee.Boss = new Employee { Name = "", Secondname = "", Surname = "" };
                }
                _context.AddEmployee(employee);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BossId"] = new SelectList(_context.FetchAllEmployees(), "Id", "Id", employee.BossId);
            ViewData["DepartmentId"] = new SelectList(_context.FetchAllDepartments(), "Id", "Id", employee.DepartmentId);
            ViewData["PositionId"] = new SelectList(_context.FetchAllPositions(), "Id", "Id", employee.PositionId);
            return View(employee);
        }


        // GET: Employees/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["BossId"] = new SelectList(_context.Employees, "Id", "Id", employee.BossId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", employee.DepartmentId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Id", employee.PositionId);
            return View(employee);
        }*/

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*        [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Secondname,Surname,BossId,PositionId,DepartmentId")] Employee employee)
                {
                    if (id != employee.Id)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(employee);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!EmployeeExists(employee.Id))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["BossId"] = new SelectList(_context.Employees, "Id", "Id", employee.BossId);
                    ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", employee.DepartmentId);
                    ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Id", employee.PositionId);
                    return View(employee);
                }*/

        // GET: Employees/Delete/5
        /*        public async Task<IActionResult> Delete(int? id)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var employee = await _context.Employees
                        .Include(e => e.Boss)
                        .Include(e => e.Department)
                        .Include(e => e.Position)
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (employee == null)
                    {
                        return NotFound();
                    }

                    return View(employee);
                }
        */
        /*        // POST: Employees/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(int id)
                {
                    var employee = await _context.Employees.FindAsync(id);
                    _context.Employees.Remove(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }*/

        private bool EmployeeExists(int id)
        {
            return _context.EmployeeExists(id);
        }
    }
}
