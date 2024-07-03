namespace AppLogin.Models
{
    public class Detalle_factura
    {
        public int id { get; set; }
        public int id_factura { get; set; }
        public Factura factura { get; set; }
        public int id_producto { get; set; }
        public Producto producto { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal descuento { get; set; }
        public decimal total { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool estado { get; set; }
    }

}
