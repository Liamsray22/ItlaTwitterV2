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
    public class ImagenesAPIRepo : RepositoryBase<Imagenes, LIMBODBContext>
    {
        private readonly LIMBODBContext _context;
        


        public ImagenesAPIRepo(LIMBODBContext context) : base(context)
        {
            _context = context;
            
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
