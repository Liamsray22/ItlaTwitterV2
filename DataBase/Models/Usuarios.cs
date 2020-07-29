using System;
using System.Collections.Generic;

namespace DataBase.Models
{
    public partial class Usuarios
    {
        public Usuarios()
        {
            AmigosIdAmigoNavigation = new HashSet<Amigos>();
            AmigosIdUsuarioNavigation = new HashSet<Amigos>();
        }

        public int IdUsuarios { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string Telefono { get; set; }
        public int? Activo { get; set; }
        public int? IdImagen { get; set; }

        public ICollection<Amigos> AmigosIdAmigoNavigation { get; set; }
        public ICollection<Amigos> AmigosIdUsuarioNavigation { get; set; }
    }
}
