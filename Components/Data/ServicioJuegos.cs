namespace blazernuevo.Components.Data
{
    public class ServicioJuegos
    {
        private List<Juego> juegos = new List<Juego>
        {
        new Juego{Identificador=1,Nombre="Ravel",Jugado=false },
        new Juego{Identificador=2,Nombre="Geometry Dash",Jugado=true}
        };

        public Task<List<Juego>> ObtenerJuegos()=>Task.FromResult(juegos);

        public Task AgregarJuego(Juego juego)
        {
            juegos.Add(juego);
            return Task.CompletedTask;
        }

    }
}
