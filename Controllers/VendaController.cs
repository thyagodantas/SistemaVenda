using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SistemaVenda.DAL;
using SistemaVenda.Entidades;
using SistemaVenda.Models;

namespace SistemaVenda.Controllers
{
    public class VendaController : Controller
    {

        protected ApplicationDbContext mContext;

        public VendaController(ApplicationDbContext context)
        {
            mContext = context;
        }

        public IActionResult Index()
        {

            IEnumerable<Venda> lista = mContext.Venda
                .ToList();
            mContext.Dispose();
            return View(lista);
        }

        private IEnumerable<SelectListItem> ListaProdutos()
        {
            return mContext.Produto
                .Select(c => new SelectListItem
                {
                    Value = c.Codigo.ToString(),
                    Text = c.Descricao
                })
                .ToList();
        }

        private IEnumerable<SelectListItem> ListaClientes()
        {
            return mContext.Cliente
                .Select(c => new SelectListItem
                {
                    Value = c.Codigo.ToString(),
                    Text = c.Nome
                })
                .ToList();
        }


        [HttpGet]
        public IActionResult Cadastro(int? id)
        {
            VendaViewModel viewModel = new VendaViewModel();
            viewModel.ListaClientes = ListaClientes();
            viewModel.ListaProdutos = ListaProdutos();

            if (id != null)
            {
                var entidade = mContext.Venda.Where(x => x.Codigo == id).FirstOrDefault();
                viewModel.Codigo = entidade.Codigo;
                viewModel.Data = entidade.Data;
                viewModel.CodigoCliente = entidade.CodigoCliente;
                viewModel.Total = entidade.Total;
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Cadastro(VendaViewModel entidade)
        {
            if (ModelState.IsValid)
            {
                Venda venda = new Venda()
                {
                    Codigo = entidade.Codigo,
                    Data = (DateTime)entidade.Data,
                    CodigoCliente = (int)entidade.CodigoCliente,
                    Total = (int)entidade.Total,
                    Produtos = JsonConvert.DeserializeObject<ICollection<VendaProdutos>>(entidade.JsonProdutos)
                };

                if (venda.Codigo == null)
                {
                    mContext.Venda.Add(venda);
                }
                else
                {
                    mContext.Entry(venda).State = EntityState.Modified;
                }

                mContext.SaveChanges();
            }
            else
            {
                entidade.ListaClientes = ListaClientes();
                entidade.ListaProdutos = ListaProdutos();
                return View(entidade);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var entidade = new Venda() { Codigo = id };
            mContext.Attach(entidade);
            mContext.Remove(entidade);
            mContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("LerValorProduto/{CodigoProduto}")]
        public decimal LerValorProduto(int CodigoProduto)
        {
            return mContext.Produto.Where(x => x.Codigo == CodigoProduto).Select(x => x.Valor).FirstOrDefault();
        }
    }
}

