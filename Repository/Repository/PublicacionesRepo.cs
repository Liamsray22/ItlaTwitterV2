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
    public class PublicacionesRepo : RepositoryBase<Publicaciones, LIMBODBContext>
    {
        private readonly LIMBODBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UsuarioRepo _usuarioRepo;
        private readonly ComentariosRepo _comentariosRepo;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment Hotin;
        private readonly ImagenesRepo _imagenesRepo;


        public PublicacionesRepo(LIMBODBContext context, UserManager<IdentityUser> userManager,
                            SignInManager<IdentityUser> signInManager, IMapper mapper, 
                            IHostingEnvironment Hotin, UsuarioRepo usuarioRepo, ComentariosRepo comentariosRepo,
                            ImagenesRepo imagenesRepo) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            this.Hotin = Hotin;
            _usuarioRepo = usuarioRepo;
            _comentariosRepo = comentariosRepo;
            _imagenesRepo = imagenesRepo;
        }

        public async Task<PublicacionesViewModel> TraerPubs(int id)
        {
            PublicacionesViewModel pvm = new PublicacionesViewModel();
            var listapubs = await _context.Publicaciones.Where(x => x.IdUsuario == id).OrderByDescending(o=>o.Fecha).ToListAsync();
            List<PublicacionesViewModel> list = new List<PublicacionesViewModel>();
            foreach (var p in listapubs)
            {
                var pv = _mapper.Map<PublicacionesViewModel>(p);
                var usupub = await _usuarioRepo.GetByIdAsync(id);
                try
                {
                    var imguser = await _imagenesRepo.GetByIdAsync(usupub.IdImagen.Value);
                    pv.ImagenUser = imguser.Ruta;
                }
                catch
                {

                }

                if (p.IdImagen != null)
                {
                    var img = await _imagenesRepo.GetByIdAsync(p.IdImagen.Value);
                    pv.Imagen = img.Ruta;
                }
                pv.Usuario = await _usuarioRepo.GetNombreUsuarioById(id);
                pv.comentarios = await _comentariosRepo.TraerComments(p.IdPublicacion);
                list.Add(pv);
            }
            pvm.publicaciones = list;
            pvm.IdUsuario = id;
            pvm.Usuario = await _usuarioRepo.GetNombreUsuarioById(id);
            var usu = await _usuarioRepo.GetUsuarioByName(pvm.Usuario);
            var im = await _imagenesRepo.GetByIdAsync(usu.IdImagen.Value);
            pvm.Imagen = im.Ruta;
            return pvm;
            
        }

        public async Task<PublicacionesViewModel> TraerPubById(int id)
        {
            var pub = await GetByIdAsync(id);
            var pv = _mapper.Map<PublicacionesViewModel>(pub);
            if (pub.IdImagen != null)
            {
                var img = await _context.Imagenes.FirstOrDefaultAsync(k => k.IdImagen == pub.IdImagen);
                pv.Imagen = img.Ruta;
            }
            pv.Usuario = await _usuarioRepo.GetNombreUsuarioById(pub.IdUsuario);

            return pv;
        }

        public async Task<List<PublicacionesViewModel>> TraerPubsAmigos(int id)
        {
            var listIdsFriends = _context.Amigos.Where(a => a.IdUsuario == id).Select(s=>s.IdAmigo).ToList();
            List<PublicacionesViewModel> pvmfriends = new List<PublicacionesViewModel>();
            foreach (int idi in listIdsFriends) {
                pvmfriends.Add(await TraerPubs(idi));
            }
            return pvmfriends;
        }
        public async Task<bool> CrearPubs(PublicacionesViewModel pub)
        {

            Publicaciones publicacion = new Publicaciones();
            publicacion.IdUsuario = pub.IdUsuario;
            publicacion.Publicacion = pub.Publicacion;

            string FileName = null;
            
            if (pub.FotoPub != null)
            {
                try
                {
                    string minipub = publicacion.Publicacion.Substring(0, 5);

                    Random r = new Random();
                    int codigo = r.Next(10000000, 99999999);
                    string Subir = Path.Combine(Hotin.WebRootPath, "images\\fotoPub");
                    FileName = minipub+ codigo + ".png";
                    string FilePath = Path.Combine(Subir, FileName);

                    pub.FotoPub.CopyTo(new FileStream(FilePath, FileMode.Create));
                    Imagenes img = new Imagenes();
                    img.Nombre = FileName;
                    img.Ruta = "images\\fotoPub\\" + FileName + "";
                    await _imagenesRepo.AddAsync(img);

                    var image = await _context.Imagenes.FirstOrDefaultAsync(d => d.Nombre.Contains(minipub + codigo));

                    publicacion.IdImagen = image.IdImagen;
                   
                }
                catch
                {
                    return false;
                }
            }

            await AddAsync(publicacion);
            //publicacion.IdImagen = pub
            return false;
        }

        public async Task<bool> EditarPubs(PublicacionesViewModel pub)
        {
            var publi = await GetByIdAsync(pub.IdPublicacion);
            publi.Publicacion = pub.Publicacion;
            string FileName = null;

            if (pub.FotoPub != null)
            {
                try
                {
                    string minipub = pub.Publicacion.Substring(0, 5);

                    Random r = new Random();
                    int codigo = r.Next(10000000, 99999999);
                    string Subir = Path.Combine(Hotin.WebRootPath, "images\\fotoPub");
                    FileName = minipub + codigo + ".png";
                    string FilePath = Path.Combine(Subir, FileName);

                    pub.FotoPub.CopyTo(new FileStream(FilePath, FileMode.Create));

                    if (publi.IdImagen != null) {
                        var img = await _imagenesRepo.GetByIdAsync(publi.IdImagen.Value);
                        _imagenesRepo.Borrar(Path.Combine(Hotin.WebRootPath, img.Ruta));

                        img.Nombre = FileName;
                        img.Ruta = "images\\fotoPub\\" + FileName + "";
                        await _imagenesRepo.Update(img);
                    }
                    else
                    {
                        Imagenes img = new Imagenes();
                        img.Nombre = FileName;
                        img.Ruta = "images\\fotoPub\\" + FileName + "";
                        await _context.Imagenes.AddAsync(img);

                        var image = await _context.Imagenes.FirstOrDefaultAsync(d => d.Nombre.Contains(minipub + codigo));

                        publi.IdImagen = image.IdImagen;


                    }

                    return true;

                }
                catch
                {
                    return false;
                }

            }
            await Update(publi);


            return true;
        }


        public async Task<bool> EliminarPub(int id)
        {
            try
            {
                var p = await GetByIdAsync(id);
                if (p.IdImagen != null)
                {
                    var img = await _imagenesRepo.GetByIdAsync(p.IdImagen.Value);
                    _imagenesRepo.Borrar(Path.Combine(Hotin.WebRootPath, img.Ruta));
                    await _imagenesRepo.DeleteEntity(img);


                }
                await DeleteEntity(p);
            }
            catch
            {
                return false;

            }
            return true;
        }




    }
}
