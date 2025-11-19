using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_app_par.auth.register.dto;
using back_app_par.dto;
using back_app_par.models;

namespace back_app_par.auth.register.contracts
{
    public interface ILogin
    {
        Task<usuario> buscarUsuario(usuarioDto loginDto);
        Task<bool> VerificarContraseña(string password, string passwordhash);
        Task<string> generarToken(usuario usuario);
    }
}