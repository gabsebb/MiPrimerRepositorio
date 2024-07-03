using AppLogin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppLogin.ViewModels
{
    public class ProductoVM
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public decimal iva { get; set; }
        public string codigo_barras { get; set; }
        public int IdTipoSeleccionado { get; set; }
        public List<SelectListItem> TiposProductos { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool estado { get; set; }

    }
}
