using JevoGastosCore.Model;
using System.Collections.Generic;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class EtiquetaDAO : JevoGastosDAO<Etiqueta>
    {
        public override DAOList Items
        {
            get
            {
                if (items is null)
                {
                    items = new DAOList(Get<Etiqueta>());
                }
                return items;
            }
        }

        public EtiquetaDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public List<T> Get<T>()
            where T : Etiqueta
        {
            var loadingEtiquetas = Context.Etiquetas.OfType<T>().ToList();
            foreach (var etiqueta in loadingEtiquetas)
            {
                UpdateTotal(etiqueta, Container);
            }
            return loadingEtiquetas;
        }
        public static void UpdateTotal(Etiqueta etiqueta, GastosContainer container)
        {
            etiqueta.Total =
                container.Context.Transacciones.Where(p => p.DestinoId == etiqueta.Id).Sum(p => p.Valor) -
                container.Context.Transacciones.Where(p => p.OrigenId == etiqueta.Id).Sum(p => p.Valor);
            EtiquetaDAO.Update(etiqueta, container);
        }
        public static Etiqueta Update(Etiqueta etiqueta, GastosContainer container)
        {
            container.Context.Etiquetas.Update(etiqueta);
            container.SaveChanges();
            int index;
            if (container.EtiquetaDAO.ItemsLoaded)
            {
                index = container.EtiquetaDAO.Items.IndexOf(etiqueta);
                container.EtiquetaDAO.Items.Remove(etiqueta);
                container.EtiquetaDAO.Items.Insert(index, etiqueta);
            }
            if (etiqueta is Cuenta & container.CuentaDAO.ItemsLoaded)
            {
                index = container.CuentaDAO.Items.IndexOf((Cuenta)etiqueta);
                container.CuentaDAO.Items.Remove((Cuenta)etiqueta);
                container.CuentaDAO.Items.Insert(index, (Cuenta)etiqueta);
            }
            else if (etiqueta is Ingreso & container.IngresoDAO.ItemsLoaded)
            {
                index = container.IngresoDAO.Items.IndexOf((Ingreso)etiqueta);
                container.IngresoDAO.Items.Remove((Ingreso)etiqueta);
                container.IngresoDAO.Items.Insert(index, (Ingreso)etiqueta);
            }
            else if (etiqueta is Gasto & container.GastoDAO.ItemsLoaded)
            {
                index = container.GastoDAO.Items.IndexOf((Gasto)etiqueta);
                container.GastoDAO.Items.Remove((Gasto)etiqueta);
                container.GastoDAO.Items.Insert(index, (Gasto)etiqueta);
            }
            return etiqueta;
        }
        public static Etiqueta Delete(Etiqueta etiqueta, GastosContainer container)
        {
            //Se revisa si la etiqueta tiene Transacciones relacionadas
            if (etiqueta.TransaccionesDestino?.Count > 0 | etiqueta.TransaccionesOrigen?.Count > 0)
            {
                throw new System.Exception("No se puede eliminar la etiqueta, tiene transacciones relacionadas.");
            }
            container.Context.Etiquetas.Remove(etiqueta);
            container.Context.SaveChanges();
            if (container.EtiquetaDAO.ItemsLoaded)
            {
                container.EtiquetaDAO.Items.Remove(etiqueta);
            }
            if (etiqueta is Cuenta & container.CuentaDAO.ItemsLoaded)
            {
                container.CuentaDAO.Items.Remove((Cuenta)etiqueta);
            }
            else if (etiqueta is Ingreso & container.IngresoDAO.ItemsLoaded)
            {
                container.IngresoDAO.Items.Remove((Ingreso)etiqueta);
            }
            else if (etiqueta is Gasto & container.GastoDAO.ItemsLoaded)
            {
                container.GastoDAO.Items.Remove((Gasto)etiqueta);
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
            container.Context.Etiquetas.Add(etiqueta);
            if (container.EtiquetaDAO.ItemsLoaded)
            {
                container.EtiquetaDAO.Items.Add(etiqueta);
            }
            if (etiqueta is Cuenta & container.CuentaDAO.ItemsLoaded)
            {
                container.CuentaDAO.Items.Add((Cuenta)etiqueta);
            }
            else if (etiqueta is Ingreso & container.IngresoDAO.ItemsLoaded)
            {
                container.IngresoDAO.Items.Add((Ingreso)etiqueta);
            }
            else if (etiqueta is Gasto & container.GastoDAO.ItemsLoaded)
            {
                container.GastoDAO.Items.Add((Gasto)etiqueta);
            }
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
