using System.ComponentModel.DataAnnotations;

namespace APIPruebaTecnica.Models
{
    public class Usuario
    {

        public Int64 Id { get; set; }
        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public int Cedula { get; set; }

        public string? Username { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

    }
}
