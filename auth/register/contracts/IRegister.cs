using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_app_par.auth.register.contracts
{
    public interface IRegister
    {
        Task<bool> VerificarUsarioExiste(string usuario);
        Task<string> HashearContraseña(string contraseña);
        Task<bool> CrearUsuario(string usuario, string password, int rol);
    }
}