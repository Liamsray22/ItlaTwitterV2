using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataBase.Models;
using DTO.DTO;

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

        //Traer Lista de amigos de un nombre de usuario
        public async Task<List<ListaAmigosDTO>> TraerListaAmigos(string name)
        {
            var user = await _usuarioAPIRepo.GetUsuarioByName(name);
            if (user != null)
            {
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
            return null;

        }


        //Agregar Amigos
        public async Task<bool> AgregarAmigos(AgregarAmigosDTO agg, string amigo)
        {
            var log =await _usuarioAPIRepo.Login(agg.Usuario, agg.Clave);
            if (log) {
                try
                {
                    var user = await _usuarioAPIRepo.GetUsuarioByName(agg.Usuario);
                    var pana = await _usuarioAPIRepo.GetUsuarioByName(amigo);
                    if (pana == null) {
                        return false;
                    }
                    Amigos ad = new Amigos();
                    ad.IdUsuario = user.IdUsuarios;
                    ad.IdAmigo = pana.IdUsuarios;
                    await AddAsync(ad);
                    ad.IdUsuario = pana.IdUsuarios;
                    ad.IdAmigo = user.IdUsuarios;
                    await AddAsync(ad);
                    return true;
                }
                catch
                {
                    return false;

                }
            }
            return false;


        }        
    }
}
