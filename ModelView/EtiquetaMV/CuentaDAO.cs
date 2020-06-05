using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class CuentaDAO : JevoGastosDAO<Cuenta>
    {
        public override DAOList Items
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

        public DAOList Get()
        {
            return new DAOList(Container.EtiquetaDAO.Get<Cuenta>());
        }

        public Cuenta Add(string name)
        {
            Cuenta added = new Cuenta() { Name = name };
            Container.Context.Cuentas.Add(added);
            Context.SaveChanges();
            if (ItemsLoaded)
            {
                Items.Add(added);
            }
            if (Container.EtiquetaDAO.ItemsLoaded)
            {
                Container.EtiquetaDAO.Items.Add(added);
            }
            return added;
        }
    }
}
