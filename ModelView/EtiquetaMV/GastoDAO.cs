 using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class GastoDAO : JevoGastosDAO<Gasto>
    {
        public GastoDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

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

        public DAOList Get()
        {
            return new DAOList(Container.EtiquetaDAO.Get<Gasto>());
        }

        public Gasto Add(string name)
        {
            Gasto added = new Gasto() { Name = name };
            Context.Gastos.Add(added);
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
