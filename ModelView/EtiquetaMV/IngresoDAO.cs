using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class IngresoDAO : JevoGastosDAO<Ingreso>
    {
        public IngresoDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

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
            return new DAOList(Container.EtiquetaDAO.Get<Ingreso>());
        }

        public Ingreso Add(string name)
        {
            Ingreso added = new Ingreso() { Name = name };
            Context.Ingresos.Add(added);
            Context.SaveChanges();
            if (this.ItemsLoaded)
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
