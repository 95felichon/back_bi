using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_app_par.models
{
    public class usuario
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public string passwordHash { get; set; }
        public int idRol { get; set; }
        public DateTime fechaCreacion { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;

    }
}