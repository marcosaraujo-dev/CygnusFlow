namespace CygnusFlow.Domain.DTOs
{
    public class ConfiguracaoDto
    {
        public string Chave { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public bool IsReadOnly { get; set; }
        public string? ValorPadrao { get; set; }
        public string? Categoria { get; set; }
    }
}
