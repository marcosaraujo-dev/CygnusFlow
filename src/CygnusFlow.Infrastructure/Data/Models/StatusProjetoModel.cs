using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CygnusFlow.Infrastructure.Data.Models
{
    [Table("StatusProjeto")]
    public class StatusProjetoModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; } = true;

        public virtual ICollection<ProjetoModel> Projetos { get; set; } = new List<ProjetoModel>();
        public virtual ICollection<AtividadeModel> Atividades { get; set; } = new List<AtividadeModel>();
    }
    
}
