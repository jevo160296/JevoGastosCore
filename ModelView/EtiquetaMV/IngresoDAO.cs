using JevoGastosCore.Model;

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
            EtiquetaDAO.Add(added, Container);
            Context.SaveChanges();
            return added;
        }
        public Ingreso Remove(Ingreso etiqueta)
        {
            return (Ingreso)EtiquetaDAO.Delete(etiqueta, Container);
        }
    }
}
