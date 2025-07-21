using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CygnusFlow.Infrastructure.Data.Models
{
    [Table("Usuario")]
    public class UsuarioModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string SenhaHash { get; set; } = string.Empty;

        public int? EquipeId { get; set; }
        public int TipoUsuarioId { get; set; }
        public int StatusUsuarioId { get; set; }
        public bool BloqueadoPorRedefinicao { get; set; } = false;
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Propriedades de navegação EF
        [ForeignKey("EquipeId")]
        public virtual EquipeModel? Equipe { get; set; }

        [ForeignKey("TipoUsuarioId")]
        public virtual TipoUsuarioModel TipoUsuario { get; set; } = null!;

        [ForeignKey("StatusUsuarioId")]
        public virtual StatusUsuarioModel StatusUsuario { get; set; } = null!;

        // Coleções relacionadas
        public virtual ICollection<AtividadeModel> AtividadesResponsavel { get; set; } = new List<AtividadeModel>();
        public virtual ICollection<ProjetoComentarioModel> ProjetoComentarios { get; set; } = new List<ProjetoComentarioModel>();
        public virtual ICollection<AtividadeComentarioModel> AtividadeComentarios { get; set; } = new List<AtividadeComentarioModel>();
    }
    
}
