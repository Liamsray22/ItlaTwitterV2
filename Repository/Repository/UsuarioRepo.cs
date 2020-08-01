using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataBase;
using DataBase.Models;
using DataBase.ViewModels;
using EmailConfig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository
{
    public class UsuarioRepo : RepositoryBase<Usuarios, LIMBODBContext>
    {
        private readonly LIMBODBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment Hotin;
        private readonly IMessage _message;



        public UsuarioRepo(LIMBODBContext context, UserManager<IdentityUser> userManager,
                            SignInManager<IdentityUser> signInManager, IMapper mapper,
                            IHostingEnvironment Hotin, IMessage message) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            this.Hotin = Hotin;
            _message = message;
        }
       

        public async Task<bool> CreateUserAsync(RegistroViewModel rvm)
        {

            var user = new IdentityUser { UserName = rvm.Usuario, Email = rvm.Correo };
            var result =  await _userManager.CreateAsync(user, rvm.Clave);

            if (result.Succeeded) {
                string FileName = null;
                if (rvm.Foto != null)
                {
                    try
                    {
                        string Subir = Path.Combine(Hotin.WebRootPath, "images\\fotoPerfil");
                        FileName = rvm.Usuario + ".png";
                        string FilePath = Path.Combine(Subir, FileName);

                        rvm.Foto.CopyTo(new FileStream(FilePath, FileMode.Create));
                        Imagenes img = new Imagenes();
                        img.Nombre = FileName;
                        img.Ruta = "images\\fotoPerfil\\" + FileName + "";
                        await _context.Imagenes.AddAsync(img);
                        await _context.SaveChangesAsync();

                        var image = await _context.Imagenes.FirstOrDefaultAsync(d => d.Nombre == FileName);
                        rvm.Activo = 0;
                        var newUsuario = _mapper.Map<Usuarios>(rvm);
                        newUsuario.IdImagen = image.IdImagen;
                        await AddAsync(newUsuario);
                        string opcion1 = "https://localhost:5001/Account/ActivarUsuario/"+ newUsuario.IdUsuarios+ "";
                        string opcion2 = "https://localhost:5000/Account/ActivarUsuario/" + newUsuario.IdUsuarios + "";
                        string opcion3 = "http://localhost:49371/Account/ActivarUsuario/" + newUsuario.IdUsuarios + "";
                        var mensaje = new Message(new string[] { rvm.Correo }, "Bienvenido a Limbo " 
                            + rvm.Nombre + " " + rvm.Apellido + "",
                            "Confirme su cuenta mediante este hipervinculo " + opcion1 +
                            " En caso de error intente con el siguiente " +opcion2+
                            " Como ultimo recurso puede intentar con este "+ opcion3);
                               await _message.SendMailAsync(mensaje);
                        return true;
                }
                    catch
                {
                    return false;
                }
            }
                return false;
                
            }
            return false;
        }

        public async Task<int> Login(LoginViewModel lvm)
        {
            var usu = await GetUsuarioByName(lvm.Usuario);
            if (usu != null)
            {
                if (usu.Activo != 0)
                {
                    var result = await _signInManager.PasswordSignInAsync(lvm.Usuario, lvm.Clave, false, true);

                    if (result.Succeeded)
                    {
                        return 2;

                    }
                }
                else
                {
                    string opcion1 = "https://localhost:5001/Account/ActivarUsuario/" + usu.IdUsuarios + "";
                    string opcion2 = "https://localhost:5000/Account/ActivarUsuario/" + usu.IdUsuarios + "";
                    string opcion3 = "http://localhost:49371/Account/ActivarUsuario/" + usu.IdUsuarios + "";
                    var mensaje = new Message(new string[] { usu.Correo }, "Bienvenido a Limbo "
                        + usu.Nombre + " " + usu.Apellido + "",
                        "Confirme su cuenta mediante este hipervinculo " + opcion1 +
                        " En caso de error intente con el siguiente " + opcion2 +
                        " Como ultimo recurso puede intentar con este " + opcion3);
                    await _message.SendMailAsync(mensaje);
                    return 4;
                }
            }
            
            return 3;
        }


        public async Task<Usuarios> GetUsuarioByName(string user)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x=>x.Usuario == user);
        }
        public async Task<string> GetNombreUsuarioById(int? id)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuarios == id);
            return user.Usuario;
        }

        public void Cerrar()
        {
            _signInManager.SignOutAsync();
        }

        public async Task ActivarUsuario(int id)
        {
            var user = await GetByIdAsync(id);
            if (user == null)
            {

            }
            else
            {
                user.Activo = 1;
                await Update(user);
            }
        }

        public async Task<bool> RecuperarPass(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var usu = await GetUsuarioByName(username);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                Random r = new Random();
                int codigo = r.Next(10000000, 99999999);
                usu.Clave = ""+codigo;
                await Update(usu);
                var result = await _userManager.ResetPasswordAsync(user, token, ""+codigo);
                if (result.Succeeded)
                {
                    var mensaje = new Message(new string[] { user.Email }, "Saludos "
                            + user.UserName +" ",
                            "Su nueva clave es: " + codigo +
                            " " );
                    await _message.SendMailAsync(mensaje);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
