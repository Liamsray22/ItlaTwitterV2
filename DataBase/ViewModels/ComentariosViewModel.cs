using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.ViewModels
{
    public class ComentariosViewModel
    {
        public int IdComentario { get; set; }
        public int IdPublicacion { get; set; }
        public string Comentario { get; set; }
        public int? IdUsuario { get; set; }
        public string Usuario { get; set; }
        public int Manda { get; set; }
        public IEnumerable<ComentariosViewModel> comentarios2 { get; set; }
        
    }
}
