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
        public readonly AmigosAPIRepo _amigosAPIRepo;

        public ValuesController(PublicacionesAPIRepo publicacionesAPIRepo, AmigosAPIRepo amigosAPIRepo)
        {
            _publicacionesAPIRepo = publicacionesAPIRepo;
            _amigosAPIRepo = amigosAPIRepo;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        //Obtener Publicaciones por el nombre de usuario
        [HttpGet]
        [Route("GetPubsByUsername/{username}")]
        public async Task< ActionResult<List<PublicacionesDTO>>> GetPubsByUsername(string username)
        {
            if (username != null || username != "") {
                var Pubs = await _publicacionesAPIRepo.TraerPubsByName(username);
                return Pubs;

            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetFriendListByUsername/{username}")]
        public async Task<ActionResult<List<ListaAmigosDTO>>> GetFriendListByUsername(string username)
        {
            if (username != null || username != "")
            {
                var Friends = await _amigosAPIRepo.TraerListaAmigos(username);
                return Friends;

            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetPubsMoreComments/{id}")]
        public async Task<ActionResult<PublicacionesDTO>> GetPubsMoreComments(int? id)
        {
            if (id != null)
            {
                var Pubs = await _publicacionesAPIRepo.TraerPubsMoreComments(id.Value);
                if (Pubs != null)
                {
                    return Pubs;
                }
                return StatusCode(500);

            }
            return NotFound();
        }


        // POST api/values
        [HttpPost]
        [Route("Publicar")]
        public async Task<IActionResult> Publicar(PublicarDTO publicar)
        {
            if (ModelState.IsValid)
            {
                var publi = await _publicacionesAPIRepo.Publicar(publicar);
                if (publi)
                {
                    return NoContent();
                }
            }
            return StatusCode(500);
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
