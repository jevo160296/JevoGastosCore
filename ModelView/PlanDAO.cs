using JevoGastosCore.Clases;
using JevoGastosCore.Enums;
using JevoGastosCore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class PlanDAO:JevoGastosDAO<Plan>
    {
        public PlanDAO(GastosContainer gastosContainer) : base(gastosContainer) 
        {
            Container.PayDaysDAO.Items.CollectionChanged += PayDays_CollectionChanged;
        }

        private void PayDays_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    foreach (Plan plan in Items)
                    {
                        CalculateProperty(plan, PlanProperty.ValorActual);
                        CalculateProperty(plan, PlanProperty.MetaDiaria);
                        CalculateProperty(plan, PlanProperty.MetaMensual);
                    }
                    break;
                default:
                    break;
            }
        }

        public override ObservableCollection<Plan> Items
        {
            get
            {
                if (items is null)
                {
                    items = Get();
                    items.CollectionChanged += Items_CollectionChanged;
                    foreach (Plan item in items)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                        item.VoidPropertyRequested += Item_VoidPropertyRequested;
                    }
                }
                return items;
            }
        }

        protected override void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.Items_CollectionChanged(sender, e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Plan item in e.NewItems)
                    {
                        item.VoidPropertyRequested += Item_VoidPropertyRequested;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Plan item in e.OldItems)
                    {
                        item.VoidPropertyRequested -= Item_VoidPropertyRequested;
                    }
                    break;
                default:
                    break;
            }
        }

        private void Item_VoidPropertyRequested(Plan sender, PlanProperty e)
        {
            CalculateProperty(sender, e);
        }

        private void CalculateProperty(Plan sender, PlanProperty e)
        {
            switch (e)
            {
                case PlanProperty.Falta:
                    sender.Falta = CalculateFalta(sender);
                    break;
                case PlanProperty.Meta:
                    break;
                case PlanProperty.MetaMensual:
                    sender.MetaMensual = CalculateMetaMensual(sender);
                    break;
                case PlanProperty.MetaDiaria:
                    sender.MetaDiaria = CalculateMetaDiaria(sender);
                    break;
                case PlanProperty.ValorActual:
                    sender.ValorActual = CalculateValorActual(sender);
                    break;
                default:
                    break;
            }
        }
        private void UpdateDependentProperties(Plan sender,PlanProperty e)
        {
            List<PlanProperty> properties = sender.UpdateTracker.Keys.Contains(e) ? new List<PlanProperty>(sender.UpdateTracker[e]) : new List<PlanProperty>();
            foreach (PlanProperty property in properties)
            {
                CalculateProperty(sender, property);
            }
        }

        protected override void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PlanProperty property;
            bool parsed = Enum.TryParse(e.PropertyName, out property);
            if (!parsed)
            {
                property = PlanProperty.NotFound;
            }
            UpdateDependentProperties((Plan)sender, property);
            base.Item_PropertyChanged(sender, e);
        }
        public ObservableCollection<Plan> Get()
        {
            var load = Context.Planes.ToList();
            foreach (Plan item in load)
            {
                item.Etiqueta = item.Etiqueta;
            }
            return Context.Planes.Local.ToObservableCollection();
        }
        public Plan Add(Etiqueta etiqueta,TipoPlan tipo,bool esMesFijo,double meta)
        {
            return Add(new Plan()
            {
                Etiqueta=etiqueta,
                Tipo = tipo,
                Meta = meta,
                EsMesFijo = esMesFijo
            }) ;
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
        public void Clear()
        {
            foreach (Plan item in Container.PlanDAO.Items)
            {
                item.PropertyChanged -= Item_PropertyChanged;
                item.VoidPropertyRequested -= Item_VoidPropertyRequested;
            }
            Container.PlanDAO.Items.Clear();
            //Actualizando propiedades de navegación
            foreach (Etiqueta etiqueta in Container.EtiquetaDAO.Items)
            {
                while (etiqueta.Planes.Count>0)
                {
                    etiqueta.Planes.RemoveAt(0);
                }
            }
        }
        public Plan Remove(Plan plan)
        {
            Container.PlanDAO.Items.Remove(plan);
            //Actualizando propiedades de navegacion
            plan.Etiqueta?.Planes?.Remove(plan);
            if (Container.StayInSyncWithDisc)
            {
                Context.SaveChanges();
            }
            return plan;
        }
        public void Remove(IList<Plan> planes)
        {
            List<Plan> helper = new List<Plan>(planes);
            foreach (Plan plan in helper)
            {
                Remove(plan);
            }
        }

        #region Metodos
        private double CalculateMetaMensual(Plan plan)
        {
            double mensual = double.NaN;
            PlanProperty property = PlanProperty.MetaMensual;
            List<PlanProperty> properties;
            switch (plan.Tipo)
            {
                case TipoPlan.Diario:
                    mensual = plan.MetaDiaria * ((this.Container.PayDaysDAO.EndDate - DateTime.Today).TotalDays + 1);
                    properties = new List<PlanProperty>() { PlanProperty.MetaDiaria,PlanProperty.Tipo };
                    break;
                case TipoPlan.Mensual:
                    mensual = plan.Meta;
                    properties = new List<PlanProperty>() { PlanProperty.Meta, PlanProperty.Tipo };
                    break;
                default:
                    properties = new List<PlanProperty>();
                    break;
            }
            SetDependentProperties(plan, property, properties);
            return mensual;
        }
        private double CalculateMetaDiaria(Plan plan)
        {
            double diario = double.NaN;
            PlanProperty property = PlanProperty.MetaDiaria;
            List<PlanProperty> properties;
            switch (plan.Tipo)
            {
                case TipoPlan.Diario:
                    diario =
                        plan.EsMesFijo
                        ? plan.Meta / ((this.Container.PayDaysDAO.EndDate - this.Container.PayDaysDAO.StartDate).TotalDays + 1)
                        : plan.Meta;
                    properties = new List<PlanProperty>() { PlanProperty.EsMesFijo, PlanProperty.Meta,PlanProperty.Tipo };
                    break;
                case TipoPlan.Mensual:
                    diario = 0;
                    properties = new List<PlanProperty>() { PlanProperty.Tipo };
                    break;
                default:
                    properties = new List<PlanProperty>();
                    break;
            }
            SetDependentProperties(plan, property, properties);
            return diario;
        }
        private double CalculateValorActual(Plan plan)
        {
            double respuesta = double.NaN;
            if (!(plan.Etiqueta is null))
            {
                if (plan.Tipo==TipoPlan.Diario)
                {
                    SetDependentProperties(plan, PlanProperty.ValorActual, new List<PlanProperty>() { PlanProperty.Total, PlanProperty.Etiqueta,PlanProperty.Tipo });
                    respuesta = Math.Min( 
                        plan.Etiqueta.CalculateTotal
                        (
                        plan.Etiqueta.TransaccionesDestino.Where(p => p.Fecha == DateTime.Today),
                        plan.Etiqueta.TransaccionesOrigen.Where(p => p.Fecha == DateTime.Today)
                        ),
                        plan.MetaDiaria
                        )
                        ;
                }
                else
                {
                    SetDependentProperties(plan, PlanProperty.ValorActual, new List<PlanProperty>() { PlanProperty.Total, PlanProperty.Etiqueta });
                    respuesta = plan.Etiqueta.CalculateTotal
                        (
                        plan.Etiqueta.TransaccionesDestino.Where(p => p.Fecha >= this.Container.PayDaysDAO.StartDate & p.Fecha < this.Container.PayDaysDAO.EndDate),
                        plan.Etiqueta.TransaccionesOrigen.Where(p => p.Fecha >= this.Container.PayDaysDAO.StartDate & p.Fecha < this.Container.PayDaysDAO.EndDate)
                        );
                }
            }
            return respuesta;
        }
        private double CalculateFalta(Plan plan)
        {
            double valor = plan.MetaMensual - plan.ValorActual;
            SetDependentProperties(plan, PlanProperty.Falta, new List<PlanProperty>() { PlanProperty.MetaMensual, PlanProperty.ValorActual });
            return plan.Etiqueta is Credito ? valor : valor > 0 ? valor : 0;
        }

        private void SetDependentProperties(Plan plan, PlanProperty property, List<PlanProperty> properties)
        {
            foreach (PlanProperty item in plan.UpdateTracker.Keys)
            {
                plan.UpdateTracker[item]?.Remove(property);
            }
            foreach (PlanProperty item in properties)
            {
                if (!(plan.UpdateTracker.Keys.Contains(item)))
                {
                    plan.UpdateTracker[item] = new List<PlanProperty>() { property };
                }
                else
                {
                    plan.UpdateTracker[item].Add(property);
                }
            }   
        }
        #endregion
    }
}