using System.Threading.Tasks;
using DataBase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository
{
    public class UsuarioAPIRepo : RepositoryBase<Usuarios, LIMBODBContext>
    {
        private readonly LIMBODBContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        
        public UsuarioAPIRepo(LIMBODBContext context, SignInManager<IdentityUser> signInManager) : base(context)
        {
            _context = context;
            _signInManager = signInManager;
            
        }

        //Validar el nombre de usuario y la clave
        public async Task<bool> Login(string Usuario, string Clave)
        {

            var result = await _signInManager.PasswordSignInAsync(Usuario, Clave, false, true);

            if (result.Succeeded)
            {
                return true;

            }
            return false;
        }

              
        //Obtener Usuario por nombre de Usuario
        public async Task<Usuarios> GetUsuarioByName(string user)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x=>x.Usuario == user);
        }


        //Obtener Nombre de Usuario por el id
        public async Task<string> GetNombreUsuarioById(int? id)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuarios == id);
            return user.Usuario;
        }

    }
}
