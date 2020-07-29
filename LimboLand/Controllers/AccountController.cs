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


        public IActionResult Index()
        {
            //try
            //{

            //    Usuarios userInfo = JsonConvert.DeserializeObject<Usuarios>(HttpContext.Session.GetString("SessionUser"));

            //    if (userInfo != null)
            //    {
            //        return RedirectToAction("Home", "Social", userInfo);
            //    }
            //}
            //catch
            //{
            //}
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var log = await _usuarioRepo.Login(loginViewModel);
                if (log) {
                    return RedirectToAction("Index","HomePage");
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o clave incorrectos");

                }

                //var User = _mapper.Map<Usuarios>(loginViewModel);
                //var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Usuario == User.Usuario &&
                //x.Clave == User.Clave);

                //if (user != null)
                //{
                //    if (user.Activo != 0)
                //    {
                //        HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(user));


                //        return RedirectToAction("Home", "Social", user);
                //    }
                //    else
                //    {
                //        Random r = new Random();
                //        int codigo = r.Next(1000, 9999);
                //        var mensaje = new Message(new string[] { user.Correo }, "Bienvenido a Limbo " + user.Nombre + " " + user.Apellido + "", "Confirme su cuenta mediante este codigo " + codigo);
                //        await _message.SendMailAsync(mensaje);
                //        HttpContext.Session.SetString("codigo", codigo.ToString());
                //        HttpContext.Session.SetString("id", user.IdUsuarios.ToString());



                //        return RedirectToAction("Confirmacion", "Home");
                //    }
                //}

            }

            return View(loginViewModel);
        }

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
                    return RedirectToAction("Index", "HomePage");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return View(registroViewModel);


        }
        #endregion

        //End Registro**************************************************


    }
}
