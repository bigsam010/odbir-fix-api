using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdbirReportingFix.Models;
namespace OdbirReportingFix.Controllers
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class RevenuesController : ControllerBase
    {
        odbir_dbContext _context;
        public RevenuesController(odbir_dbContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<Revenues[]>> Get()
        {
            return await _context.Revenues.ToArrayAsync();
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Revenues>> Get(Guid Id)
        {
            var target = await _context.Revenues.SingleOrDefaultAsync(obj => obj.Id == Id);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Revenues obj)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state");
                }
                else
                {
                    var target = _context.Revenues.Where(r => r.Date.Month == obj.Date.Month && r.Date.Year == obj.Date.Year && r.TaxStationRevenueTargetId == obj.TaxStationRevenueTargetId);
                    if (target.Count() != 0)
                    {
                        target.First().Amount = obj.Amount;
                        await _context.SaveChangesAsync();
                        return Ok(target.First());
                    }
                    _context.Revenues.Add(obj);
                    await _context.SaveChangesAsync();
                    return Created("api/Revenues", obj);
                }
            }
            catch (Exception ex) {
                return StatusCode(500, "Error: " + ex);
            }
        }

        [HttpPut("{month}/{year}/{targetid}")]
        public async Task<ActionResult> Put(int month, int year, Guid targetid, [FromBody] Revenues obj)
        {
            var target = _context.Revenues.Where(r => r.Date.Month == month && r.Date.Year == year && r.TaxStationRevenueTargetId == targetid);
            if (target.Count() != 0)
            {
                target.First().Amount = obj.Amount;
                await _context.SaveChangesAsync();
                return Ok(target.First());
            }
            return BadRequest("No record found for the specified parameters");
        }

        //[HttpDelete("{Id}")]
        //public async Task<ActionResult> Delete(Guid Id)
        //{
        //    var target = await _context.Revenues.SingleOrDefaultAsync(obj => obj.Id == Id);
        //    if (target != null)
        //    {
        //        _context.Revenues.Remove(target);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    return NotFound();
        //}
    }
}