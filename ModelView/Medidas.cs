using JevoGastosCore.Esqueleto;
using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class Medidas:Vista<GastosContext,GastosContainer>
    {
        private double? libre = null;

        public double? Libre
        {
            get
            {
                if (libre is null)
                {
                    libre =CalculateLibre();
                    this.Container.PropertyChanged += Container_PropertyChanged;
                    this.Container.PlanDAO.Items.CollectionChanged += Items_CollectionChanged; ;
                }
                return libre;
            }
            private set
            {
                libre = value;
                this.OnPropertyChanged();
            }
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateLibre();
        }
        private void Container_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateLibre();
        }
        private void UpdateLibre()
        {
            Libre = CalculateLibre();
        }
        private double CalculateLibre()
        {
            return 
                this.Container.CuentaDAO.Items.Where(p=>!p.EsAhorro).Sum(p => p.Total) +
                2 * this.Container.PlanDAO.Items.Where(p=>p.Etiqueta is Ingreso).Sum(p=>p.Falta) +
                2 * this.Container.PlanDAO.Items.Where(p => p.Etiqueta is Cuenta).Sum(p => p.Falta) -
                this.Container.PlanDAO.Items.Sum(p => p.Falta);
        }
        public Medidas(GastosContainer container) : base(container)
        {

        }
    }
}
