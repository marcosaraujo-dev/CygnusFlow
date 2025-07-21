using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CygnusFlow.Infrastructure.Data.Models
{
    [Table("UsuarioRedefinicaoSenha")]
    public class UsuarioRedefinicaoSenhaModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(10)]
        public string Codigo { get; set; } = string.Empty;

        public bool GeradoPorAdmin { get; set; } = false;
        public bool Utilizado { get; set; } = false;

        [Required]
        public DateTime ExpiraEm { get; set; }

        public DateTime DataGeracao { get; set; } = DateTime.Now;
        public DateTime? DataUtilizacao { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual UsuarioModel Usuario { get; set; } = null!;
    }
    
}
