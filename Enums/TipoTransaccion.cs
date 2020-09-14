namespace JevoGastosCore.Enums
{
    public enum TipoTransaccion
    {
        Entrada= 2 ^ (4 + TipoEtiqueta.Ingreso)+2^TipoEtiqueta.Cuenta,
        Movimiento= 2 ^ (4 + TipoEtiqueta.Cuenta) + 2 ^ TipoEtiqueta.Cuenta,
        Salida= 2 ^ (4 + TipoEtiqueta.Cuenta) + 2 ^ TipoEtiqueta.Gasto,
        Prestamo= 2 ^ (4 + TipoEtiqueta.Credito) + 2 ^ TipoEtiqueta.Cuenta,
        Pago= 2 ^ (4 + TipoEtiqueta.Cuenta) + 2 ^ TipoEtiqueta.Credito
    }
}
