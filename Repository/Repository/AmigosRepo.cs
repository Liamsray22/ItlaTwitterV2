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
    public class AmigosRepo : RepositoryBase<Amigos, LIMBODBContext>
    {
        private readonly LIMBODBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UsuarioRepo _usuarioRepo;
        private readonly ImagenesRepo _imagenesRepo;
        private readonly IMapper _mapper;


        public AmigosRepo(LIMBODBContext context, UserManager<IdentityUser> userManager,
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

        

        public async Task<List<ListaAmigosViewModel>> TraerListaAmigos (int id)
        {
            var listIdsAmigos = _context.Amigos.Where(l => l.IdUsuario == id).Select(s => s.IdAmigo).ToList();
            List<ListaAmigosViewModel> listaAmigos = new List<ListaAmigosViewModel>();
            foreach(int i in listIdsAmigos)
            {
                var user = await _usuarioRepo.GetByIdAsync(i);
                var amigo = _mapper.Map<ListaAmigosViewModel>(user);
                try
                {
                    var img = await _imagenesRepo.GetByIdAsync(user.IdImagen.Value);
                    amigo.ImagenUser = img.Ruta;
                }
                catch
                {

                }
                listaAmigos.Add(amigo);
            }
            return listaAmigos;
        }

        public async Task<List<ListaAmigosViewModel>> BuscarPersonas(string user)
        {
            var personas = await _context.Usuarios.Where(w=>w.Usuario.Contains(user)).ToListAsync();
            List<ListaAmigosViewModel> listaAmigos = new List<ListaAmigosViewModel>();
            foreach (var u in personas)
            {
                var amigo = _mapper.Map<ListaAmigosViewModel>(u);
                try
                {
                    var img = await _imagenesRepo.GetByIdAsync(u.IdImagen.Value);
                    amigo.ImagenUser = img.Ruta;
                }
                catch
                {

                }
                listaAmigos.Add(amigo);
                
            }
            return listaAmigos;
        }

        public async Task AgregarAmigos(int IdUsuario, int IdAmigo)
        {
            try
            {
                Amigos ad = new Amigos();
                ad.IdUsuario = IdUsuario;
                ad.IdAmigo = IdAmigo;
                await AddAsync(ad);
                ad.IdUsuario = IdAmigo;
                ad.IdAmigo = IdUsuario;
                await AddAsync(ad);
            }
            catch
            {

            }

        }

        public async Task BorrarAmigos(int IdUsuario, int IdAmigo)
        {
            try
            {
                Amigos ad = new Amigos();
            ad.IdUsuario = IdUsuario;
            ad.IdAmigo = IdAmigo;
            await DeleteEntity(ad);

            Amigos ad2 = new Amigos();
            ad2.IdUsuario = IdAmigo;
            ad2.IdAmigo = IdUsuario;
            await DeleteEntity(ad2);
            }
            catch
            {

            }
        }

    }
}
