using JevoGastosCore.Model;
using System;

namespace JevoGastosCore.ModelView
{
    public class TransaccionDAO : JevoGastosDAO<Transaccion>
    {
        public TransaccionDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public Transaccion Transaccion<O, D>(O origen, D destino, double valor, string descripcion, DateTime? fecha = null)
            where O : Etiqueta
            where D : Etiqueta
        {
            return Add(origen, destino, valor, descripcion, fecha);
        }
        public Transaccion Entrada(Ingreso origen, Cuenta destino, double valor, string descripcion, DateTime? fecha = null)
        {
            return Add(origen, destino, valor, descripcion, fecha);

        }
        public Transaccion Movimiento(Cuenta origen, Cuenta destino, double valor, string descripcion, DateTime? fecha = null)
        {
            return Add(origen, destino, valor, descripcion, fecha);
        }
        public Transaccion Salida(Cuenta origen, Gasto destino, double valor, string descripcion, DateTime? fecha = null)
        {
            return Add(origen, destino, valor, descripcion, fecha);
        }

        public Transaccion Actualizar(Transaccion transaccion)
        {
            return Update(transaccion);
        }

        public Transaccion Remove(Transaccion transaccion)
        {
            this.Context.Transacciones.Remove(transaccion);
            Context.SaveChanges();
            UpdateTotal(transaccion);
            return transaccion;
        }

        private Transaccion Add<O, D>(O origen, D destino, double valor, string descripcion)
            where O : Etiqueta
            where D : Etiqueta
        {
            return Add(origen, destino, valor, descripcion, DateTime.Now);
        }
        private Transaccion Add<O, D>(O origen, D destino, double valor, string descripcion, DateTime? fecha)
            where O : Etiqueta
            where D : Etiqueta
        {
            if (fecha is null)
            {
                return Add(origen, destino, valor, descripcion);
            }
            else
            {
                return Add(origen, destino, valor, descripcion, (DateTime)fecha);
            }
        }
        private Transaccion Add<O, D>(O origen, D destino, double valor, string descripcion, DateTime fecha)
            where O : Etiqueta
            where D : Etiqueta
        {
            Transaccion newTransaccion = new Transaccion()
            {
                Origen = origen,
                Destino = destino,
                Valor = valor,
                Fecha = fecha,
                Descripcion = descripcion,
            };
            Add(newTransaccion);
            return newTransaccion;
        }

        private Transaccion Add(Transaccion transaccion)
        {
            this.Context.Transacciones.Add(transaccion);
            Context.SaveChanges();
            UpdateTotal(transaccion);
            return transaccion;
        }
        private Transaccion Update(Transaccion transaccion)
        {
            Context.Transacciones.Update(transaccion);
            Context.SaveChanges();
            UpdateTotal(transaccion);
            return transaccion;
        }

        private Transaccion UpdateTotal(Transaccion transaccion)
        {
            EtiquetaDAO.UpdateTotal(transaccion.Origen, Context);
            EtiquetaDAO.UpdateTotal(transaccion.Destino, Context);
            return transaccion;
        }

    }
}
