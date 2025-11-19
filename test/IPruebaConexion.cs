using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_app_par.test
{
    public interface IPruebaConexion
    {
        Task<bool> ProbarConexion();
    }
}