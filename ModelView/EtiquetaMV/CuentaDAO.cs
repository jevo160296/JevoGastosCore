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

        public Cuenta Add(string name, bool esAhorro)
        {
            Cuenta added = new Cuenta() { Name = name, EsAhorro = esAhorro };
            return Add(added);
        }
        public Cuenta Add(string name)
        {
            Cuenta added = new Cuenta() { Name = name };
            return Add(added);
        }
        public Cuenta Remove(Cuenta etiqueta)
        {
            return (Cuenta)EtiquetaDAO.Delete(etiqueta, Container);
        }
        private Cuenta Add(Cuenta cuenta)
        {
            EtiquetaDAO.Add(cuenta, Container);
            if (Container.StayInSyncWithDisc)
            {
                Container.SaveChanges();
            }
            return cuenta;
        }
    }
}
