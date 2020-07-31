using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class ComentariosDTO
    {
        public int IdComentario { get; set; }
        public int IdPublicacion { get; set; }
        public string Comentario { get; set; }
        public int? IdUsuario { get; set; }
        public string Usuario { get; set; }
        //public int Manda { get; set; }
        public IEnumerable<ComentariosDTO> respuestas { get; set; }
        
    }
}
