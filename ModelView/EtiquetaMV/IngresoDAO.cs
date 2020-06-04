using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class IngresoDAO : JevoGastosDAO<Ingreso>
    {
        public IngresoDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

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

        public DAOList Get()
        {
            return new DAOList(Context.Ingresos.ToList());
        }

        public Ingreso Add(string name)
        {
            Ingreso added = new Ingreso() { Name = name };
            Context.Ingresos.Add(added);
            if (ItemsLoaded)
            {
                Items.Add(added);
            }
            Context.SaveChanges();
            return added;
        }
    }
}
