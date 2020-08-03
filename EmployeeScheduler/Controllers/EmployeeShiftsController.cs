using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeScheduler.Data;
using EmployeeScheduler.Models;
using static EmployeeScheduler.Models.Enums;
using Microsoft.Extensions.Logging;

namespace EmployeeScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeShiftsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeShiftsController> _logger;

        public EmployeeShiftsController(ApplicationDbContext context, ILogger<EmployeeShiftsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/EmployeeShifts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeShifts>>> GetEmployeeShifts()
        {
            try
            {
                return await _context.EmployeeShifts.Include(x => x.Employee).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while getting employee shifts", ex);
                return null;
            }

        }

        // GET: api/EmployeeShifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeShifts>> GetEmployeeShifts(Guid id)
        {
            EmployeeShifts employeeShifts = null;
            try
            {
                employeeShifts = await _context.EmployeeShifts.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while getting employee shift", ex);
            }


            if (employeeShifts == null)
            {
                return NotFound();
            }

            return employeeShifts;
        }

        // PUT: api/EmployeeShifts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeShifts(Guid id, EmployeeShifts employeeShifts)
        {
            if (id != employeeShifts.UID)
            {
                return BadRequest();
            }

            _context.Entry(employeeShifts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EmployeeShiftsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Exception while updateing employee shift", ex);
                    return StatusCode((int)Status.Failure, new { message = "Failed to update employee shift" });
                }
            }

            return StatusCode((int)Status.Success, new { message = "Employee shift updated successfully" });
        }

        // POST: api/EmployeeShifts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EmployeeShifts>> PostEmployeeShifts(EmployeeShifts employeeShifts)
        {
            try
            {
                _context.EmployeeShifts.Add(employeeShifts);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEmployeeShifts", new { id = employeeShifts.UID }, employeeShifts);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while creating employee shift", ex);
                return StatusCode((int)Status.Failure, new { message = "Exception while creating employee shift. Exception: " + ex.StackTrace });
            }
        }

        // DELETE: api/EmployeeShifts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeShifts>> DeleteEmployeeShifts(Guid id)
        {
            var employeeShifts = await _context.EmployeeShifts.FindAsync(id);
            if (employeeShifts == null)
            {
                return NotFound();
            }

            _context.EmployeeShifts.Remove(employeeShifts);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while deleting employee shift", ex);
            }


            return employeeShifts;
        }

        [HttpGet("[action]/{firstSwapId}/{SecondSwapId}")]
        public async Task<IActionResult> SwapShift(Guid firstSwapId, Guid SecondSwapId)
        {
            var firstSwap = await _context.EmployeeShifts.FirstOrDefaultAsync(x => x.UID == firstSwapId);
            if (firstSwap == null)
                return StatusCode((int)Status.Failure, new { message = "Record to swap not found" });
            var secondSwap = await _context.EmployeeShifts.FirstOrDefaultAsync(x => x.UID == SecondSwapId);
            if (secondSwap == null)
                return StatusCode((int)Status.Failure, new { message = "Record to swap not found" });

            if (IsValidSwap(firstSwap, secondSwap))
                return StatusCode((int)Status.Success, new { message = "Swap successfull" });
            return StatusCode((int)Status.Failure, new { message = "Employee not allowed to swap in this time" });
        }

        private bool IsValidSwap(EmployeeShifts shift1, EmployeeShifts shift2)
        {
            if ((shift1.StartTime > shift2.StartTime && shift1.StartTime < shift2.EndTime)
                || (shift1.EndTime > shift2.StartTime && shift1.EndTime < shift2.EndTime))
            {
                return false;
            }
            return true;
        }

        private bool EmployeeShiftsExists(Guid id)
        {
            return _context.EmployeeShifts.Any(e => e.UID == id);
        }
    }
}
