using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace JevoGastosCore.Esqueleto
{
    public abstract class Vista<DataContext,DataContainer>:INotifyPropertyChanged
        where DataContext:DbContext
        where DataContainer:Container<DataContext>
    {
        protected DataContainer Container;
        protected DataContext Context
        {
            get
            {
                return Container.Context;
            }
        }

        public Vista(DataContainer Container)
        {
            this.Container = Container;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
