using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataBase;
using DataBase.Models;
using DataBase.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository
{
    public class ComentariosRepo : RepositoryBase<Comentarios, LIMBODBContext>
    {
        private readonly LIMBODBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UsuarioRepo _usuarioRepo;
        private readonly IMapper _mapper;
        private readonly ImagenesRepo _imagenesRepo;


        public ComentariosRepo(LIMBODBContext context, UserManager<IdentityUser> userManager,
                            SignInManager<IdentityUser> signInManager, IMapper mapper, 
                            UsuarioRepo usuarioRepo, ImagenesRepo imagenesRepo) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _usuarioRepo = usuarioRepo;
            _imagenesRepo = imagenesRepo;
        }
        //Traer Comentarios
        public async Task<List<ComentariosViewModel>> TraerComments(int id)
        {
            var comentarios = await _context.Comentarios.Where(x=>x.IdPublicacion == id).ToListAsync();
            List<ComentariosViewModel> listcvm = new List<ComentariosViewModel>();
            foreach (var com in comentarios)
            {
                var coment = _mapper.Map<ComentariosViewModel>(com);
                var usucoment = await _usuarioRepo.GetByIdAsync(coment.IdUsuario.Value);
                coment.Usuario = usucoment.Usuario;
                try
                {
                    var imguser = await _imagenesRepo.GetByIdAsync(usucoment.IdImagen.Value);
                    coment.ImagenUser = imguser.Ruta;
                }
                catch
                {

                }

                var replysIds = _context.Comentarios2.Where(c => c.IdComentarioPadre == coment.IdComentario).
                    Select(s=>s.IdComentarioHijo).ToList();
                List<ComentariosViewModel> comen2 = new List<ComentariosViewModel>();
                foreach (int ide in replysIds)
                {
                    var Comentariohijo = await GetByIdAsync(ide);
                    var comentarito = _mapper.Map<ComentariosViewModel>(Comentariohijo);
                    var usucomentarito = await _usuarioRepo.GetByIdAsync(comentarito.IdUsuario.Value);
                    comentarito.Usuario = usucomentarito.Usuario;
                    try
                    {
                        var imgusercomentarito = await _imagenesRepo.GetByIdAsync(usucomentarito.IdImagen.Value);
                        comentarito.ImagenUser = imgusercomentarito.Ruta;
                    }
                    catch
                    {

                    }
                    comen2.Add(comentarito);
                }
                coment.comentarios2 = comen2;
                listcvm.Add(coment);
            }
            return listcvm;
            
        }

        //Crear Comentarios
        public async Task CrearComent(ComentariosViewModel cvm)
        {
            var comentario = _mapper.Map<Comentarios>(cvm);
            await AddAsync(comentario);
        }

        //Crear Respuesta
        public async Task CrearRespuesta(RespuestaViewModel res)
        {
            var comentario = _mapper.Map<Comentarios>(res);
            await AddAsync(comentario);
            Comentarios2 cm2 = new Comentarios2();
            cm2.IdComentarioHijo = comentario.IdComentario;
            cm2.IdComentarioPadre = res.IdComentarioPadre;
            await _context.Comentarios2.AddAsync(cm2);
            await _context.SaveChangesAsync();
        }


    }
}
