using JevoGastosCore.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class TransaccionDAO : JevoGastosDAO<Transaccion>
    {
        public TransaccionDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public override ObservableCollection<Transaccion> Items
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


        public ObservableCollection<Transaccion> Get()
        {
            var load = Context.Transacciones.ToList();
            return Context.Transacciones.Local.ToObservableCollection();
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
            this.Container.TransaccionDAO.Items.Add(transaccion);
            if (Container.StayInSyncWithDisc)
            {
                Context.SaveChanges();
            }
            UpdateTotal(transaccion);
            return transaccion;
        }
        public Transaccion Remove(Transaccion transaccion)
        {
            Container.TransaccionDAO.Items.Remove(transaccion);
            UpdateTotal(transaccion);
            //Actualizando propiedades de navegacion
            transaccion.Origen?.TransaccionesDestino?.Remove(transaccion);
            transaccion.Origen?.TransaccionesOrigen?.Remove(transaccion);
            transaccion.Destino?.TransaccionesDestino?.Remove(transaccion);
            transaccion.Origen?.TransaccionesOrigen?.Remove(transaccion);
            if (Container.StayInSyncWithDisc)
            {
                Context.SaveChanges();
            }
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
