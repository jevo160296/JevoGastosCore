using JevoGastosCore.Model;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public Transaccion Prestamo(Credito origen,Cuenta destino,double valor,string descripcion,DateTime? fecha = null)
        {
            return Add(origen, destino, valor, descripcion, fecha);
        }
        public Transaccion Pago(Cuenta origen, Credito destino, double valor, string descripcion, DateTime? fecha = null)
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
        public void Remove(IList<Transaccion> transacciones)
        {
            List<Transaccion> helper = new List<Transaccion>(transacciones);
            foreach (Transaccion transaccion in helper)
            {
                Remove(transaccion);
            }
        }
        public void Clear()
        {
            Container.TransaccionDAO.Items.Clear();
            EtiquetaDAO.UpdateTotal(Container.EtiquetaDAO.Items,Container);
            //Actualizando propiedades de navegacion
            foreach (Etiqueta etiqueta in Container.EtiquetaDAO.Items)
            {
                while (etiqueta.TransaccionesOrigen.Count>0)
                {
                    etiqueta.TransaccionesOrigen.RemoveAt(0);
                }
                while (etiqueta.TransaccionesDestino.Count>0)
                {
                    etiqueta.TransaccionesDestino.RemoveAt(0);
                }
            }
        }

        private Transaccion UpdateTotal(Transaccion transaccion)
        {
            EtiquetaDAO.UpdateTotal(transaccion.Origen, Container);
            EtiquetaDAO.UpdateTotal(transaccion.Destino, Container);
            return transaccion;
        }
    }
}
