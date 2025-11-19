using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_app_par.auth.register.contracts;
using back_app_par.data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace back_app_par.auth.register.repository
{
    public class Register_repo : IRegister
    {
          private readonly appContext _context;

        public Register_repo(appContext context)
        {
            _context = context;
        }



        public async Task<bool> VerificarUsarioExiste(string usuario)
        {

            var cadena = _context.Database.GetDbConnection().ConnectionString;
            using var con = new NpgsqlConnection(cadena);
            await con.OpenAsync();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"SELECT COUNT(1) FROM usuario WHERE nombre = @nombre;";
                var p = cmd.CreateParameter();
                p.ParameterName = "@nombre";
                p.Value = usuario;
                cmd.Parameters.Add(p);
                var reply = await cmd.ExecuteScalarAsync();

                if (reply != null)
                {
                    return true;
                }
                return false;

            }
        }
        public async Task<string> HashearContraseña(string contraseña)
        {
            var hasPassword = BCrypt.Net.BCrypt.HashPassword(contraseña);
            return hasPassword;
        }

        public async Task<bool> CrearUsuario(string usuario, string password, int rol)
        {
            var cadena = _context.Database.GetDbConnection().ConnectionString;
            using var con = new NpgsqlConnection(cadena);
            await con.OpenAsync();
            using(var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO usuario (nombre, passwordHash, idRol, fechaCreacion, Activo) 
                                    VALUES (@nombre, @passwordHash, @idRol, @fechaCreacion, @activo)  
                                  ";
                var p = cmd.CreateParameter();
                p.ParameterName = "@nombre";
                p.Value = usuario;
                cmd.Parameters.Add(p);
                var p2 = cmd.CreateParameter();
                p2.ParameterName = "@passwordHash";
                p2.Value = password;
                cmd.Parameters.Add(p2);
                var p3 = cmd.CreateParameter();
                p3.ParameterName = "@idRol";
                p3.Value = rol;
                cmd.Parameters.Add(p3);
                var p4 = cmd.CreateParameter();
                p4.ParameterName = "@fechaCreacion";
                p4.Value = DateTime.UtcNow;
                cmd.Parameters.Add(p4);
                var p5 = cmd.CreateParameter();
                p5.ParameterName = "@activo";
                p5.Value = true;
                cmd.Parameters.Add(p5);

                var filasAfectadas = await cmd.ExecuteNonQueryAsync();
                if (filasAfectadas <= 0)
                {
                    return false;
                }
                return true;
            }
        }
    }
}