using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CygnusFlow.Infrastructure.Data.Models
{
    [Table("ProjetoComentario")]
    public class ProjetoComentarioModel
    {
        [Key]
        public int Id { get; set; }

        public int ProjetoId { get; set; }
        public int UsuarioId { get; set; }

        [Required]
        public string Comentario { get; set; } = string.Empty;

        public DateTime DataComentario { get; set; } = DateTime.Now;

        [ForeignKey("ProjetoId")]
        public virtual ProjetoModel Projeto { get; set; } = null!;

        [ForeignKey("UsuarioId")]
        public virtual UsuarioModel Usuario { get; set; } = null!;
    }
    
}
