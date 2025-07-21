using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CygnusFlow.Infrastructure.Data.Models
{
    [Table("TipoUsuario")]
    public class TipoUsuarioModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descricao { get; set; }

        public int Nivel { get; set; }
        public bool Ativo { get; set; } = true;

        public virtual ICollection<UsuarioModel> Usuarios { get; set; } = new List<UsuarioModel>();
    }
    
}
