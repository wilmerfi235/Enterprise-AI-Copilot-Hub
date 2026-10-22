using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Domain.Exceptions;

/// <summary>
/// Excepciones personalizadas orientadas a reportar violaciones directas de las reglas del modelo de dominio.
/// 
/// Excepción base general para cualquier regla de negocio quebrantada dentro del dominio.
/// 
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
