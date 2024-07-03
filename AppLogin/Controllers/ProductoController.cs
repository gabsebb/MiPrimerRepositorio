using AppLogin.Data;
using AppLogin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppLogin.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppLogin.Controllers
{
    public class ProductoController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public ProductoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public IActionResult List()
        {
           
            var productosConTipos = _appDBContext.productos
                .Include(p => p.tipo_producto) 
                .ToList();

            return View(productosConTipos);
        }

        [HttpGet]
        public IActionResult crear()
        {
            var viewModel = new ProductoVM
            {
                TiposProductos = _appDBContext.tipo_productos
                .Select(tp => new SelectListItem
                {
                    Value = tp.id.ToString(),
                    Text = tp.nombre_tipo
                })
                .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> crear(ProductoVM modelo)
        {
            try
            {         
                    var producto = new Producto
                    {
                        nombre = modelo.Nombre,
                        iva = modelo.iva,
                        codigo_barras = modelo.codigo_barras,
                        id_tipo = modelo.IdTipoSeleccionado,
                        fechaModificacion = DateTime.Now,
                        estado = true
                    };

                    _appDBContext.productos.Add(producto);
                    await _appDBContext.SaveChangesAsync();

                    return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                ModelState.AddModelError("", "Error al intentar guardar el producto. Por favor, inténtelo de nuevo.");

                modelo.TiposProductos = _appDBContext.tipo_productos
                    .Select(tp => new SelectListItem
                    {
                        Value = tp.id.ToString(),
                        Text = tp.nombre_tipo
                    })
                    .ToList();

                return View(modelo);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var producto = _appDBContext.productos.Find(id);

                if (producto == null)
                {
                    return NotFound();
                }
                var viewModel = new ProductoVM
                {
                    id = producto.id,
                    Nombre = producto.nombre,
                    iva = producto.iva,
                    codigo_barras = producto.codigo_barras,
                    IdTipoSeleccionado = producto.id_tipo,
                    fechaModificacion = DateTime.Now,
                    estado = true,
                    TiposProductos = _appDBContext.tipo_productos
                        .Select(tp => new SelectListItem
                        {
                            Value = tp.id.ToString(),
                            Text = tp.nombre_tipo
                        })
                        .ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductoVM modelo)
        {
            try
            {
                var producto = await _appDBContext.productos.FindAsync(modelo.id);

                if (producto == null)
                {
                    return NotFound(); 
                }
                producto.nombre = modelo.Nombre;
                producto.iva = modelo.iva;
                producto.codigo_barras = modelo.codigo_barras;
                producto.id_tipo = modelo.IdTipoSeleccionado;
                producto.fechaModificacion = DateTime.Now;

                _appDBContext.productos.Update(producto);
                await _appDBContext.SaveChangesAsync();

                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                ModelState.AddModelError("", "Error al intentar actualizar el producto. Por favor, inténtelo de nuevo.");

                modelo.TiposProductos = _appDBContext.tipo_productos
                    .Select(tp => new SelectListItem
                    {
                        Value = tp.id.ToString(),
                        Text = tp.nombre_tipo
                    })
                    .ToList();

                return View(modelo);
            }
        }
        public ActionResult Delete(int id)
        {
            var model = _appDBContext.productos
                .Include(p => p.tipo_producto) 
                .Where(p => p.id == id)
                .Select(p => new ProductoVM
                {
                    id = p.id,
                    Nombre = p.nombre,
                    iva = p.iva,
                    codigo_barras = p.codigo_barras,
                    IdTipoSeleccionado = p.id_tipo,
                    TiposProductos = _appDBContext.tipo_productos
                        .Select(tp => new SelectListItem
                        {
                            Value = tp.id.ToString(),
                            Text = tp.nombre_tipo
                        })
                        .ToList(),
                    fechaModificacion = p.fechaModificacion,
                    estado = p.estado
                })
                .FirstOrDefault();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteS(int id)
        {
            try
            {
                var producto = await _appDBContext.productos.FindAsync(id);

                if (producto == null)
                {
                    return NotFound();
                }

                _appDBContext.productos.Remove(producto);
                await _appDBContext.SaveChangesAsync();

                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction(nameof(Error));
            }
        }


    }
}
