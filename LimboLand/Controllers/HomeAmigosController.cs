using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Repository;

namespace LimboLand.Controllers
{
    [Authorize]
    public class HomeAmigosController : Controller
    {
        private readonly UsuarioRepo _usuarioRepo;
        private readonly PublicacionesRepo _publicacionesRepo;
        private readonly ComentariosRepo _comentariosRepo;


        public HomeAmigosController(UsuarioRepo usuarioRepo, PublicacionesRepo publicacionesRepo,
            ComentariosRepo comentariosRepo)
        {
            _usuarioRepo = usuarioRepo;
            _publicacionesRepo = publicacionesRepo;
            _comentariosRepo = comentariosRepo;

        }
        public async Task<int> IdUser()
        {
            var usu = await _usuarioRepo.GetUsuarioByName(User.Identity.Name);
            return usu.IdUsuarios;
        }


        public async Task<IActionResult> Index()
        {
            var pubs = await _publicacionesRepo.TraerPubsAmigos(await IdUser());
            ViewBag.IdLog = await IdUser();
            return View(pubs);
        }
    }
}