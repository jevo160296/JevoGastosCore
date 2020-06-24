using JevoGastosCore.Model;
using System.Collections.ObjectModel;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class GastoDAO : JevoGastosDAO<Gasto>
    {
        public GastoDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public override ObservableCollection<Gasto> Items
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

        public ObservableCollection<Gasto> Get()
        {
            return Container.EtiquetaDAO.GetGastos();
        }
        public Gasto Add(string name)
        {
            Gasto added = new Gasto() { Name = name };
            EtiquetaDAO.Add(added, Container);
            if (Container.StayInSyncWithDisc)
            {
                Context.SaveChanges();
            }
            return added;
        }
        public Gasto Remove(Gasto etiqueta)
        {
            return (Gasto)EtiquetaDAO.Delete(etiqueta, Container);
        }
    }
}
