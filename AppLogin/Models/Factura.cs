namespace AppLogin.Models
{
    public class Factura
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public int idCliente { get; set; }
        public Cliente cliente { get; set; }
        public decimal total { get; set; }
        public decimal iva { get; set; }
        public decimal subtotal { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool estado { get; set; }

        // Relación uno a muchos con Detalle_factura
        public List<Detalle_factura> Detalle_factura { get; set; }
    }

}
