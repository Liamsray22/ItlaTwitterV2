using System;
using System.Collections.Generic;

namespace DataBase.Models
{
    public partial class Publicaciones
    {
        public int IdPublicacion { get; set; }
        public int? IdUsuario { get; set; }
        public string Publicacion { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdImagen { get; set; }
    }
}
