using System;

namespace SGE.Aplicacion.ExceptionApp;

public class EntidadNoEncontradaException : Exception
{
    public EntidadNoEncontradaException(string mensaje) : base(mensaje) { }
}