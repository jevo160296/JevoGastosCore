using JevoGastosCore.Model;

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
            EtiquetaDAO.Add(added, Container);
            Context.SaveChanges();
            return added;
        }
        public Gasto Remove(Gasto etiqueta)
        {
            return (Gasto)EtiquetaDAO.Delete(etiqueta, Container);
        }
    }
}
