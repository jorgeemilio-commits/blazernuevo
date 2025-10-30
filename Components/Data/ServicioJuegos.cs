using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace blazernuevo.Components.Data
{
    public class ServicioJuegos
    {
        private List<Juego> juegos = new List<Juego>
        {
        new Juego{Identificador=1,Nombre="Ravel",Jugado=false },
        new Juego{Identificador=2,Nombre="Geometry Dash",Jugado=true}
        };

        public async Task<List<Juego>> ObtenerJuegos()
        {
            juegos.Clear();
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();

            comando.CommandText = "SELECT identificador, nombre, jugado FROM juegos;";
            using var lector = await comando.ExecuteReaderAsync();

            while (await lector.ReadAsync())
            {
                juegos.Add(new Juego
                {
                    Identificador = lector.GetInt32(0),
                    Nombre = lector.GetString(1),
                    Jugado = lector.GetInt32(2) == 0 ? false : true
                });
            }
            return juegos;
        }

        public async Task AgregarJuego(Juego juego)
        {
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();

            comando.CommandText = "insert into juegos(identificador,nombre,jugado) values(3,'Mario Bros',1);";
            comando.Parameters.AddWithValue("$identificador", juego.Identificador);
            comando.Parameters.AddWithValue("$nombre", juego.Nombre);
            comando.Parameters.AddWithValue("$jugado", juego.Jugado ? 1 : 0);

            comando.ExecuteNonQueryAsync();

            juegos.Add(juego);
        }


        public async Task BorrarJuego(Juego juego) 
        {
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();


            comando.CommandText = "DELETE FROM juegos WHERE identificador = $id;";
            comando.Parameters.AddWithValue("$id", juego.Identificador);
            await comando.ExecuteNonQueryAsync();

            juegos.RemoveAll(j => j.Identificador == juego.Identificador);
        }
    }
}
