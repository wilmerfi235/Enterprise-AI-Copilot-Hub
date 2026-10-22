using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Domain.ValueObjects;

/// <summary>
/// Los Value Objects son componentes inmutables que se definen exclusivamente por sus atributos y no por una identidad única. 
/// Dos objetos de valor con los mismos valores son iguales.
/// 
/// Identificador específico para fragmentos de texto (chunks) generados a partir de los documentos analizados 
/// para la base de datos vectorial.
/// 
/// Evita el uso indiscriminado de tipos primitivos (Guid) en todo el código, asegurando mediante un registro (record) 
/// inmutable que el identificador del fragmento sea siempre válido.
/// 
/// </summary>
/// <param name="Value"></param>
public record DocumentChunkId(Guid Value)
{
    public static DocumentChunkId Unique() => new(Guid.NewGuid());

    public static DocumentChunkId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("El ID del fragmento vectorial no puede estar vacío.");
        return new DocumentChunkId(value);
    }
}
