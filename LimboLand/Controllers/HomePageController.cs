using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Repository;

namespace LimboLand.Controllers
{
    [Authorize]
    public class HomePageController : Controller
    {
        private readonly UsuarioRepo _usuarioRepo;
        private readonly PublicacionesRepo _publicacionesRepo;
        private readonly ComentariosRepo _comentariosRepo;


        public HomePageController(UsuarioRepo usuarioRepo, PublicacionesRepo publicacionesRepo,
            ComentariosRepo comentariosRepo)
        {
            _usuarioRepo = usuarioRepo;
            _publicacionesRepo = publicacionesRepo;
            _comentariosRepo = comentariosRepo;

        }
        public async Task< int> IdUser()
        {
            var usu = await _usuarioRepo.GetUsuarioByName(User.Identity.Name);
            return usu.IdUsuarios;
        }

        public async Task<IActionResult> Index()
        {

            var pubs = await _publicacionesRepo.TraerPubs(await IdUser());
            pubs.Usuario = User.Identity.Name;
            return View(pubs);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PublicacionesViewModel publicacionesViewModel)
        {
            if (ModelState.IsValid)
            {
                await _publicacionesRepo.CrearPubs(publicacionesViewModel);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Comentar(ComentariosViewModel cm)
        {
            await _comentariosRepo.CrearComent(cm);
            if (cm.Manda == 2)
            {
                return RedirectToAction("Index", "HomeAmigos");

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Responder(RespuestaViewModel respuesta)
        {
            await _comentariosRepo.CrearRespuesta(respuesta);
            if (respuesta.Manda == 2)
            {
                return RedirectToAction("Index", "HomeAmigos");

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditarPub(int id)
        {
            var editar = await _publicacionesRepo.TraerPubById(id);
            return View(editar);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPub(PublicacionesViewModel edit)
        {
            if (ModelState.IsValid)
            {
                await _publicacionesRepo.EditarPubs(edit);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }


        }
}