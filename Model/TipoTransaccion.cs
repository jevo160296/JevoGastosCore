using System;
using System.Collections.Generic;
using System.Text;

namespace JevoGastosCore.Model
{
    public class TipoTransaccion
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TOrigenId { get; set; }
        public TipoEtiqueta TOrigen { get; set; }

        public int TDestinoId { get; set; }
        public TipoEtiqueta TDestino { get; set; }

        public List<Transaccion> Transacciones { get; set; }
    }
}
