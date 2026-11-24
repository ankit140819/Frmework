using Frmework.Data;
using Frmework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Frmework.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee List
        public IActionResult Index(string searchString,string sortOrder,int pageNumber = 1)
        {
            int pageSize = 7;

            var employees = _context.Employees.AsQueryable();

            // 🔍 SEARCH
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                employees = employees.Where(e =>
                    (e.Name ?? "").Contains(searchString) ||
                    (e.Email ?? "").Contains(searchString) ||
                    (e.Department ?? "").Contains(searchString)
                );
            }

            // 🔃 SORTING (we’ll explain below)
            ViewBag.NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DeptSort = sortOrder == "dept" ? "dept_desc" : "dept";

            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.Name);
                    break;
                case "dept":
                    employees = employees.OrderBy(e => e.Department);
                    break;
                case "dept_desc":
                    employees = employees.OrderByDescending(e => e.Department);
                    break;
                default:
                    employees = employees.OrderBy(e => e.Name);
                    break;
            }

            int totalRecords = employees.Count();

            var pagedEmployees = employees
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            return View(pagedEmployees);
        }




        // GET: Create Employee
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee emp)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the validation errors.";
            }
            if (ModelState.IsValid)
            {
                _context.Employees.Add(emp);
                _context.SaveChanges();
                TempData["ShowSuccessModal"] = true;
                TempData["SuccessMessage"] = "Employee added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(emp);
        }

        // GET: Edit Employee
        public IActionResult Edit(int id)
        {
            var Employee = _context.Employees.Find(id);
            if (Employee == null)
                return NotFound();

            return View(Employee);
        }

        // POST: Edit Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Update(emp);
                _context.SaveChanges();

                TempData["ShowSuccessModal"] = true;
                TempData["SuccessMessage"] = "Employee update successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(emp);
        }

        // GET: Delete Employee
        public IActionResult Delete(int id)
        {
            var emp = _context.Employees.Find(id);
            if (emp == null)
                return NotFound();

            return View(emp);
        }

        // POST: Delete Employee
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var emp = _context.Employees.Find(id);
            _context.Employees.Remove(emp);
            _context.SaveChanges();
            TempData["success"] = "Employee deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

    }
}
