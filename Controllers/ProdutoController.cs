using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaVenda.DAL;
using SistemaVenda.Entidades;
using SistemaVenda.Models;

namespace SistemaVenda.Controllers
{
    public class ProdutoController : Controller
    {

        protected ApplicationDbContext mContext;

        public ProdutoController(ApplicationDbContext context)
        {
            mContext = context;
        }

        public IActionResult Index()
        {

            IEnumerable<Produto> listaCategorias = mContext.Produto
                .Include(x => x.Categoria)
                .ToList();
            mContext.Dispose();
            return View(listaCategorias);
        }

        private IEnumerable<SelectListItem> ListaCategoria()
        {
            return mContext.Categoria
                .Select(c => new SelectListItem
                {
                    Value = c.Codigo.ToString(),
                    Text = c.Descricao
                })
                .ToList();
        }


        [HttpGet]
        public IActionResult Cadastro(int? id)
        {
            ProdutoViewModel viewModel = new ProdutoViewModel
            {
                ListaCategorias = ListaCategoria()
            };

            if (id != null)
            {
                var entidade = mContext.Produto.Where(x => x.Codigo == id).FirstOrDefault();
                viewModel.Codigo = entidade.Codigo;
                viewModel.Descricao = entidade.Descricao;
                viewModel.Quantidade = entidade.Quantidade;
                viewModel.Valor = entidade.Valor;
                viewModel.CodigoCategoria = entidade.CodigoCategoria;
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Cadastro(ProdutoViewModel entidade)
        {
            ModelState.Remove(nameof(entidade.ListaCategorias));
            if (ModelState.IsValid)
            {
                Produto produto = new Produto()
                {
                    Codigo = entidade.Codigo,
                    Descricao = entidade.Descricao,
                    Quantidade = entidade.Quantidade,
                    Valor = (decimal)entidade.Valor,
                    CodigoCategoria = (int)entidade.CodigoCategoria
                };

                if (produto.Codigo == null)
                {
                    mContext.Produto.Add(produto);
                }
                else
                {
                    mContext.Entry(produto).State = EntityState.Modified;
                }

                mContext.SaveChanges();
            }
            else
            {
                entidade.ListaCategorias = ListaCategoria();
                return View(entidade);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var entidade = new Produto() { Codigo = id };
            mContext.Attach(entidade);
            mContext.Remove(entidade);
            mContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
