using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeScheduler.Data;
using static EmployeeScheduler.Models.Enums;
using EmployeeScheduler.Models;
using Microsoft.Extensions.Logging;

namespace EmployeeScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ApplicationDbContext context, ILogger<EmployeesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(Guid id)
        {
            Employee employee = null;
            try
            {
                employee = await _context.Employees.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while finding an employee", ex);
            }

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(Guid id, Employee employee)
        {
            if (id != employee.UID)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Exception while updating employee", ex);
                    return StatusCode((int)Status.Failure, new { message = "Exception while updating employee" });
                }
            }

            return StatusCode((int)Status.Success, new { message = "Successfully updated employee" });
        }

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            try
            {
                if (await _context.Employees.AnyAsync(x => x.Email == employee.Email))
                    return StatusCode((int)Status.Failure, new { message = "Email already exist" });
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEmployee", new { id = employee.UID }, employee);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while creating employee", ex);
                return StatusCode((int)Status.Failure, new { message = "Exception while creating employee. Exception: " + ex.StackTrace });
            }

        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while deleting an employee", ex);
            }

            return employee;
        }

        private bool EmployeeExists(Guid id)
        {
            return _context.Employees.Any(e => e.UID == id);
        }
    }
}
