using JevoGastosCore.Model;
using System.Collections.ObjectModel;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class CreditoDAO : JevoGastosDAO<Credito>
    {
        public override ObservableCollection<Credito> Items
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


        public CreditoDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public ObservableCollection<Credito> Get()
        {
            return Container.EtiquetaDAO.GetCreditos();
        }

        public Credito Add(string name)
        {
            Credito added = new Credito() { Name = name };
            EtiquetaDAO.Add(added, Container);
            if (Container.StayInSyncWithDisc)
            {
                Container.SaveChanges();
            }
            return added;
        }
        public Credito Remove(Credito etiqueta)
        {
            return (Credito)EtiquetaDAO.Delete(etiqueta, Container);
        }
    }
}
