using JevoGastosCore.Model;
using System.Collections.ObjectModel;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class CuentaDAO : JevoGastosDAO<Cuenta>
    {
        public override ObservableCollection<Cuenta> Items
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


        public CuentaDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public ObservableCollection<Cuenta> Get()
        {
            return Container.EtiquetaDAO.GetCuentas();
        }

        public Cuenta Add(string name)
        {
            Cuenta added = new Cuenta() { Name = name };
            EtiquetaDAO.Add(added, Container);
            if (Container.StayInSyncWithDisc)
            {
                Container.SaveChanges();
            }
            return added;
        }
        public Cuenta Remove(Cuenta etiqueta)
        {
            return (Cuenta)EtiquetaDAO.Delete(etiqueta, Container);
        }
    }
}
