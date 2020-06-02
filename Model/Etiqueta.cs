using System;
using System.Collections.Generic;
using System.Text;

namespace JevoGastosCore.Model
{
    /// <summary>
    /// Representa una etiqueta
    /// </summary>
    public class Etiqueta
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TipoEtiquetaId { get; set; }
        public TipoEtiqueta TipoEtiqueta { get; set; }

        public List<Transaccion> TransaccionesOrigen { get; set; }
        public List<Transaccion> TransaccionesDestino { get; set; }
    }
}
