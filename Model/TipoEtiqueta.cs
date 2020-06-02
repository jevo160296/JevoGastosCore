using System;
using System.Collections.Generic;
using System.Text;

namespace JevoGastosCore.Model
{
    public class TipoEtiqueta
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Etiqueta> Etiquetas { get; set; }
        public List<TipoTransaccion> TTransaccionesOrigen { get; set; }
        public List<TipoTransaccion> TTransaccionesDestino { get; set; }
    }
}
