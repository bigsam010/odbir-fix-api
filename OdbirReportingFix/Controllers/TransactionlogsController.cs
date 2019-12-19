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
    public class TransactionLogsController : ControllerBase
    {
        odbir_dbContext _context;
        public TransactionLogsController(odbir_dbContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<TransactionLogs[]>> Get()
        {
            try
            {
                return await _context.TransactionLogs.ToArrayAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<TransactionLogs>> Get(string Id)
        {
            var target = await _context.TransactionLogs.SingleOrDefaultAsync(obj => obj.Id == Id);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }

        [HttpPost]
        public ActionResult Post([FromBody] TransactionLogs[] obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state");
            }

            //var temp = _context.TransactionLogs;
            //foreach (var o in obj)
            //{
            //    if (temp.Where(t => t.PaymentReferenceNumber == o.PaymentReferenceNumber).Count() != 0)
            //    {
            //        Hashtable err = new Hashtable();
            //        err["cause"] = "Payment reference already exist";
            //        err["ref"] = o.PaymentReferenceNumber;
            //        return BadRequest(err);
            //    }
            //   System.IO.File.WriteAllText("mirror.txt", _context.TransactionLogs.Count().ToString());

            //    _context.TransactionLogs.Add(o);
            //}
            try
            {
                _context.TransactionLogs.AddRange(obj);
                _context.SaveChanges();
                return Created("api/TransactionLogs", obj);
            }
            catch (Exception ex)
            {
                if (ex.ToString().ToLower().Contains("tl_pref_uq"))
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

        //[HttpPut("{Id}")]
        //public async Task<ActionResult> Put(string Id, [FromBody] TransactionLogs obj)
        //{
        //    var target = await _context.TransactionLogs.SingleOrDefaultAsync(nobj => nobj.Id == Id);
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
        //    var target = await _context.TransactionLogs.SingleOrDefaultAsync(obj => obj.Id == Id);
        //    if (target != null)
        //    {
        //        _context.TransactionLogs.Remove(target);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    return NotFound();
        //}
    }
}