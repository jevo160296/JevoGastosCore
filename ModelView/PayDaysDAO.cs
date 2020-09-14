using JevoGastosCore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace JevoGastosCore.ModelView
{
    public class PayDaysDAO : JevoGastosDAO<PayDay>
    {
        #region Atributos
        private DateTime? startDate = null;
        private DateTime? endDate = null;
        private DateTime? previousPayDateCalculated = null;
        private DateTime? nextPayDateCalculated = null;
        private ObservableCollection<PayDay> nonSelectedItems =null;
        #endregion
        #region Propiedades
        public override ObservableCollection<PayDay> Items
        {
            get
            {
                if (items is null)
                {
                    items = Get();
                    items.CollectionChanged += Items_CollectionChanged;
                }
                return items;
            }
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            startDate = null;
            endDate = null;
        }

        public ObservableCollection<PayDay> NonSelectedItems
        {
            get
            {
                if (nonSelectedItems is null)
                {
                    nonSelectedItems = new ObservableCollection<PayDay>();
                    for (int i = 1; i <= PayDay.EndMonth; i++)
                    {
                        if (!Items.Contains(i))
                        {
                            nonSelectedItems.Add(i);
                        }
                    }
                }
                return nonSelectedItems;
            }
        }

        public DateTime StartDate => DateTime.Now.Date != previousPayDateCalculated?.Date ? PreviousPayDate() : startDate ?? PreviousPayDate();
        public DateTime EndDate => DateTime.Now.Date != nextPayDateCalculated?.Date ? NextPayDate() : endDate ?? NextPayDate();
        #endregion
        #region Constructores
        public PayDaysDAO(GastosContainer gastosContainer) : base(gastosContainer) { }
        #endregion
        #region Metodos
        public ObservableCollection<PayDay> Get()
        {
            var load = Context.PayDays.OrderBy(p=>p.Day).ToList();
            if (load.Count==0)
            {
                Context.PayDays.Add(PayDay.EndMonth);
            }
            return Context.PayDays.Local.ToObservableCollection();
        }
        private DateTime CurrentDay() => DateTime.Now;
        private DateTime PreviousPayDate()
        {
            int day = CurrentDay().Day;
            IEnumerable<PayDay> days = Items.Count > 0 ? Items.Where(p => p <= day) : new List<PayDay>() { PayDay.EndMonth };
            startDate =
                days.Count() > 0
                ? PayDayToDate(days.Max(), CurrentDay())
                : PayDayToDate(Items.Max(), CurrentDay().AddMonths(-1));
            previousPayDateCalculated = DateTime.Now;
            OnPropertyChanged("StartDate");
            return startDate ?? CurrentDay();
        }
        private DateTime NextPayDate()
        {
            int day = CurrentDay().Day;
            IEnumerable<PayDay> days = Items.Count > 0 ? Items.Where(p => p > day) : new List<PayDay>() { PayDay.EndMonth };
            endDate =
                days.Count() > 0
                ? PayDayToDate(days.Min(), CurrentDay())
                : PayDayToDate(Items.Min(), CurrentDay().AddMonths(1));
            nextPayDateCalculated = DateTime.Now;
            OnPropertyChanged("EndDate");
            return endDate ?? CurrentDay();
        }
        private DateTime PayDayToDate(int payDay, DateTime date)
        {
            int year = date.Year, month = date.Month;
            return new DateTime(year, month, payDay >= PayDay.EndMonth || payDay > DateTime.DaysInMonth(year, month) ? DateTime.DaysInMonth(year, month) : payDay);
        }
        #endregion
        #region ItemManipulation
        public PayDay Add(int payDay)
        {
            int i = 0;
            while (Items.Count>i && Items[i]<payDay)
            {
                i++;
            }
            Items.Insert(i,payDay);
            if (!(nonSelectedItems is null))
            {
                Container.PayDaysDAO.NonSelectedItems.Remove(payDay);
            }
            if (Container.StayInSyncWithDisc)
            {
                Context.SaveChanges();
            }
            return payDay;
        }
        public PayDay Remove(int payDay)
        {
            Items.Remove(payDay);
            if (!(nonSelectedItems is null))
            {
                int i = 0;
                while (NonSelectedItems.Count>i && NonSelectedItems[i]<payDay)
                {
                    i++;
                }
                NonSelectedItems.Insert(i,payDay);
            }
            if (Items.Count==0)
            {
                Add(PayDay.EndMonth);
            }
            return payDay;
        }
        #endregion
    }
}
