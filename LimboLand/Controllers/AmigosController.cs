using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Repository;

namespace LimboLand.Controllers
{
    [Authorize]
    public class AmigosController : Controller
    {
        private readonly UsuarioRepo _usuarioRepo;
        private readonly PublicacionesRepo _publicacionesRepo;
        private readonly AmigosRepo _amigosRepo;


        public AmigosController(UsuarioRepo usuarioRepo, PublicacionesRepo publicacionesRepo,
            AmigosRepo amigosRepo)
        {
            _usuarioRepo = usuarioRepo;
            _publicacionesRepo = publicacionesRepo;
            _amigosRepo = amigosRepo;

        }
        //Id del Usuario Logueado
        #region IdUser
        public async Task<int> IdUser()
        {
            var usu = await _usuarioRepo.GetUsuarioByName(User.Identity.Name);
            return usu.IdUsuarios;
        }
        #endregion


        //Listado de Amigos
        #region ListaAmigos
        public async Task<IActionResult> ListaAmigos()
        {
            var friends = await _amigosRepo.TraerListaAmigos(await IdUser());
            return View(friends);
        }
        #endregion


        //Buscar Personas
        #region BuscarPersonas
        public async Task<IActionResult> Buscar(string Name)
        {
            var Personas = await _amigosRepo.BuscarPersonas(Name);
            return View(Personas);
        }
        #endregion


        //Agregar Amigos
        #region AgregarAmigos
        public async Task<IActionResult> Agregar(int IdAmigo)
        {
            await _amigosRepo.AgregarAmigos(await IdUser(), IdAmigo);
            return RedirectToAction("ListaAmigos");
        }
        #endregion


        //Eliminar Amigos
        #region EliminarAmigos
        public async Task<IActionResult> EliminarAmigos(int id)
        {
            await _amigosRepo.BorrarAmigos(await IdUser(), id);
            return RedirectToAction("ListaAmigos");
        }
        #endregion

    }
}