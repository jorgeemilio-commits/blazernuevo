using blazernuevo.Components.Data;
using System.Linq;

namespace blazernuevo.Components.Servicios
{
    public class ServicioControlador
    {
        public bool MostrarSoloJugados { get; set; } = false;

        public string FiltroNombre { get; set; } = string.Empty;

        private readonly ServicioJuegos _servicioJuegos;

        public ServicioControlador(ServicioJuegos servicioJuegos)
        {
            _servicioJuegos = servicioJuegos;
        }
        public async Task<List<Juego>> ObtenerJuegos()
        {
            return await _servicioJuegos.ObtenerJuegos(MostrarSoloJugados, FiltroNombre);
        }
        public async Task AgregarJuego(Juego juego)
        {
            juego.Identificador = await GenerarNuevoID();
            _servicioJuegos.AgregarJuego(juego);
        }

        private async Task<int> GenerarNuevoID()
        {
            var juegos = await _servicioJuegos.ObtenerJuegos();
            return juegos.Any() ? juegos.Max(t=> t.Identificador) + 1 : 1;
        }

        public async Task BorrarJuego(Juego juego)
        {
            await _servicioJuegos.BorrarJuego(juego);
        }

        public async Task ActualizarJuego(Juego juego)
        {
            await _servicioJuegos.ActualizarJuego(juego);
        }
    }
}
