using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Entities
{
    [Table("UsuarioRedefinicaoSenha")]
    public class UsuarioRedefinicaoSenha
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

        // Propriedade de navegação
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        // Métodos auxiliares
        public bool EstaExpirado()
        {
            return DateTime.Now > ExpiraEm;
        }

        public bool PodeSerUtilizado()
        {
            return !Utilizado && !EstaExpirado();
        }

        public void MarcarComoUtilizado()
        {
            Utilizado = true;
            DataUtilizacao = DateTime.Now;
        }

        public void DefinirExpiracao(int horasParaExpirar = 24)
        {
            ExpiraEm = DataGeracao.AddHours(horasParaExpirar);
        }
    }
}
