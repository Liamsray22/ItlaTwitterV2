using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataBase;
using DTO.DTO;
using DataBase.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository
{
    public class PublicacionesAPIRepo : RepositoryBase<Publicaciones, LIMBODBContext>
    {
        private readonly LIMBODBContext _context;
        private readonly UsuarioAPIRepo _usuarioAPIRepo;
        private readonly ComentariosAPIRepo _comentariosAPIRepo;
        private readonly ImagenesAPIRepo _imagenesAPIRepo;

        private readonly IMapper _mapper;


        public PublicacionesAPIRepo(LIMBODBContext context, IMapper mapper, 
                             UsuarioAPIRepo usuarioAPIRepo, ComentariosAPIRepo comentariosAPIRepo,
                            ImagenesAPIRepo imagenesAPIRepo) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _usuarioAPIRepo = usuarioAPIRepo;
            _comentariosAPIRepo = comentariosAPIRepo;
            _imagenesAPIRepo = imagenesAPIRepo;
        }
        //Obtener Publicaciones por el nombre de usuario
        public async Task<List<PublicacionesDTO>> TraerPubsByName(string name)
        {
            var user = await _usuarioAPIRepo.GetUsuarioByName(name);
            if (user != null) {
                List<PublicacionesDTO> list = new List<PublicacionesDTO>();
                var listapubs = await _context.Publicaciones.Where(x => x.IdUsuario == user.IdUsuarios).OrderByDescending(o => o.Fecha).ToListAsync();
                foreach (var p in listapubs)
                {
                    var pv = _mapper.Map<PublicacionesDTO>(p);
                    if (p.IdImagen != null)
                    {
                        var img = await _imagenesAPIRepo.GetByIdAsync(p.IdImagen.Value);
                        pv.Imagen = img.Ruta;
                    }
                    pv.Usuario = name;
                    pv.comentarios = await _comentariosAPIRepo.TraerComments(p.IdPublicacion);
                    list.Add(pv);
                }
                return list;
            }
            return null;
        }



        //Obtener Publicaciones con mas comentarios
        public async Task<PublicacionesDTO> TraerPubsMoreComments(int id)
        {
            var usus = await _usuarioAPIRepo.GetByIdAsync(id);
            if (usus != null) {
                var listaIdspubs = await _context.Publicaciones.Where(x => x.IdUsuario == id)
                    .OrderByDescending(o => o.Fecha).Select(s => s.IdPublicacion).ToListAsync();
                int valorMasAlto = 0;
                int IdPubMoreComments = 0;
                foreach (var idPub in listaIdspubs)
                {
                    var valor = _context.Comentarios.Where(w => w.IdPublicacion == idPub).Count();
                    if (valor > valorMasAlto)
                    {
                        IdPubMoreComments = idPub;
                        valorMasAlto = valor;
                    }
                }
                if (IdPubMoreComments != 0) {
                    var publ = await GetByIdAsync(IdPubMoreComments);
                    var pv = _mapper.Map<PublicacionesDTO>(publ);
                    if (publ.IdImagen != null)
                    {
                        var img = await _imagenesAPIRepo.GetByIdAsync(publ.IdImagen.Value);
                        pv.Imagen = img.Ruta;
                    }
                    pv.Usuario = await _usuarioAPIRepo.GetNombreUsuarioById(pv.IdUsuario);
                    pv.comentarios = await _comentariosAPIRepo.TraerComments(publ.IdPublicacion);
                    return pv;
                }
            }
            return null;

        }



        //Publicar
        public async Task<bool> Publicar(PublicarDTO publicar)
        {
            var confirm = await _usuarioAPIRepo.Login(publicar.Usuario, publicar.Clave);
            if (confirm)
            {
                var user = await _usuarioAPIRepo.GetUsuarioByName(publicar.Usuario);

                Publicaciones publicacion = new Publicaciones();
                publicacion.IdUsuario = user.IdUsuarios;
                publicacion.Publicacion = publicar.Publicacion;
                await AddAsync(publicacion);
                return true;

            }
            return false;
        }   
    }
}
