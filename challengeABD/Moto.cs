using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace challengeABD
{
    public class Moto
    {
        [Required]
        [Description("Identificador único da moto")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Description("Modelo da moto")]
        public string? Modelo { get; set; }

        [Description("Status da moto (true = ativo/disponível, false = inativo/indisponível - ajuste a lógica conforme necessário)")]
        public bool Status { get; set; }

        [Required]
        [MaxLength(7)]
        [Description("Placa da moto")]
        public string? Placa { get; set; }
    }
}