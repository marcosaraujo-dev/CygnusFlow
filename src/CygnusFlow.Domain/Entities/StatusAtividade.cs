using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Entities
{
    public class StatusAtividade
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Descrição deve ter no máximo 200 caracteres")]
        public string? Descricao { get; set; }

        [StringLength(7, ErrorMessage = "Cor deve ter no máximo 7 caracteres (#FFFFFF)")]
        public string? Cor { get; set; }

        public int Ordem { get; set; }
        public bool Ativo { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
    }
}
