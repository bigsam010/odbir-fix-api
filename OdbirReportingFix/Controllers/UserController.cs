using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdbirReportingFix.Models;
namespace OdbirReportingFix.Controllers
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        odbir_dbContext _context;
        public UserController(odbir_dbContext _context)
        {
            this._context = _context;
        }
        [Route("[action]/{email}/{password}")]
        [HttpGet]
        public ActionResult Authenticate(string email, string password)
        {
            return Ok((email.ToLower() == "clients@prunedge.com" && password == "Prunedge@123") || (email.ToLower() == "admin@odbir.com" && password == "Password@19"));
        }


    }
}