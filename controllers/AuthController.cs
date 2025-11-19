using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_app_par.auth.register.contracts;
using back_app_par.auth.register.dto;
using Microsoft.AspNetCore.Mvc;

namespace back_app_par.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRegister _repo;
        private readonly ILogin _rep;

        public AuthController(IRegister repo,ILogin rep)
        {
            _repo = repo;
            _rep = rep;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> CrearUsuario([FromBody] usuarioDto usuarioDto)
        {
            // VERIFICAR SI EXISTE USUARIO
            var response = await _repo.VerificarUsarioExiste(usuarioDto.nombre);
            if (response == false)
            {
                return BadRequest(new { mensaje = "Usario ya Existe" });
            }

            // HASHEAR CONTRASEÑA
            var hashPassword = await _repo.HashearContraseña(usuarioDto.password);
            var nuevoUsuario = await _repo.CrearUsuario(usuarioDto.nombre, hashPassword, usuarioDto.idRol);
            return Ok(new { mensaje = "Usuario Creado" });

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] usuarioDto usuario)
        {
            var findUsuario = await _rep.buscarUsuario(usuario);
            if(findUsuario == null)
            {
               return BadRequest(new{mensaje= "USUARIO NO EXISTE"});
            }
            var valido = await _rep.VerificarContraseña(usuario.password,findUsuario.passwordHash);
            if(valido == false)
            {
                return BadRequest(new{mensaje= "CONTRASEÑA INCORRECTA"});
            }
            var token = await _rep.generarToken(findUsuario);

            return Ok(new{success=true,token=token,usuario = new{id = findUsuario.Id, usuario=findUsuario.nombre, idRol=findUsuario.idRol}});
        }
    }
}