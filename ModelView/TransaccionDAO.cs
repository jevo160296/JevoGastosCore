using JevoGastosCore.Model;
using System;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class TransaccionDAO : JevoGastosDAO<Transaccion>
    {
        public TransaccionDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        private new DAOList items = null;
        public override DAOList Items
        {
            get
            {
                if (items is null)
                {
                    items = Get();
                }
                return items;
            }
        }

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
            if (!(items is null))
            {
                Items.Remove(transaccion);
            }
            UpdateTotal(transaccion);
            return transaccion;
        }

        public DAOList Get()
        {
            return new DAOList(Context.Transacciones.ToList());
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
            if (!(items is null))
            {
                Items.Add(transaccion);
            }
            UpdateTotal(transaccion);
            return transaccion;
        }
        private Transaccion Update(Transaccion transaccion)
        {
            int index;
            Context.Transacciones.Update(transaccion);
            Context.SaveChanges();
            if (!(items is null))
            {
                index = items.IndexOf(transaccion);
                items.Remove(transaccion);
                items.Insert(index, transaccion);
            }
            UpdateTotal(transaccion);
            return transaccion;
        }

        private Transaccion UpdateTotal(Transaccion transaccion)
        {
            EtiquetaDAO.UpdateTotal(transaccion.Origen, Container);
            EtiquetaDAO.UpdateTotal(transaccion.Destino, Container);
            return transaccion;
        }
    }
}
