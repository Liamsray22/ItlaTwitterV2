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
        private readonly IHostingEnvironment Hotin;


        public ComentariosRepo(LIMBODBContext context, UserManager<IdentityUser> userManager,
                            SignInManager<IdentityUser> signInManager, IMapper mapper, 
                            IHostingEnvironment Hotin, UsuarioRepo usuarioRepo) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            this.Hotin = Hotin;
            _usuarioRepo = usuarioRepo;
            //TarjetadeUsuario = new TarjetadeUsuario(context);
        }

        public async Task<List<ComentariosViewModel>> TraerComments(int id)
        {
            var comentarios = await _context.Comentarios.Where(x=>x.IdPublicacion == id).ToListAsync();
            List<ComentariosViewModel> listcvm = new List<ComentariosViewModel>();
            foreach (var com in comentarios)
            {
                var coment = _mapper.Map<ComentariosViewModel>(com);
                coment.Usuario = await _usuarioRepo.GetNombreUsuarioById(coment.IdUsuario);
                var replysIds = _context.Comentarios2.Where(c => c.IdComentarioPadre == coment.IdComentario).
                    Select(s=>s.IdComentarioHijo).ToList();
                List<ComentariosViewModel> comen2 = new List<ComentariosViewModel>();
                foreach (int ide in replysIds)
                {
                    var Comentariohijo = await GetByIdAsync(ide);
                    var comentarito = _mapper.Map<ComentariosViewModel>(Comentariohijo);
                    comentarito.Usuario = await _usuarioRepo.GetNombreUsuarioById(Comentariohijo.IdUsuario);
                    comen2.Add(comentarito);
                }
                coment.comentarios2 = comen2;
                listcvm.Add(coment);
            }
            return listcvm;
            
        }

        public async Task CrearComent(ComentariosViewModel cvm)
        {
            var comentario = _mapper.Map<Comentarios>(cvm);
            await AddAsync(comentario);
        }

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
