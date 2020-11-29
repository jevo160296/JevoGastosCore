using JevoGastosCore.Enums;
using JevoGastosCore.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
            IQueryable<Transaccion> query = Context.Transacciones.OrderByDescending(p => p.Fecha);
            Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.Load<Transaccion>(query);
            ObservableCollection<Transaccion> returnedData = Context.Transacciones.Local.ToObservableCollection();
            return returnedData;
        }

        public static TipoTransaccion Tipo(Transaccion transaccion)
        {
            TipoEtiqueta tipoEtiquetaOrigen, tipoEtiquetaDestino;
            tipoEtiquetaOrigen = EtiquetaDAO.Tipo(transaccion.Origen);
            tipoEtiquetaDestino = EtiquetaDAO.Tipo(transaccion.Destino);
            Array TiposDisponibles = Enum.GetValues(typeof(TipoTransaccion));
            int tipoInt = ((int)tipoEtiquetaOrigen << 3) + (int)tipoEtiquetaDestino;
            int likeness = 0;
            int max = 0;
            foreach (int item in TiposDisponibles)
            {
                if ((item & tipoInt)>likeness)
                {
                    likeness = (item & tipoInt);
                    max = item;
                }
            }
            TipoTransaccion tipo = (TipoTransaccion)max;
            return tipo;
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
            int index = 0;
            while (index<Items.Count && transaccion.Fecha<Items[index].Fecha)
            {
                index++;
            }
            if (index==Items.Count)
            {
                this.Items.Add(transaccion);
            }
            else
            {
                this.Items.Insert(index, transaccion);
            }
            if (Container.StayInSyncWithDisc)
            {
                Context.SaveChanges();
            }
            return transaccion;
        }
        public Transaccion Remove(Transaccion transaccion)
        {
            this.Items.Remove(transaccion);
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
            this.Items.Clear();
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
    }
}
