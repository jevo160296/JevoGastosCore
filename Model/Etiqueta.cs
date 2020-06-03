using System.Collections.Generic;

namespace JevoGastosCore.Model
{
    /// <summary>
    /// Representa una etiqueta
    /// </summary>
    public abstract class Etiqueta
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Transaccion> TransaccionesOrigen { get; set; }
        public List<Transaccion> TransaccionesDestino { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
        public double Total { get; set; }
    }
}
