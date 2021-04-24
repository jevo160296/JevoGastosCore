using EntityCoreBasics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace JevoGastosCore.Model
{
    public class PayDay:INotifyPropertyChanged,IComparable,IHaveId
    {
        private int day;
        static public int EndMonth => 31;

        public int Id { get; set; }
        public int Day
        {
            get => day;
            set
            {
                if (day!=value)
                {
                    day = value;
                    OnPropertyChanged();
                }
            }
        }

        public static implicit operator int(PayDay p)=>p.Day;
        public static implicit operator PayDay(int p)=>new PayDay() { Day = p };

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public override string ToString()
        {
            return Day == EndMonth ? "EoM" : Day.ToString();
        }
        public override int GetHashCode()
        {
            return Day;
        }
        public override bool Equals(object obj)
        {
            return (obj is PayDay) && ((PayDay)obj).Day==this.Day;
        }

        public int CompareTo(object obj)
        {
            return day.CompareTo((int)(PayDay)obj);
        }
    }
}
