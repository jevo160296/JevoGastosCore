using JevoGastosCore.Enums;
using JevoGastosCore.Model;
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
                    items.CollectionChanged += Items_CollectionChanged;
                    foreach (Etiqueta item in items)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
                return items;
            }
        }
        public EtiquetaDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public ObservableCollection<Etiqueta> Get()
        {
            IQueryable<Etiqueta> query = Context.Etiquetas.OrderBy(p => p.Name);
            Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.Load(query);
            ObservableCollection<Etiqueta> loadingEtiquetas = Context.Etiquetas.Local.ToObservableCollection();
            return loadingEtiquetas;
        }
        public ObservableCollection<Ingreso> GetIngresos()
        {
            IQueryable<Ingreso> query = Context.Ingresos.OrderBy(p => p.Name);
            Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.Load(query);
            ObservableCollection<Ingreso> loadingEtiquetas = Context.Ingresos.Local.ToObservableCollection();
            return loadingEtiquetas;
        }
        public ObservableCollection<Cuenta> GetCuentas()
        {
            IQueryable<Cuenta> query = Context.Cuentas.OrderBy(p=>p.EsAhorro).ThenBy(p=>p.Name);
            Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.Load(query);
            ObservableCollection<Cuenta> loadingEtiquetas = Context.Cuentas.Local.ToObservableCollection();
            return loadingEtiquetas;
        }
        public ObservableCollection<Gasto> GetGastos()
        {
            IQueryable<Gasto> query = Context.Gastos.OrderBy(p => p.Name);
            Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.Load(query);
            ObservableCollection<Gasto> loadingEtiquetas = Context.Gastos.Local.ToObservableCollection();
            return loadingEtiquetas;
        }
        public ObservableCollection<Credito> GetCreditos()
        {
            IQueryable<Credito> query = Context.Creditos.OrderBy(p => p.Name);
            Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.Load(query);
            ObservableCollection<Credito> loadingEtiquetas = Context.Creditos.Local.ToObservableCollection();
            return loadingEtiquetas;
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
            else if (etiqueta is Credito)
            {
                List<Credito> cs;
                if (container.CreditoDAO.ItemsLoaded)
                {
                    cs = new List<Credito>(container.CreditoDAO.Items);
                }
                else
                {
                    cs = container.Context.Creditos.ToList();
                }
                repetido = EtiquetaDAO.In<Credito>(cs, name);
                type = "El credito";
            }
            if (repetido)
            {
                throw new System.Exception($"{type} ya existe");
            }
            container.EtiquetaDAO.Items.Add(etiqueta);
            return etiqueta;
        }
        public static bool Clear(TipoEtiqueta tipo,GastosContainer container)
        {
            List<Etiqueta> deletingEtiquetas=new List<Etiqueta>();
            bool cleared=false;
            switch (tipo)
            {
                case TipoEtiqueta.Ingreso:
                    deletingEtiquetas = new List<Etiqueta>(container.IngresoDAO.Items);
                    if (IsSafeToDelete(deletingEtiquetas, container))
                    {
                        container.IngresoDAO.Items.Clear();
                        cleared = true;
                    }
                    break;
                case TipoEtiqueta.Cuenta:
                    deletingEtiquetas = new List<Etiqueta>(container.CuentaDAO.Items);
                    if (IsSafeToDelete(deletingEtiquetas, container))
                    {
                        container.CuentaDAO.Items.Clear();
                        cleared = true;
                    }
                    break;
                case TipoEtiqueta.Gasto:
                    deletingEtiquetas = new List<Etiqueta>(container.GastoDAO.Items);
                    if (IsSafeToDelete(deletingEtiquetas, container))
                    {
                        container.GastoDAO.Items.Clear();
                        cleared = true;
                    }
                    break;
                case TipoEtiqueta.Credito:
                    deletingEtiquetas = new List<Etiqueta>(container.CreditoDAO.Items);
                    if (IsSafeToDelete(deletingEtiquetas, container))
                    {
                        container.CreditoDAO.Items.Clear();
                        cleared = true;
                    }
                    break;
                default:
                    break;
            }
            return cleared;
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
        public static TipoEtiqueta Tipo(Etiqueta etiqueta)
        {
            if (etiqueta is Ingreso)
            {
                return TipoEtiqueta.Ingreso;
            }
            else if(etiqueta is Cuenta)
            {
                return TipoEtiqueta.Cuenta;
            }
            else if (etiqueta is Gasto)
            {
                return TipoEtiqueta.Gasto;
            }
            else if (etiqueta is Credito)
            {
                return TipoEtiqueta.Credito;
            }
            else
            {
                return TipoEtiqueta.Ingreso;
            }
        }
        private static bool IsSafeToDelete(Etiqueta etiqueta, GastosContainer container)
        {
            return etiqueta.TransaccionesDestino.Count == 0 && etiqueta.TransaccionesOrigen.Count == 0 && etiqueta.Planes.Count == 0;
        }
        private static bool IsSafeToDelete(IList<Etiqueta> etiquetas,GastosContainer container)
        {
            var destinoTrans = from transaccion in container.TransaccionDAO.Items
                               join etiqueta in etiquetas on transaccion.DestinoId equals etiqueta.Id
                               select new { transaccion.Id };
            var origenTrans = from transaccion in container.TransaccionDAO.Items
                               join etiqueta in etiquetas on transaccion.OrigenId equals etiqueta.Id
                               select new { transaccion.Id };
            var planes = from plan in container.PlanDAO.Items
                         join etiqueta in etiquetas on plan.EtiquetaId equals etiqueta.Id
                         select new { plan.Id };
            return destinoTrans.Count() == 0 && origenTrans.Count() == 0 && planes.Count() == 0;
        }
    }
}
