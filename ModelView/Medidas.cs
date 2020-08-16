using JevoGastosCore.Esqueleto;
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
                    libre =
                        this.Container.CuentaDAO.Items.Sum(p => p.Total) -
                        this.Container.PlanDAO.Items.Sum(p => p.Meta);
                    this.Container.PropertyChanged += Container_PropertyChanged;
                }
                return libre;
            }
            private set
            {
                libre = value;
                this.OnPropertyChanged();
            }
        }
        private void Container_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateLibre();
        }
        private void UpdateLibre()
        {
            Libre =
                        this.Container.CuentaDAO.Items.Sum(p => p.Total) -
                        this.Container.PlanDAO.Items.Sum(p => p.Meta);
        }

        public Medidas(GastosContainer container) : base(container)
        {

        }
    }
}
