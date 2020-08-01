using DataBase.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.ViewModels
{
    public class RegistroViewModel
    {
        public int Id_Usuarios { get; set; }

        [Required(ErrorMessage ="El Campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El Campo no debe superar los 50 caracteres")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = "El Campo Apellido es requerido")]
        [StringLength(100, ErrorMessage = "El Campo no debe superar los 100 caracteres")]

        public String Apellido { get; set; }

        [Required(ErrorMessage = "El Campo Clave es requerido")]
        [StringLength(20, ErrorMessage = "El Campo no debe superar los 20 caracteres")]

        public String Clave { get; set; }

        [Required(ErrorMessage = "El Campo Confirmar Clave es requerido")]
        [Compare(nameof(Clave),ErrorMessage = "Las contraseñas no son identicas")]
        public String Clave2 { get; set; }

        [Required(ErrorMessage = "El Campo Correo es requerido")]
        [StringLength(50, ErrorMessage = "El Campo no debe superar los 50 caracteres")]
        [RegularExpression("^[\\w.+\\-]+@gmail\\.com$", ErrorMessage ="Correo Invalido")]
        public String Correo { get; set; }

        [Required(ErrorMessage = "El Campo Usuario es requerido")]
        [StringLength(30, ErrorMessage = "El Campo no debe superar los 30 caracteres")]
        [Usuario]

        public String Usuario { get; set; }

        [Required(ErrorMessage = "El campo Telefono es requerido")]
        [RegularExpression("[0-9]{3}-[0-9]{3}-[0-9]{4}", ErrorMessage = "Numero de Telefono invalido, recuerde usar guiones")]
        [Telefono(ErrorMessage = "Numero de Telefono invalido,  recuerde usar guiones")]
        public string Telefono { get; set; }

        public int? Activo { get; set; }
        [Required]
        public IFormFile Foto { get; set; }


    }

    public class TelefonoAttribute : ValidationAttribute
    {


        public override bool IsValid(object value)
        {
            try
            {
                string tel = value.ToString();

                if (tel[0] != '8' || tel[1] != '0' || tel[2] != '9')
                {
                    if (tel[0] != '8' || tel[1] != '2' || tel[2] != '9')
                    {
                        if (tel[0] != '8' || tel[1] != '4' || tel[2] != '9')
                        {
                            return false;

                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;


            }
        }
    }

    public class UsuarioAttribute : ValidationAttribute
    {


        public override bool IsValid(object value)
        {
         LIMBODBContext _context = new LIMBODBContext();

            var listaUsuarios = _context.Usuarios.Select(x => x.Usuario).ToList();

            if (listaUsuarios.Contains(value))
            {
                return false;
            }

            return true;

        }
    }
}