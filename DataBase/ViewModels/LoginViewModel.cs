using DataBase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="El campo Usuario es requerido")]
        [StringLength(30, ErrorMessage = "No debe superar los 30 caracteres")]
        [UsuarioBien(ErrorMessage = "Usuario o clave incorrectas")]
        public String Usuario { get; set; }

        [Required(ErrorMessage = "El campo Clave es requerido")]
        [StringLength(20, ErrorMessage ="No debe superar los 20 caracteres")]
        public String Clave { get; set; }
    }


    public class UsuarioBienAttribute : ValidationAttribute
    {


        public override bool IsValid(object value)
        {
            LIMBODBContext _context = new LIMBODBContext();

            var listaUsuarios = _context.Usuarios.Select(x => x.Usuario).ToList();

            if (listaUsuarios.Contains(value))
            {
                return true;
            }

            return false;

        }
    }
}
