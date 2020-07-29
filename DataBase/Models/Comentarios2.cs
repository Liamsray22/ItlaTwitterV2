using System;
using System.Collections.Generic;

namespace DataBase.Models
{
    public partial class Comentarios2
    {
        public int IdComentarioPadre { get; set; }
        public int IdComentarioHijo { get; set; }

        public Comentarios IdComentarioHijoNavigation { get; set; }
        public Comentarios IdComentarioPadreNavigation { get; set; }
    }
}
