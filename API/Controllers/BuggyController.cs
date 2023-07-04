using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    
    public class BuggyController :BaseApiController
    {
        public StoreContext _context { get; }
        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetSecretText()
        {
            return "secret text";
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = _context.Products.Find(33);
            if(thing == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok();
        }
        
        [HttpGet("servererror")]
        public ActionResult GetServerErrorRequest()
        {
            var thing = _context.Products.Find(33);
            var thing1 = thing.ToString();
            return Ok();
        }        
        
        [HttpGet("badrequest")]
        public ActionResult GetBadRequestRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        
        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }
    }
}