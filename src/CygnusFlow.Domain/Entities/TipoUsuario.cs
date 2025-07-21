using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CygnusFlow.Domain.Entities
{
    public class TipoUsuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Descrição deve ter no máximo 200 caracteres")]
        public string? Descricao { get; set; }

        public int Nivel { get; set; } // 1 = Viewer, 2 = Dev, 3 = PO, 4 = Admin
        public bool Ativo { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
