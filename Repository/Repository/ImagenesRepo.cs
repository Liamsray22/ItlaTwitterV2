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
    public class ImagenesRepo : RepositoryBase<Imagenes, LIMBODBContext>
    {
        private readonly LIMBODBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UsuarioRepo _usuarioRepo;
        private readonly IMapper _mapper;


        public ImagenesRepo(LIMBODBContext context, UserManager<IdentityUser> userManager,
                            SignInManager<IdentityUser> signInManager, IMapper mapper,
                            UsuarioRepo usuarioRepo) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _usuarioRepo = usuarioRepo;
        }

        public void Borrar(string path)
        {
            File.SetAttributes(path, FileAttributes.Normal);
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            File.Delete(path);
        }


    }
}
