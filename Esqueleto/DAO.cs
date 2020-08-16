﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JevoGastosCore.Esqueleto
{
    public abstract class DAO<T, DataContext, DataContainer>:INotifyPropertyChanged
        where T : class
        where DataContainer : Container<DataContext>
        where DataContext : DbContext
    {
        protected DataContainer Container;
        protected DataContext Context
        {
            get
            {
                return Container.Context;
            }
        }
        protected ObservableCollection<T> items;
        public abstract ObservableCollection<T> Items { get; }
        public bool ItemsLoaded => !(items is null);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}