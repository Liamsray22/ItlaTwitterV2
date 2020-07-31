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
    public class AmigosAPIRepo : RepositoryBase<Amigos, LIMBODBContext>
    {
        private readonly LIMBODBContext _context;
        private readonly UsuarioAPIRepo _usuarioAPIRepo;
        private readonly IMapper _mapper;


        public AmigosAPIRepo(LIMBODBContext context, IMapper mapper,
                            UsuarioAPIRepo usuarioAPIRepo) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _usuarioAPIRepo = usuarioAPIRepo;
        }



        public async Task<List<ListaAmigosDTO>> TraerListaAmigos(string name)
        {
            var user = await _usuarioAPIRepo.GetUsuarioByName(name);

            var listIdsAmigos = _context.Amigos.Where(l => l.IdUsuario == user.IdUsuarios).Select(s => s.IdAmigo).ToList();
            List<ListaAmigosDTO> listaAmigos = new List<ListaAmigosDTO>();
            foreach (int i in listIdsAmigos)
            {
                var usera = await _usuarioAPIRepo.GetByIdAsync(i);
                var amigo = _mapper.Map<ListaAmigosDTO>(usera);
                listaAmigos.Add(amigo);
            }
            return listaAmigos;
        }

        //public async Task<List<ListaAmigosViewModel>> BuscarPersonas(string user)
        //{
        //    var personas = await _context.Usuarios.Where(w=>w.Usuario.Contains(user)).ToListAsync();
        //    List<ListaAmigosViewModel> listaAmigos = new List<ListaAmigosViewModel>();
        //    foreach (var u in personas)
        //    {
        //        var amigo = _mapper.Map<ListaAmigosViewModel>(u);
        //        listaAmigos.Add(amigo);
        //    }

        //    return listaAmigos;
        //}

        //public async Task AgregarAmigos(int IdUsuario, int IdAmigo)
        //{
        //    try
        //    {
        //        Amigos ad = new Amigos();
        //        ad.IdUsuario = IdUsuario;
        //        ad.IdAmigo = IdAmigo;
        //        await AddAsync(ad);
        //        ad.IdUsuario = IdAmigo;
        //        ad.IdAmigo = IdUsuario;
        //        await AddAsync(ad);
        //    }
        //    catch
        //    {

        //    }

        //}

        //public async Task BorrarAmigos(int IdUsuario, int IdAmigo)
        //{
        //    try
        //    {
        //        Amigos ad = new Amigos();
        //    ad.IdUsuario = IdUsuario;
        //    ad.IdAmigo = IdAmigo;
        //    await DeleteEntity(ad);

        //    Amigos ad2 = new Amigos();
        //    ad2.IdUsuario = IdAmigo;
        //    ad2.IdAmigo = IdUsuario;
        //    await DeleteEntity(ad2);
        //    }
        //    catch
        //    {

        //    }
        //}

    }
}
