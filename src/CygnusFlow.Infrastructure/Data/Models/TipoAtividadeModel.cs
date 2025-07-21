using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CygnusFlow.Infrastructure.Data.Models
{
    [Table("TipoAtividade")]
    public class TipoAtividadeModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descricao { get; set; }

        [StringLength(7)]
        public string? Cor { get; set; }

        public bool Ativo { get; set; } = true;

        public virtual ICollection<AtividadeModel> Atividades { get; set; } = new List<AtividadeModel>();
    }
    
}
