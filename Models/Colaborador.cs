using System.ComponentModel.DataAnnotations;

namespace crud_maui.Models
{
    public class Colaborador
    {
        [Key]
        public int IdColaborador { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public decimal Salario { get; set; }
        public DateTime DataContratacao { get; set; }

    }
}