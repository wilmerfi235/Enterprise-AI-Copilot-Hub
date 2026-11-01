using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.DTOs;

/// <summary>
/// 
/// Para transferir datos hacia la capa de presentación (API) sin exponer la entidad
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="FilePath"></param>
/// <param name="FileSize"></param>
/// <param name="CreatedAt"></param>
/// <param name="IsProcessed"></param>
public record DocumentDto(Guid Id, string Title, string FilePath, long FileSize, DateTime CreatedAt, bool IsProcessed);
