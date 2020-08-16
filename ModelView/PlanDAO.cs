using JevoGastosCore.Enums;
using JevoGastosCore.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class PlanDAO:JevoGastosDAO<Plan>
    {
        public PlanDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public override ObservableCollection<Plan> Items
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

        public ObservableCollection<Plan> Get()
        {
            var load = Context.Planes.ToList();
            return Context.Planes.Local.ToObservableCollection();
        }

        private Plan Add(Etiqueta etiqueta,TipoPlan tipo,double meta)
        {
            return Add(new Plan()
            {
                Etiqueta = etiqueta,
                Tipo = tipo,
                Meta = meta
            });
        }

        private Plan Add(Plan plan)
        {
            this.Container.PlanDAO.Items.Add(plan);
            if (Container.StayInSyncWithDisc)
            {
                Context.SaveChanges();
            }
            return plan;
        }
    }
}
