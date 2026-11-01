using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.DTOs;

/// <summary>
/// 
/// 
/// </summary>
/// <param name="Sender"></param>
/// <param name="Content"></param>
/// <param name="Timestamp"></param>
public record AgentMessageDto(string Sender, string Content, DateTime Timestamp);
