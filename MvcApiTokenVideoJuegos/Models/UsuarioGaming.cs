using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApiTokenVideoJuegos.Models
{
    public class UsuarioGaming
    {
        public int IdUsuario { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Imagen { get; set; }
    }
}
