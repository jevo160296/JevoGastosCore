﻿using JevoGastosCore.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class EtiquetaDAO : JevoGastosDAO<Etiqueta>
    {
        public override ObservableCollection<Etiqueta> Items
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

        public EtiquetaDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public ObservableCollection<Etiqueta> Get()
        {
            var load = Context.Etiquetas.ToList();
            ObservableCollection<Etiqueta> loadingEtiquetas = Context.Etiquetas.Local.ToObservableCollection();
            UpdateTotal(loadingEtiquetas, Container);
            return loadingEtiquetas;
        }
        public ObservableCollection<Ingreso> GetIngresos()
        {
            var load = Context.Ingresos.ToList();
            ObservableCollection<Ingreso> loadingEtiquetas = Context.Ingresos.Local.ToObservableCollection();
            UpdateTotal(loadingEtiquetas, Container);
            return loadingEtiquetas;
        }
        public ObservableCollection<Cuenta> GetCuentas()
        {
            var load = Context.Cuentas.ToList();
            ObservableCollection<Cuenta> loadingEtiquetas = Context.Cuentas.Local.ToObservableCollection();
            UpdateTotal(loadingEtiquetas, Container);
            return loadingEtiquetas;
        }
        public ObservableCollection<Gasto> GetGastos()
        {
            var load = Context.Gastos.ToList();
            ObservableCollection<Gasto> loadingEtiquetas = Context.Gastos.Local.ToObservableCollection();
            UpdateTotal(loadingEtiquetas, Container);
            return loadingEtiquetas;
        }
        public static void UpdateTotal(Etiqueta etiqueta, GastosContainer container)
        {
            etiqueta.Total =
                container.TransaccionDAO.Items.Where(p => p.Destino == etiqueta).Sum(p => p.Valor) -
                container.TransaccionDAO.Items.Where(p => p.Origen == etiqueta).Sum(p => p.Valor);
        }
        public static void UpdateTotal<T>(IEnumerable<T> etiquetas,GastosContainer container)
            where T:Etiqueta
        {
            List<int> ids = (from etiqueta in etiquetas
                             select etiqueta.Id).ToList();
            var transacciones = (from transaccion in container.TransaccionDAO.Items
                                select new { origen = transaccion.OrigenId, destino = transaccion.DestinoId, valor = transaccion.Valor })
                                .ToList();
            foreach (T etiqueta in etiquetas)
            {
                etiqueta.Total=
                    transacciones.Where(p => p.destino == etiqueta.Id).Sum(p => p.valor) -
                    transacciones.Where(p => p.origen == etiqueta.Id).Sum(p => p.valor);
            }
        }
        public static Etiqueta Delete(Etiqueta etiqueta, GastosContainer container)
        {
            //Se revisa si la etiqueta tiene Transacciones relacionadas
            if (etiqueta.TransaccionesDestino?.Count > 0 | etiqueta.TransaccionesOrigen?.Count > 0)
            {
                throw new System.Exception("No se puede eliminar la etiqueta, tiene transacciones relacionadas.");
            }
            container.EtiquetaDAO.Items.Remove(etiqueta);
            if (container.StayInSyncWithDisc)
            {
                container.Context.SaveChanges();
            }
            return etiqueta;
        }
        public static Etiqueta Add(Etiqueta etiqueta, GastosContainer container)
        {
            //Se verificará si hay etiquetas repetidas
            bool repetido = true;
            string name = etiqueta.Name;
            string type = "La etiqueta";
            if (etiqueta is Cuenta)
            {
                List<Cuenta> cs;
                if (container.CuentaDAO.ItemsLoaded)
                {
                    cs = new List<Cuenta>(container.CuentaDAO.Items);
                }
                else
                {
                    cs = container.Context.Cuentas.ToList();
                }
                repetido = EtiquetaDAO.In<Cuenta>(cs, name);
                type = "La cuenta";
            }
            else if (etiqueta is Ingreso)
            {
                List<Ingreso> cs;
                if (container.IngresoDAO.ItemsLoaded)
                {
                    cs = new List<Ingreso>(container.IngresoDAO.Items);
                }
                else
                {
                    cs = container.Context.Ingresos.ToList();
                }
                repetido = EtiquetaDAO.In<Ingreso>(cs, name);
                type = "El ingreso";
            }
            else if (etiqueta is Gasto)
            {
                List<Gasto> cs;
                if (container.GastoDAO.ItemsLoaded)
                {
                    cs = new List<Gasto>(container.GastoDAO.Items);
                }
                else
                {
                    cs = container.Context.Gastos.ToList();
                }
                repetido = EtiquetaDAO.In<Gasto>(cs, name);
                type = "El gasto";
            }
            if (repetido)
            {
                throw new System.Exception($"{type} ya existe");
            }
            container.EtiquetaDAO.Items.Add(etiqueta);
            return etiqueta;
        }
        public static bool In<T>(IEnumerable<T> list, string name)
            where T : Etiqueta
        {
            bool respuesta = false;
            foreach (T etiqueta in list)
            {
                if (etiqueta.Name == name)
                {
                    respuesta = true;
                    return respuesta;
                }
            }
            return respuesta;
        }
    }
}
