using System;

namespace CygnusFlow.Application.DTOs.Reports
{
    public class GanttHtmlDto
    {
        public string ConteudoHtml { get; set; } = string.Empty;
        public string NomeArquivo { get; set; } = string.Empty;
        public DateTime DataGeracao { get; set; } = DateTime.Now;
    }
}
