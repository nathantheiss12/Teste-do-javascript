﻿using Model;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace View.Controllers
{
    [Route("produto/")]
    public class ProdutoController : BaseController
    {
        private ProdutoRepository repository;
        private CategoriaRepository repositoryCategoria;

        public ProdutoController()
        {
            repository = new ProdutoRepository();
            repositoryCategoria = new CategoriaRepository();
        }

        public ActionResult Index()
        {
            List<Categoria> categorias = repositoryCategoria.ObterTodos();

            ViewBag.Categorias = categorias;
            return View();
        }

        //[HttpGet, Route("obtertodospeloidvenda")]
        //public JsonResult ObterTodosPeloIdVenda(int idVenda)
        //{
        //    var produtos = repository.ObterProdutosPeloIdVenda(idVenda);
        //    var resultado = new { data = produtos };
        //    return Json(resultado, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet, Route("obtertodos")]
        public JsonResult ObterTodos()
        {
            var produtos = repository.ObterTodos();
            var resultado = new { data = produtos };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("inserir")]
        public ActionResult Inserir(Produto produto)
        {
            produto.RegistroAtivo = true;
            var id = repository.Inserir(produto);
            var resultado = new { id = id };
            return Json(resultado,
              JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("apagar")]
        public JsonResult Apagar(int id)
        {
            var apagou = repository.Apagar(id);
            var resultado = new { status = apagou };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("update")]
        public JsonResult Update(Produto produto)
        {
            var alterou = repository.Alterar(produto);
            var resultado = new { status = alterou };
            return Json(resultado);
        }

        [HttpGet, Route("obterpeloid")]
        public ActionResult ObterPeloId(int id)
        {
            var produto = repository.ObterPeloId(id);
            if (produto == null)
                return HttpNotFound();

            return Json(produto, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("obtertodosselect2")]
        public JsonResult ObterTodosSelect2(string term)
        {
            var produtos = repository.ObterTodos();

            List<object> produtosSelect2 = new List<object>();
            foreach (Produto produto in produtos)
            {
                produtosSelect2.Add(new
                {
                    id = produto.Id,
                    nome = produto.Nome,
                    categoria = produto.Categoria,
                    valor = produto.Valor,
                    registro = produto.RegistroAtivo

                });
            }
            var resultado = new { results = produtosSelect2 };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObterTodosPeloIdInventario(int idInventario)
        {
            return Json(new { data = repository.ObterTodosPeloIdInventario(idInventario) }, JsonRequestBehavior.AllowGet);
        }
    }
}