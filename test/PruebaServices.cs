using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_app_par.data;
using Microsoft.EntityFrameworkCore;

namespace back_app_par.test
{
    public class PruebaServices : IPruebaConexion
    {
        private readonly appContext _context;

        public PruebaServices(appContext context)
        {
            _context = context;
        }

        public async Task<bool> ProbarConexion()
        {
            try
            {
                await _context.Database.OpenConnectionAsync();
            await _context.Database.CloseConnectionAsync();
            return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERROR :: {ex}");
                return false;
            }
            
        }
    }
}