using blazernuevo.Components.Data;

namespace blazernuevo.Components.Servicios
{
    public class ServicioControlador
    {
        public bool MostrarSoloJugados { get; set; } = false;

        private readonly ServicioJuegos _servicioJuegos;

        public ServicioControlador(ServicioJuegos servicioJuegos)
        {
            _servicioJuegos = servicioJuegos;
        }

        public async Task<List<Juego>> ObtenerJuegos()
        {
            return await _servicioJuegos.ObtenerJuegos();
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
    }
}
