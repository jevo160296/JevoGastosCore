using JevoGastosCore.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class EtiquetaDAO : JevoGastosDAO<Etiqueta>
    {
        public override DAOList Items => Get<Etiqueta>();

        public EtiquetaDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public DAOList Get<T>()
            where T : Etiqueta
        {
            return new DAOList(Context.Etiquetas.OfType<T>().ToList());
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
            if (container.EtiquetaDAO.ItemsLoaded)
            {
                container.EtiquetaDAO.Items.Remove(etiqueta);
                container.EtiquetaDAO.Items.Add(etiqueta);
            }
            if (etiqueta is Cuenta & container.CuentaDAO.ItemsLoaded)
            {
                container.CuentaDAO.Items.Remove((Cuenta)etiqueta);
                container.CuentaDAO.Items.Add((Cuenta)etiqueta);
            }
            else if (etiqueta is Ingreso & container.IngresoDAO.ItemsLoaded)
            {
                container.IngresoDAO.Items.Remove((Ingreso)etiqueta);
                container.IngresoDAO.Items.Add((Ingreso)etiqueta);
            }
            else if (etiqueta is Gasto & container.GastoDAO.ItemsLoaded)
            {
                container.GastoDAO.Items.Remove((Gasto)etiqueta);
                container.GastoDAO.Items.Add((Gasto)etiqueta);
            }
            container.Context.SaveChanges();
            return etiqueta;
        }
        public static Etiqueta Delete(Etiqueta etiqueta, GastosContainer container)
        {
            container.Context.Etiquetas.Remove(etiqueta);
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
            container.Context.SaveChanges();
            return etiqueta;
        }
    }
}
