using AppLogin.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AppLogin.Models;
using AppLogin.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace AppLogin.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public UsuarioController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }


        // GET: UsuarioController
        public IActionResult List()
        {
            try
            {
                var usuarios = _appDBContext.usuario
            .Select(u => new UsuarioVM
            {
                id = u.id,
                usuario = u.usuario,
                clave = u.clave,  // No se debe mostrar directamente por razones de seguridad
                rol = u.rol,
                fechaModificacion = u.fechaModificacion,
                estado = u.estado
            })
            .ToList();
                return View(usuarios); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al cargar la lista de usuarios.");
                return RedirectToAction("Index"); // Por ejemplo, redirigir a la página principal
            }
        }

        public IActionResult Edit(int id)
        {
            var usuario = _appDBContext.usuario.Find(id);
            if (usuario == null)
            {
                ViewData["Mensaje"] = "No se pudo encontrar el usuario";
                return NotFound();
            }
            var modelo = new UsuarioVM
            {
                id = usuario.id,
                usuario = usuario.usuario,
                clave = usuario.clave,  
                rol = usuario.rol,
                estado = usuario.estado,
                fechaModificacion = usuario.fechaModificacion
            };
            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioVM modelo)
        {
            try
            {
                if (id != modelo.id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)  
                {
                    if (modelo.clave != modelo.ConfirmarClave)
                    {
                        ModelState.AddModelError("confirmarClave", "Las contraseñas no coinciden.");
                        ViewData["Mensaje"] = "Las claves no son iguales";
                        return View(modelo);
                    }

                    // Mapea el modelo de vista al modelo de dominio
                    var usuario = await _appDBContext.usuario.FindAsync(id);
                    if (usuario == null)
                    {
                        return NotFound();
                    }
                    usuario.usuario = modelo.usuario;
                    usuario.clave = modelo.clave;  // Actualiza la clave solo si es necesario
                    usuario.rol = modelo.rol;
                    usuario.estado = modelo.estado;
                    usuario.fechaModificacion = DateTime.Now;

                    _appDBContext.Entry(usuario).State = EntityState.Modified;
                    await _appDBContext.SaveChangesAsync();
                    return RedirectToAction("List", new { id = modelo.id });
                }
                return RedirectToAction(nameof(List), new { id = modelo.id});
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError("", "Error de concurrencia al intentar guardar los cambios.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al intentar guardar los cambios.");
            }
                return View();
        }

        public ActionResult Delete(int id)
        {
            var model = _appDBContext.usuario.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteS(int id)
        {
            try
            {
                var usuario = await _appDBContext.usuario.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }
                _appDBContext.usuario.Remove(usuario);
                await _appDBContext.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al intentar eliminar el usuario.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
