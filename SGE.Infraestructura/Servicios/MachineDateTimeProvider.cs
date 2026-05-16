using System;
using SGE.Aplicacion.Fecha;

namespace SGE.Infraestructura.Servicios;

public class MachineDateTimeProvider : IDateTimeProvider
{
    public DateTime ObtenerFechaActual() => DateTime.Now;
}