using Microsoft.AspNetCore.Mvc;
using AppLogin.Data;
using AppLogin.Models;
using Microsoft.EntityFrameworkCore;
using AppLogin.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;



namespace AppLogin.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public AccesoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public IActionResult crearUsuarios()
        {
            return View("crearUsuarios");
        }

        [HttpPost]
        public async Task<IActionResult> crearUsuarios(UsuarioVM modelo)
        {
            if (modelo.clave != modelo.ConfirmarClave)
            {

                ViewData["Mensaje"] = "Las claves no son iguales";
                return View();
            }

            Usuario nuevoUsuario = new Usuario()
            {
                clave = modelo.clave,
                usuario = modelo.usuario,
                rol = modelo.rol,
                fechaModificacion = DateTime.Now,
                estado = true
            };

            await _appDBContext.usuario.AddAsync(nuevoUsuario);
            await _appDBContext.SaveChangesAsync();

            if (nuevoUsuario.id != 0)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["Mensaje"] = "Error al crear nuevo usuario";

            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM modelo)
        {
            Usuario? usuario_encontrado = await _appDBContext.usuario
                                            .Where(u =>
                                            u.usuario == modelo.usuario &&
                                            u.clave == modelo.clave
                                            ).FirstOrDefaultAsync();

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encuentra al usuario especificado";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,usuario_encontrado.usuario)

            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,

            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");




        }




    }

}

