using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO.DTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Repository;

namespace APILimboLand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public readonly PublicacionesAPIRepo _publicacionesAPIRepo;
        public ValuesController(PublicacionesAPIRepo publicacionesAPIRepo)
        {
            _publicacionesAPIRepo = publicacionesAPIRepo;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("GetByUserName/{username}")]
        public async Task< ActionResult<List<PublicacionesDTO>>> GetByUsername(string username)
        {
            if (username != null || username != "") {
                var Pubs = await _publicacionesAPIRepo.TraerPubsByName(username);
                return Pubs;

            }
            return NotFound();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
