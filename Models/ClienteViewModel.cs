using System.ComponentModel.DataAnnotations;

namespace SistemaVenda.Models
{
    public class ClienteViewModel
    {
        public int? Codigo { get; set; }
        [Required(ErrorMessage = "Informe o nome do cliente")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o CNPJ/CPF do cliente")]
        public string CNPJ_CPF { get; set; }

        [Required(ErrorMessage = "Informe o Email do cliente")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe o Celular do cliente")]
        public string Celular { get; set; }
    }
}
