using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataBase;
using DataBase.Models;
using DTO.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository
{
    public class ComentariosAPIRepo : RepositoryBase<Comentarios, LIMBODBContext>
    {
        private readonly LIMBODBContext _context;
        private readonly UsuarioAPIRepo _usuarioAPIRepo;
        private readonly IMapper _mapper;


        public ComentariosAPIRepo(LIMBODBContext context, IMapper mapper, 
                            IHostingEnvironment Hotin, UsuarioAPIRepo usuarioAPIRepo) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _usuarioAPIRepo = usuarioAPIRepo;
        }

        public async Task<List<ComentariosDTO>> TraerComments(int id)
        {
            var comentarios = await _context.Comentarios.Where(x=>x.IdPublicacion == id).ToListAsync();
            List<ComentariosDTO> listcvm = new List<ComentariosDTO>();
            foreach (var com in comentarios)
            {
                var coment = _mapper.Map<ComentariosDTO>(com);
                coment.Usuario = await _usuarioAPIRepo.GetNombreUsuarioById(coment.IdUsuario);
                var replysIds = _context.Comentarios2.Where(c => c.IdComentarioPadre == coment.IdComentario).
                    Select(s=>s.IdComentarioHijo).ToList();
                List<ComentariosDTO> comen2 = new List<ComentariosDTO>();
                foreach (int ide in replysIds)
                {
                    var Comentariohijo = await GetByIdAsync(ide);
                    var comentarito = _mapper.Map<ComentariosDTO>(Comentariohijo);
                    comentarito.Usuario = await _usuarioAPIRepo.GetNombreUsuarioById(Comentariohijo.IdUsuario);
                    comen2.Add(comentarito);
                }
                coment.respuestas = comen2;
                listcvm.Add(coment);
            }
            return listcvm;
            
        }

        //public async Task CrearComent(ComentariosViewModel cvm)
        //{
        //    var comentario = _mapper.Map<Comentarios>(cvm);
        //    await AddAsync(comentario);
        //}

        //public async Task CrearRespuesta(RespuestaViewModel res)
        //{
        //    var comentario = _mapper.Map<Comentarios>(res);
        //    await AddAsync(comentario);
        //    Comentarios2 cm2 = new Comentarios2();
        //    cm2.IdComentarioHijo = comentario.IdComentario;
        //    cm2.IdComentarioPadre = res.IdComentarioPadre;
        //    await _context.Comentarios2.AddAsync(cm2);
        //    await _context.SaveChangesAsync();
        //}


    }
}
