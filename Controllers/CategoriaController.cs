using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVenda.DAL;
using SistemaVenda.Entidades;
using SistemaVenda.Models;

namespace SistemaVenda.Controllers
{
    public class CategoriaController : Controller
    {

        protected ApplicationDbContext mContext;

        public CategoriaController(ApplicationDbContext context)
        {
            mContext = context;
        }

        public IActionResult Index()
        {

            IEnumerable<Categoria> listaCategorias = mContext.Categoria.ToList();
            mContext.Dispose();
            return View(listaCategorias);
        }

        [HttpGet]
        public IActionResult Cadastro(int? id)
        {
            CategoriaViewModel viewModel = new CategoriaViewModel();

            if(id != null)
            {
                var entidade = mContext.Categoria.Where(x => x.Codigo == id).FirstOrDefault();
                viewModel.Codigo = entidade.Codigo;
                viewModel.Descricao = entidade.Descricao;
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Cadastro(CategoriaViewModel entidade)
        {
            if (ModelState.IsValid)
            {
                Categoria categoria = new Categoria()
                {
                    Codigo = entidade.Codigo,
                    Descricao = entidade.Descricao
                };

                if (categoria.Codigo == null)
                {
                    mContext.Categoria.Add(categoria);
                }
                else
                {
                    mContext.Entry(categoria).State = EntityState.Modified;
                }

                mContext.SaveChanges();
            }
            else
            {
                return View(entidade);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var entidade = new Categoria() { Codigo = id };
            mContext.Attach(entidade);
            mContext.Remove(entidade);
            mContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
