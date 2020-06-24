using JevoGastosCore.Model;
using System.Collections.ObjectModel;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class IngresoDAO : JevoGastosDAO<Ingreso>
    {
        public IngresoDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public override ObservableCollection<Ingreso> Items
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

        public ObservableCollection<Ingreso> Get()
        {
            return Container.EtiquetaDAO.GetIngresos();
        }

        public Ingreso Add(string name)
        {
            Ingreso added = new Ingreso() { Name = name };
            EtiquetaDAO.Add(added, Container);
            if (Container.StayInSyncWithDisc)
            {
                Context.SaveChanges();
            }
            return added;
        }
        public Ingreso Remove(Ingreso etiqueta)
        {
            return (Ingreso)EtiquetaDAO.Delete(etiqueta, Container);
        }
    }
}
