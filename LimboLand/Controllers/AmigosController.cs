using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LimboLand.Controllers
{
    public class AmigosController : Controller
    {
        public IActionResult ListaAmigos()
        {
            return View();
        }


        public IActionResult Buscar()
        {
            return View();
        }
    }
}