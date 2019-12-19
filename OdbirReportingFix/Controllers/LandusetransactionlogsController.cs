using System;
using System.Collections;
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
    public class LandUseTransactionLogsController : ControllerBase
    {
        odbir_dbContext _context;
        public LandUseTransactionLogsController(odbir_dbContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<LandUseTransactionLogs[]>> Get()
        {
            return await _context.LandUseTransactionLogs.ToArrayAsync();
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<LandUseTransactionLogs>> Get(string Id)
        {
            var target = await _context.LandUseTransactionLogs.SingleOrDefaultAsync(obj => obj.Id == Id);
            if (target == null)
            {
                return BadRequest("Invalid Id");
            }
            return target;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LandUseTransactionLogs[] obj)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state");
            }
            else
            {
                //foreach (var o in obj)
                //{
                //    if (_context.LandUseTransactionLogs.Where(t => t.PaymentReferenceNumber == o.PaymentReferenceNumber).Count() != 0)
                //    {
                //        Hashtable err = new Hashtable();
                //        err["cause"] = "Payment reference already exist";
                //        err["ref"] = o.PaymentReferenceNumber;
                //        return BadRequest(err);
                //    }

                //    _context.LandUseTransactionLogs.Add(o);
                //}
                try
                {
                    _context.AddRange(obj);
                    await _context.SaveChangesAsync();
                    return Created("api/LandUseTransactionLogs", obj);
                }
                catch (Exception ex)
                {
                    if (ex.ToString().ToLower().Contains("lt_pref_uq"))
                    {
                        string target = ex.ToString();
                        string[] lines = target.Split('\n');
                        string al = lines[0];
                        Hashtable err = new Hashtable();
                        err["cause"] = "Payment reference already exist";
                        int length = al.LastIndexOf(')') - (target.IndexOf("is") + 3) + 1;
                        err["ref"] = al.Substring(target.IndexOf("is") + 3, length);
                        return BadRequest(err);
                    }
                    return StatusCode(500, "Error: " + ex);
                }

            }
        }

        //[HttpPut("{Id}")]
        //public async Task<ActionResult> Put(string Id, [FromBody] LandUseTransactionLogs obj)
        //{
        //    var target = await _context.LandUseTransactionLogs.SingleOrDefaultAsync(nobj => nobj.Id == Id);
        //    if (target != null && ModelState.IsValid)
        //    {
        //        _context.Entry(target).CurrentValues.SetValues(obj);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    return BadRequest();
        //}

        //[HttpDelete("{Id}")]
        //public async Task<ActionResult> Delete(string Id)
        //{
        //    var target = await _context.LandUseTransactionLogs.SingleOrDefaultAsync(obj => obj.Id == Id);
        //    if (target != null)
        //    {
        //        _context.LandUseTransactionLogs.Remove(target);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    return NotFound();
        //}
    }
}