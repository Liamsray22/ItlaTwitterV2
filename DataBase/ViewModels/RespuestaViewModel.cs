using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.ViewModels
{
    public class RespuestaViewModel
    {
        public int IdComentario { get; set; }
        public int IdComentarioPadre { get; set; }
        public string Comentario { get; set; }
        public int? IdUsuario { get; set; }
    }
}
