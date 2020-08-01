using DataBase.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.ViewModels
{
    public class PublicacionesViewModel
    {
        public int? IdUsuario { get; set; }
        public int IdPublicacion { get; set; }
        [Required]
        public string Publicacion { get; set; }
        public DateTime? Fecha { get; set; }
        public string Imagen { get; set; }
        public string ImagenUser { get; set; }
        public IFormFile FotoPub { get; set; }

        public string Usuario { get; set; }

        public IEnumerable<PublicacionesViewModel> publicaciones;

        public IEnumerable<ComentariosViewModel> comentarios;

    }
}
