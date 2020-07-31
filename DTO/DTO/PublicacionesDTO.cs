using DataBase.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class PublicacionesDTO
    {
        public int? IdUsuario { get; set; }
        public int IdPublicacion { get; set; }
        //[Required]
        public string Publicacion { get; set; }
        public DateTime? Fecha { get; set; }
        public string Imagen { get; set; }
        public IFormFile FotoPub { get; set; }

        public string Usuario { get; set; }
        //public int CantidadAmigos { get; set; }


        public IEnumerable<PublicacionesDTO> publicaciones;

        public IEnumerable<ComentariosViewModel> comentarios;
    }
}
