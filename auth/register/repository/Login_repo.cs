using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using back_app_par.auth.register.contracts;
using back_app_par.auth.register.dto;
using back_app_par.data;
using back_app_par.dto;
using back_app_par.models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace back_app_par.auth.register.repository
{
    public class Login_repo : ILogin
    {
        private readonly appContext _context;
        private readonly IConfiguration _config;

        public Login_repo(appContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<usuario> buscarUsuario(usuarioDto usuarioDto)
        {
            var usuario = new usuario();
            var cadenastring = _context.Database.GetDbConnection().ConnectionString;
            using var con = new NpgsqlConnection(cadenastring);
            con.OpenAsync();

            using var cmd = con.CreateCommand();
            cmd.CommandText = @"SELECT id,nombre,passwordHash,idRol,fechaCreacion,Activo FROM usuario WHERE nombre = @nombre";

            var p = cmd.CreateParameter();
            p.ParameterName = "@nombre";
            p.Value = usuarioDto.nombre;
            cmd.Parameters.Add(p);

            using var leer = await cmd.ExecuteReaderAsync();
            if(await leer.ReadAsync())
            {
                usuario.Id = leer.GetInt32(0);
                usuario.nombre = leer.GetString(1);
                usuario.passwordHash = leer.GetString(2);
                usuario.idRol = leer.GetInt32(3);
                usuario.fechaCreacion = leer.GetDateTime(4);
                usuario.Activo = leer.GetBoolean(5);

                return usuario;
            }
            return null;

        }
        public async Task<bool> VerificarContraseña(string password, string passwordhash)
        {
            bool valido = BCrypt.Net.BCrypt.Verify(password, passwordhash);
            if(valido == true)
            {
                return true;
            }
            return false;
        }
        public async Task<string> generarToken(usuario usuario)
        {
        var secretkey = _config["Jwt:SecretKey"];
        var key = System.Text.Encoding.UTF8.GetBytes(secretkey);
        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, usuario.nombre),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, usuario.idRol.ToString())
        };
        var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);  // Nombre correcto
        return tokenString;  // ← Devuelves el string
        }

    }

}
