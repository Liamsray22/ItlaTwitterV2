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

     

        //Login***********************************************

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

        //End Login**************************************************


        //Registro***********************************************
        #region registro
        public IActionResult Registro()
        {
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

        //End Registro**************************************************



        //Cerrar Sesion**********************************************
        #region CerrarSesion
        public IActionResult CerrarSesion()
        {
            _usuarioRepo.Cerrar();
            return RedirectToAction("Index");
        }
        #endregion

        //End Cerrar Sesion**********************************************


        //Activar Usuario**********************************************
        #region ActivarUsuario
        public async Task<IActionResult> ActivarUsuario(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");

            }
            await _usuarioRepo.ActivarUsuario(id.Value);
            return RedirectToAction("ConfirmacionS");
        }
        #endregion

        //End Activar Usuario**********************************************

        //Recuperar Clave**********************************************

        #region RecuperarPass

        public async Task<IActionResult> RecuperarPass(string User)
        {
            var rec = await _usuarioRepo.RecuperarPass(User);
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
        //End Recuperar Clave**********************************************

        public IActionResult Confirmacioncl()
        {
            return View();
        }

        public IActionResult Confirmacion()
        {
            return View();
        }
        public IActionResult ConfirmacionS()
        {
            return View();
        }

    }
}
