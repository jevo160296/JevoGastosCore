using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class CuentaDAO : JevoGastosDAO<Cuenta>
    {
        private new DAOList items = null;
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
            return new DAOList(Context.Cuentas.ToList());
        }

        public Cuenta Add(string name)
        {
            Cuenta added = new Cuenta() { Name = name };
            Container.Context.Cuentas.Add(added);
            if (ItemsLoaded)
            {
                Items.Add(added);
            }
            if (Container.EtiquetaDAO.ItemsLoaded)
            {
                Container.EtiquetaDAO.Items.Add(added);
            }
            Context.SaveChanges();
            return added;
        }
    }
}
