using System;
using System.Collections.Generic;
using System.Text;

namespace JevoGastosCore.Model
{
    public class Transaccion
    {
        public int Id { get; set; }

        public int TipoTransaccionId { get; set; }
        public TipoTransaccion TipoTransaccion { get; set; }

        public DateTime Fecha { get; set; }

        public int OrigenId { get; set; }
        public Etiqueta Origen { get; set; }

        public int DestinoId { get; set; }
        public Etiqueta Destino { get; set; }

        public double Valor { get; set; }
        public string Descripcion { get; set; }
    }
}
