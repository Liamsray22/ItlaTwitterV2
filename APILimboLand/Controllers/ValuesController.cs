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

        //Obtener Publicaciones por el nombre de usuario
        #region GetPubsByUsername
        [HttpGet]
        [Route("GetPubsByUsername/{username}")]
        public async Task< ActionResult<List<PublicacionesDTO>>> GetPubsByUsername(string username)
        {
            if (username != null || username != "") {
                var Pubs = await _publicacionesAPIRepo.TraerPubsByName(username);
                if (Pubs != null)
                {
                    return Pubs;
                }
            }
            return NotFound();
        }
        #endregion



        //Obtener Lista de Amigos mediante el nombre de usuario
        #region GetFriendListByUsername
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
        #endregion



        //Obtener Publicaciones con mas comentarios
        #region GetPubsMoreComments
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
                return NotFound();

            }
            return NotFound();
        }
        #endregion



        // Publicar
        #region Publicar
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
        #endregion



        //Agregar Amigoss
        #region AgregarAmigos
        [HttpPost]
        [Route("AgregarAmigos/{id}")]
        public async Task<IActionResult> AgregarAmigos(AgregarAmigosDTO agregar, int id)
        {
            if (ModelState.IsValid)
            {
                var friend = await _amigosAPIRepo.AgregarAmigos(agregar, id);
                if (friend)
                {
                    return NoContent();
                }
            }
            return StatusCode(500);
        }
        #endregion
    }
}
