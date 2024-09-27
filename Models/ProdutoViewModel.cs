using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SistemaVenda.Models
{
    public class ProdutoViewModel
    {
        public int? Codigo { get; set; }
        [Required(ErrorMessage = "Informe a descrição do produto")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Informe a quantidade em estoque")]
        public decimal Quantidade { get; set; }

        [Required(ErrorMessage = "Informe o valor do produto")]
        [Range(0.1, Double.PositiveInfinity)]
        public decimal? Valor { get; set; }

        [Required(ErrorMessage = "Informe a categoria do produto")]
        public int? CodigoCategoria { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> ListaCategorias { get; set; }
    }
}
