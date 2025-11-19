using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_app_par.test;
using Microsoft.AspNetCore.Mvc;

namespace back_app_par.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IPruebaConexion _repo;

        public LoginController(IPruebaConexion repo)
        {
            _repo = repo;
        }

        [HttpPost("CrearUsuario")]
        public async Task<IActionResult> ProbarConexion()
        {
            var reply = await _repo.ProbarConexion();
            if (reply == true)
            {
                return Ok(new { mensaje = "CONEXION HECHITA" });
            }
            return BadRequest(new { mensaje = "ERROR" });
        }
    }
}