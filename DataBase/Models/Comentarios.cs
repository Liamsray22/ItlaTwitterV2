using System;
using System.Collections.Generic;

namespace DataBase.Models
{
    public partial class Comentarios
    {
        public Comentarios()
        {
            Comentarios2IdComentarioHijoNavigation = new HashSet<Comentarios2>();
            Comentarios2IdComentarioPadreNavigation = new HashSet<Comentarios2>();
        }

        public int IdComentario { get; set; }
        public int IdPublicacion { get; set; }
        public string Comentario { get; set; }
        public int? IdUsuario { get; set; }

        public ICollection<Comentarios2> Comentarios2IdComentarioHijoNavigation { get; set; }
        public ICollection<Comentarios2> Comentarios2IdComentarioPadreNavigation { get; set; }
    }
}
