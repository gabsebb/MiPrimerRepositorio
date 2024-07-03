using AppLogin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppLogin.ViewModels
{
    public class nuevaFacturaVM
    {
        public int Id { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "IVA")]
        public decimal Iva { get; set; }

        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        public List<DetalleFacturaVM> DetalleFactura { get; set; }

        public List<SelectListItem> Productos { get; set; }
        public List<SelectListItem> Clientes { get; set; }
    }
    
    public class DetalleFacturaVM
    {
        public int Id { get; set; }

        [Display(Name = "Producto")]
        public int IdProducto { get; set; }

        public Producto Producto { get; set; }

        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Display(Name = "Descuento")]
        public decimal Descuento { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }
    }
}
