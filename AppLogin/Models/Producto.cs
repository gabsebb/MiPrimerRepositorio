namespace AppLogin.Models
{
    public class Producto
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int id_tipo { get; set; }
        public Tipo_producto tipo_producto { get; set; }
        public decimal iva { get; set; }    
        public string codigo_barras { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool estado { get; set; }
    }
}
