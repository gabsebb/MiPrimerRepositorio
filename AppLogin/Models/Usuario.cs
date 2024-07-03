namespace AppLogin.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public string clave { get; set; }
        public string usuario { get; set; }
        public string rol { get; set; }
        public DateTime fechaModificacion { get; set; }   
        public bool estado { get; set; } 

    }
}
