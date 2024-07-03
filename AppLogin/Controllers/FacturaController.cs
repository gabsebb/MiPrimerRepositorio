using AppLogin.Data;
using AppLogin.Models;
using AppLogin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppLogin.Controllers
{
    public class FacturaController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public FacturaController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public IActionResult Nueva()
        {
            var viewModel = new nuevaFacturaVM
            {
                Fecha = DateTime.Now,
                Productos = _appDBContext.productos
                                .Where(p => p.estado)
                                .Select(p => new SelectListItem
                                {
                                    Value = p.id.ToString(),
                                    Text = p.nombre
                                })
                                .ToList(),
                Clientes = _appDBContext.clientes
                            .Select(c => new SelectListItem
                            {
                                Value = c.id.ToString(),
                                Text = c.nombre
                            })
                            .ToList(),
                DetalleFactura = new List<DetalleFacturaVM>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Nueva(nuevaFacturaVM model)
        {
            if (ModelState.IsValid)
            {
                var factura = new Factura
                {
                    fecha = model.Fecha,
                    idCliente = model.IdCliente,
                    subtotal = model.Subtotal,
                    iva = model.Iva,
                    total = model.Total,
                    estado = true, // Por ejemplo, marca la factura como activa
                    fechaModificacion = DateTime.Now
       
                };

                foreach (var detalle in model.DetalleFactura)
                {
                    var detalleFactura = new Detalle_factura
                    {
                        id_producto = detalle.IdProducto,
                        cantidad = detalle.Cantidad,
                        precio = detalle.Precio,
                        descuento = detalle.Descuento,
                        total = detalle.Cantidad * detalle.Precio - detalle.Descuento,
                        estado = true, // Por ejemplo, marca el detalle como activo
                        fechaModificacion = DateTime.Now
                    };

                    factura.Detalle_factura.Add(detalleFactura);
                }

                _appDBContext.Add(factura);
                _appDBContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            model.Productos = _appDBContext.productos
                                .Where(p => p.estado)
                                .Select(p => new SelectListItem
                                {
                                    Value = p.id.ToString(),
                                    Text = p.nombre
                                })
                                .ToList();

            model.Clientes = _appDBContext.clientes
                                .Select(c => new SelectListItem
                                {
                                    Value = c.id.ToString(),
                                    Text = c.nombre
                                })
                                .ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult ObtenerDatosCliente(int clienteId)
        {
            var cliente = _appDBContext.clientes.FirstOrDefault(c => c.id == clienteId);

            if (cliente == null)
            {
                return NotFound(); 
            }
            var datosCliente = new
            {
                Nombre = cliente.nombre,
                Apellido= cliente.apellido,
                Cedula= cliente.cedula,
                Direccion = cliente.direccion,
                Telefono = cliente.telefono,

            };
            return Json(datosCliente);
        }

    }
}
