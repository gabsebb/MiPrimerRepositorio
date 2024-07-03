using AppLogin.Data;
using AppLogin.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppLogin.Models;
using Microsoft.EntityFrameworkCore;


namespace AppLogin.Controllers
{
    public class ClienteController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public ClienteController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public IActionResult List()
        {
            try
            {
                var clientes = _appDBContext.clientes.ToList();
                return View(clientes);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al cargar la lista de usuarios.");
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult crear()
        {
            return View("crear");
        }

        [HttpPost]
        public async Task<IActionResult> crear(Cliente modelo)
        {
            
            Cliente nuevoCliente = new Cliente()
            {
                nombre = modelo.nombre,
                apellido = modelo.apellido,
                cedula = modelo.cedula,
                direccion=modelo.direccion,
                telefono=modelo.telefono,   
                fechaModificacion = DateTime.Now,
                estado = true
            };

            await _appDBContext.clientes.AddAsync(nuevoCliente);
            await _appDBContext.SaveChangesAsync();

            if (nuevoCliente.id != 0)
            {
                return RedirectToAction("List", "Cliente");
            }

            ViewData["Mensaje"] = "Error al crear nuevo usuario";

            return View();
        }

        public IActionResult edit(int id)
        {
            var cliente = _appDBContext.clientes.Find(id);
            if (cliente == null)
            {
                ViewData["Mensaje"] = "No se pudo encontrar el usuario";
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> edit(int id, Cliente modelo)
        {
            try
            {
                if (id != modelo.id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    modelo.fechaModificacion = DateTime.Now;

                    _appDBContext.Entry(modelo).State = EntityState.Modified;
                    await _appDBContext.SaveChangesAsync();
                    return RedirectToAction("List", "Cliente", new { id = modelo.id });
                }
                return RedirectToAction("List", "Cliente", new { id = modelo.id });
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
            var model = _appDBContext.clientes.Find(id);
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
                var cliente = await _appDBContext.clientes.FindAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }
                _appDBContext.clientes.Remove(cliente);
                await _appDBContext.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al intentar eliminar el usuario.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
