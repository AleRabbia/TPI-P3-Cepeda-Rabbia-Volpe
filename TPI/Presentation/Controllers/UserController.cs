using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet ("{Id}")]
        public ActionResult GetById([FromRoute]Guid id)
        {
            return Ok();
        }
        
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok();
        }
    }
}
