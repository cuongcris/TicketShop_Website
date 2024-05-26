using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketOnline.Data;
using TicketOnline.Model;

namespace TicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public EmployeesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(string id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(string id, EmpDTO employee)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp == null) { return  NotFound(); }

            if (!string.IsNullOrEmpty(employee.Cid))
                if (emp.Cid != employee.Cid) { emp.Cid = employee.Cid; }
            if (!string.IsNullOrEmpty(employee.Name))
                if (emp.Name != employee.Name) { emp.Name = employee.Name; }
            if (!string.IsNullOrEmpty(employee.Email))
                if (emp.Email != employee.Email) { emp.Email = employee.Email; }
            if(!string.IsNullOrEmpty(employee.StartDate))
                if(DateOnly.Parse(employee.StartDate) != emp.StartDate && employee.StartDate != null) { emp.StartDate = DateOnly.Parse(employee.StartDate); }
            if (!string.IsNullOrEmpty(employee.Address))
                if (emp.Address != employee.Address) { emp.Address = employee.Address; }
            if (!string.IsNullOrEmpty(employee.PhoneNumber))
                if (emp.PhoneNumber != employee.PhoneNumber) { emp.PhoneNumber = employee.PhoneNumber; }
            if (!string.IsNullOrEmpty(employee.Position))
                if (emp.Position != employee.Position && emp.Position != null) { emp.Position = employee.Position; }
            if (!string.IsNullOrEmpty(employee.Dob))
                if (emp.Dob != DateOnly.Parse(employee.Dob)) { emp.Dob = DateOnly.Parse(employee.Dob); }
            emp.UpdateAt = DateTime.UtcNow;

            _context.Entry(emp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(emp);
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmpDTO employee)
        {
            Employee employee1 = new Employee()
            {
                Address = employee.Address,
                Cid = employee.Cid,
                Email = employee.Email,
                Dob = DateOnly.Parse(employee.Dob),
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                Position = employee.Position,
                StartDate = DateOnly.Parse(employee.StartDate),
                CreateAt = DateTime.UtcNow
            };
            _context.Employees.Add(employee1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee1.Id }, employee1);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
