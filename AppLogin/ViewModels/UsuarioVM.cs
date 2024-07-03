namespace AppLogin.ViewModels
{
    public class UsuarioVM
    {
        public int id { get; set; }
        public string clave { get; set; }
        public string usuario { get; set; }
        public string rol { get; set; }
        public string ConfirmarClave { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool estado { get; set; }

    }
}
