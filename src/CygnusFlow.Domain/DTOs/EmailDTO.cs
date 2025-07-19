using System;
using System.Collections.Generic;

namespace CygnusFlow.Domain.DTOs
{

    public class EmailDto
    {
        public string Destinatario { get; set; } = string.Empty;
        public string Assunto { get; set; } = string.Empty;
        public string Corpo { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = false;
        public List<string>? Anexos { get; set; }
        public DateTime? DataEnvio { get; set; }
    }
}
