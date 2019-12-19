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
    public class TaxStationRevenueTargetsController : ControllerBase
    {
        odbir_dbContext _context;
        public TaxStationRevenueTargetsController(odbir_dbContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<TaxStationRevenueTargets[]>> Get()
        {
            return await _context.TaxStationRevenueTargets.ToArrayAsync();
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult> GetStationNames()
        {
            return Ok(await _context.TaxStationRevenueTargets.Select(t => t.TaxStationName).Distinct().ToListAsync());
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<TaxStationRevenueTargets>> Get(Guid Id)
        {
            var target = await _context.TaxStationRevenueTargets.SingleOrDefaultAsync(obj => obj.Id == Id);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }

        class AdvStationTarget : TaxStationRevenueTargets
        {
            public decimal TotalRemittance { set; get; }
            public AdvStationTarget(TaxStationRevenueTargets target) {
                this.Id = target.Id;
                this.AnnualTarget = target.AnnualTarget;
                this.Date = target.Date;
                this.MonthlyTarget = target.MonthlyTarget;
                this.Revenues = target.Revenues;
                this.TaxStationName = target.TaxStationName;
                this.Year = target.Year;
               
            }
        }

        [Route("[action]/{station}/{year}")]
        [HttpGet]
        public ActionResult GetTarget(string station, int year)
        {
            var target = _context.TaxStationRevenueTargets.Where(t => t.TaxStationName == station && t.Year == year.ToString());
            if (target.Count() > 0) {
                var advst = new AdvStationTarget(target.First());
                advst.TotalRemittance=_context.Revenues.Where(r=>r.TaxStationRevenueTargetId==target.First().Id).Sum(r=>r.Amount);
                return Ok(advst);
            }
            return Ok(target);
        }

        [HttpPost]
        public async Task<ActionResult<TaxStationRevenueTargets>> Post([FromBody] TaxStationRevenueTargets obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state");
            }
            else
            {
                var target = _context.TaxStationRevenueTargets.Where(t => t.TaxStationName == obj.TaxStationName && t.Year == obj.Year);
                if (target.Count() != 0)
                {
                    target.First().AnnualTarget = obj.AnnualTarget;
                    target.First().MonthlyTarget = obj.AnnualTarget / 12;
                    await _context.SaveChangesAsync();
                    return Ok(target.First());
                }
                obj.MonthlyTarget = obj.AnnualTarget / 12;
                _context.TaxStationRevenueTargets.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/TaxStationRevenueTargets", obj);
            }
        }

        [HttpPut("{station}/{year}")]
        public async Task<ActionResult> Put(string station, int year, [FromBody] TaxStationRevenueTargets obj)
        {
            var target = _context.TaxStationRevenueTargets.Where(nobj => nobj.TaxStationName == station && nobj.Year == year.ToString());
            if (target.Count() != 0 && ModelState.IsValid)
            {
                // _context.Entry(target.First()).CurrentValues.SetValues(obj);
                target.First().AnnualTarget = obj.AnnualTarget;
                target.First().MonthlyTarget = obj.AnnualTarget / 12;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("No record found for the given tax station and year");
        }

        //[HttpDelete("{Id}")]
        //public async Task<ActionResult> Delete(Guid Id)
        //{
        //    var target = await _context.TaxStationRevenueTargets.SingleOrDefaultAsync(obj => obj.Id == Id);
        //    if (target != null)
        //    {
        //        _context.TaxStationRevenueTargets.Remove(target);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    return NotFound();
        //}
    }
}