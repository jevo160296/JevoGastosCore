using System.Collections.Generic;
using System.Linq;

namespace JevoGastosCore.Model
{
    public class Ingreso : Etiqueta 
    {
        public override double CalculateTotal(IEnumerable<Transaccion> transaccionesDestino, IEnumerable<Transaccion> transaccionesOrigen)
        {
            return transaccionesOrigen.Sum(p => p.Valor) - transaccionesDestino.Sum(p => p.Valor);
        }
    }
}
