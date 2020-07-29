using System;
using System.Collections.Generic;

namespace DataBase.Models
{
    public partial class Amigos
    {
        public int IdUsuario { get; set; }
        public int IdAmigo { get; set; }

        public Usuarios IdAmigoNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
    }
}
