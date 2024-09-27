using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVenda.DAL;
using SistemaVenda.Entidades;
using SistemaVenda.Models;

namespace SistemaVenda.Controllers
{
    public class ClienteController : Controller
    {

        protected ApplicationDbContext mContext;

        public ClienteController(ApplicationDbContext context)
        {
            mContext = context;
        }

        public IActionResult Index()
        {

            IEnumerable<Cliente> listaCategorias = mContext.Cliente.ToList();
            mContext.Dispose();
            return View(listaCategorias);
        }

        [HttpGet]
        public IActionResult Cadastro(int? id)
        {
            ClienteViewModel viewModel = new ClienteViewModel();

            if(id != null)
            {
                var entidade = mContext.Cliente.Where(x => x.Codigo == id).FirstOrDefault();
                viewModel.Codigo = entidade.Codigo;
                viewModel.Nome = entidade.Nome;
                viewModel.CNPJ_CPF = entidade.CNPJ_CPF;
                viewModel.Email = entidade.Email;
                viewModel.Celular = entidade.Celular;
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Cadastro(ClienteViewModel entidade)
        {
            if (ModelState.IsValid)
            {
                Cliente cliente = new Cliente()
                {
                    Codigo = entidade.Codigo,
                    Nome = entidade.Nome,
                    CNPJ_CPF = entidade.CNPJ_CPF,
                    Email = entidade.Email,
                    Celular = entidade.Celular
                };

                if (cliente.Codigo == null)
                {
                    mContext.Cliente.Add(cliente);
                }
                else
                {
                    mContext.Entry(cliente).State = EntityState.Modified;
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
            var entidade = new Cliente() { Codigo = id };
            mContext.Attach(entidade);
            mContext.Remove(entidade);
            mContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
