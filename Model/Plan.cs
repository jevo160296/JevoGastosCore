using JevoGastosCore.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System;
using JevoGastosCore.Clases;
using System.Collections.Generic;
using EntityCoreBasics;

namespace JevoGastosCore.Model
{
    public class Plan : INotifyPropertyChanged,IHaveId
    {
        private int etiquetaId;
        private Etiqueta etiqueta;
        private TipoPlan tipo;
        private double meta;
        private bool esmesfijo;
        private double metaMensual= double.NaN;
        private double metaDiaria = double.NaN;
        private double valorActual = double.NaN;
        private double falta = double.NaN;
        private Dictionary<PlanProperty, List<PlanProperty>> updateTracker = new Dictionary<PlanProperty, List<PlanProperty>>();

        public int Id { get; set; }

        public int EtiquetaId
        {
            get => etiquetaId;
            set 
            {
                if (etiquetaId!=value)
                {
                    etiquetaId = value;
                    OnPropertyChanged();
                }
            }
        }
        public Etiqueta Etiqueta
        {
            get => etiqueta;
            set 
            {
                if (etiqueta!=value)
                {
                    etiqueta = value;
                    OnPropertyChanged();
                }
                if(!(etiqueta is null))
                {
                    etiqueta.PropertyChanged -= Etiqueta_PropertyChanged;
                    etiqueta.PropertyChanged += Etiqueta_PropertyChanged;
                }
            }
        }

        private void Etiqueta_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public TipoPlan Tipo
        {
            get => tipo;
            set 
            {
                if (tipo!=value)
                {
                    tipo = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Meta
        {
            get => meta;
            set 
            {
                if (meta!=value)
                {
                    meta = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool EsMesFijo
        {
            get => esmesfijo;
            set
            {
                if (esmesfijo!=value)
                {
                    esmesfijo = value;
                    OnPropertyChanged();
                }
            }
        }

        public double MetaMensual
        {
            get
            {
                if (double.IsNaN(metaMensual))
                {
                    OnVoidPropertyRequested();
                }
                return metaMensual;
            }
            set
            {
                if (metaMensual!=value)
                {
                    metaMensual = value;
                    OnPropertyChanged();
                }
            }
        }
        public double MetaDiaria
        {
            get
            {
                if (double.IsNaN(metaDiaria))
                {
                    OnVoidPropertyRequested();
                }
                return metaDiaria;
            }
            set
            {
                if (metaDiaria!=value)
                {
                    metaDiaria = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ValorActual
        {
            get
            {
                if (double.IsNaN(valorActual))
                {
                    OnVoidPropertyRequested();
                }
                return valorActual;
            }   
            set 
            {
                if (valorActual!=value)
                {
                    valorActual = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Falta
        {
            get 
            {
                if (double.IsNaN(falta))
                {
                    OnVoidPropertyRequested();
                }
                return falta;
            }
            set 
            {
                if (falta!=value)
                {
                    falta = value;
                    OnPropertyChanged();
                }
            }
        }
        public Dictionary<PlanProperty, List<PlanProperty>> UpdateTracker { get => updateTracker; set => updateTracker = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void VoidPropertyRequestedEventHandler(Plan sender, PlanProperty e);
        public event VoidPropertyRequestedEventHandler VoidPropertyRequested;

        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        protected void OnVoidPropertyRequested([CallerMemberName]string name = null)
        {

            PlanProperty property;
            bool parsed = Enum.TryParse<PlanProperty>(name, out property);
            if (!parsed)
            {
                property = PlanProperty.NotFound;
            }
            VoidPropertyRequested?.Invoke(this, property);
        }
    }
}
