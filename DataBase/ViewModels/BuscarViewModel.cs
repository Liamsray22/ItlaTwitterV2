using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.ViewModels
{
    public class BuscarViewModel
    {
        public int IdUsuario { get; set; }

        public int IdPublicacion { get; set; }

        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }


        public string Publicacion { get; set; }

        public IEnumerable<ComentariosViewModel> comm;

        public IEnumerable<BuscarViewModel> TraerUsuarios;


    }
}
