using JevoGastosCore.Model;

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
            EtiquetaDAO.Add(added, Container);
            Container.SaveChanges();
            return added;
        }
        public Cuenta Remove(Cuenta etiqueta)
        {
            return (Cuenta)EtiquetaDAO.Delete(etiqueta, Container);
        }
    }
}
