using DataBase.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Repository.Repository;
using System.Threading.Tasks;

namespace LimboLand.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuarioRepo _usuarioRepo;
        public AccountController(UsuarioRepo usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;

        }

     

        //Login

        #region Login
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "HomePage");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var log = await _usuarioRepo.Login(loginViewModel);
                if (log ==2) {
                    return RedirectToAction("Index","HomePage");
                }
                else if(log == 3)
                {
                    ModelState.AddModelError("", "Usuario o clave incorrectos");
                }else if (log == 4)
                {
                    return RedirectToAction("Confirmacion");
                }
            }
            return View(loginViewModel);
        }

        public IActionResult Login()
        {
            return RedirectToAction("Index");
        }
        #endregion



        //Registro
        #region registro
        public IActionResult Registro()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "HomePage");
            }
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Registro(RegistroViewModel registroViewModel)
        {
            if (ModelState.IsValid)
            {
                var register = await _usuarioRepo.CreateUserAsync(registroViewModel);
                if (register) {
                    return RedirectToAction("Confirmacion");
                }
                else
                {
                    return RedirectToAction("Registro");
                }
            }
            return View(registroViewModel);
        }
        #endregion




        //Cerrar Sesion
        #region CerrarSesion
        public IActionResult CerrarSesion()
        {
            _usuarioRepo.Cerrar();
            return RedirectToAction("Index");
        }
        #endregion



        //Activar Usuario
        #region ActivarUsuario
        public async Task<IActionResult> ActivarUsuario(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "HomePage");
            }
            if (id == null)
            {
                return RedirectToAction("Index");

            }
            await _usuarioRepo.ActivarUsuario(id.Value);
            return RedirectToAction("ConfirmacionS");
        }
        #endregion


        //Recuperar Clave

        #region RecuperarPass

        public async Task<IActionResult> RecuperarPass(string user)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "HomePage");
            }
            var rec = await _usuarioRepo.RecuperarPass(user);
            if (rec)
            {
                return RedirectToAction("Confirmacioncl");

            }
            else
            {
                return RedirectToAction("Index");

            }
        }
        #endregion


        //Confirmaciones
        #region Confirmaciones
        public IActionResult Confirmacioncl()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "HomePage");
            }
            return View();
        }

        public IActionResult Confirmacion()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "HomePage");
            }
            return View();
        }
        public IActionResult ConfirmacionS()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "HomePage");
            }
            return View();
        }
#endregion

    }
}
