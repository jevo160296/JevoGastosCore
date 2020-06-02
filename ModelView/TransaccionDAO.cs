using JevoGastosCore.Context;
using JevoGastosCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JevoGastosCore.ModelView
{
    public class TransaccionDAO:DAO<Transaccion, GastosContext, GastosContainer>
    {
        public TransaccionDAO(GastosContainer gastosContainer)
        {
            this.Container = gastosContainer;
        }

        public Transaccion Add(
            TipoTransaccion tipoTransaccion,
            Etiqueta origen,
            Etiqueta destino,
            double valor,
            DateTime fecha,
            string descripcion)
        {
            Transaccion newTransaccion=new Transaccion() 
            {
                TipoTransaccion=tipoTransaccion,
                Origen=origen,
                Destino=destino,
                Valor=valor,
                Fecha=fecha,
                Descripcion=descripcion,
            };
            Add(newTransaccion);
            return newTransaccion;
        }
        public Transaccion Add(Transaccion transaccion)
        {
            Transaccion newTransaccion= LazyAdd(transaccion);
            Context.SaveChanges();
            return newTransaccion;
        }
        public Transaccion LazyAdd(Transaccion transaccion)
        {
            this.Context.Transacciones.Add(transaccion);
            return transaccion;
        }
    }
}
