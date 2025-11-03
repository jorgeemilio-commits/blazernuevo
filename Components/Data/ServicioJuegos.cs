using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks; 
using System.Linq;

namespace blazernuevo.Components.Data
{
    public class ServicioJuegos
    {
        private List<Juego> juegos = new List<Juego>();

        public async Task<List<Juego>> ObtenerJuegos(bool mostrarSoloJugados = false, string filtroNombre = null)
        {
            var listaJuegos = new List<Juego>();

            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();

            // --- Lógica de filtrado con WHERE dinámica ---
            var whereClauses = new List<string>();

            if (mostrarSoloJugados)
            {
                whereClauses.Add("jugado = 1");
            }
            if (!string.IsNullOrWhiteSpace(filtroNombre))
            {
                whereClauses.Add("nombre LIKE $filtroNombre");
                comando.Parameters.AddWithValue("$filtroNombre", $"%{filtroNombre}%");
            }
            string condicionWhere = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : "";

            // Construye el comando final
            comando.CommandText = $"SELECT identificador, nombre, jugado FROM juegos {condicionWhere};";

            using var lector = await comando.ExecuteReaderAsync();

            while (await lector.ReadAsync())
            {
                listaJuegos.Add(new Juego
                {
                    Identificador = lector.GetInt32(0),
                    Nombre = lector.GetString(1),
                    Jugado = lector.GetInt32(2) != 0
                });
            }
            return listaJuegos;
        }
        public async Task AgregarJuego(Juego juego)
        {
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();

            comando.CommandText = "insert into juegos(identificador,nombre,jugado) values($identificador,$nombre,$jugado);";
            comando.Parameters.AddWithValue("$identificador", juego.Identificador);
            comando.Parameters.AddWithValue("$nombre", juego.Nombre);
            comando.Parameters.AddWithValue("$jugado", juego.Jugado ? 1 : 0);

            comando.ExecuteNonQueryAsync();

            juegos.Add(juego);
        }

        public async Task ActualizarJuego(Juego juego)
        {
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();
            var comando = conexion.CreateCommand();

            comando.CommandText = "UPDATE juegos SET nombre = $nombre, jugado = $jugado WHERE identificador = $id;";

            comando.Parameters.AddWithValue("$id", juego.Identificador);
            comando.Parameters.AddWithValue("$nombre", juego.Nombre);
            comando.Parameters.AddWithValue("$jugado", juego.Jugado ? 1 : 0);

            await comando.ExecuteNonQueryAsync();
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
