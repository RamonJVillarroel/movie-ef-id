using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using movie_ef_mvc.Models;

namespace movie_ef_mvc.Controllers
{
    public class UsuarioController : Controller
    {
        // Inyección de dependencias para UserManager y SignInManager,
        // que son servicios proporcionados por ASP.NET Core Identity para gestionar usuarios y autenticación.
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        //private readonly ImagenStorage _imagenStorage;
       // private readonly IEmailService _emailService;
        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager /*ImagenStorage imagenStorage, IEmailService emailService*/)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            //_imagenStorage = imagenStorage;
           // _emailService = emailService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Clave, usuario.Recordarme, lockoutOnFailure: false);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Inicio de sesión inválido.");
                }
            }
            return View(usuario);
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF, ayudando a asegurar que el formulario se envíe desde la misma aplicación.
        public async Task<IActionResult> Registro(RegistroViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                // Lógica para registrar al usuario
                var nuevoUsuario = new Usuario
                {
                    UserName = usuario.Email,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    ImagenUrlPerfil = "/images/default-avatar.png"
                };
                // registrar al usuario utilizando UserManager
                var resultado = await _userManager.CreateAsync(nuevoUsuario, usuario.Clave);
                if (resultado.Succeeded)
                {
                    //si el resultado es correcto , se inicia sesión automáticamente al nuevo usuario utilizando SignInManager y se redirige a la página de inicio.
                    await _signInManager.SignInAsync(nuevoUsuario, isPersistent: false);
                    //await _emailService.SendAsync(nuevoUsuario.Email, "Bienvenido a Movie mvc", "<h1>Gracias por registrarte en Movie mvc!</h1><p>Esperamos que disfrutes de nuestra plataforma.</p>");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }


            }
            return View(usuario);
        }

    }
}
