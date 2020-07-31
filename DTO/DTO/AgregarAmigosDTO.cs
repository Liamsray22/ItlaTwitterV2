using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.DTO
{
    public class AgregarAmigosDTO
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Clave { get; set; }
    }
}
