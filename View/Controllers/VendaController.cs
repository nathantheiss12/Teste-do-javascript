﻿using Model;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace View.Controllers
{
    [Route("venda/")]
    public class VendaController : BaseController
    {
        VendaRepository repository;
        private VendedorRepository repositoryVendedor;
        private ClienteRepository repositoryCliente;

        public VendaController()
        {
            repository = new VendaRepository();
            repositoryVendedor = new VendedorRepository();
            repositoryCliente = new ClienteRepository();
        }

        [HttpGet, Route("obtertodos")]
        public JsonResult ObterTodos()
        {
            var vendas = repository.ObterTodos();
            var resultado = new { data = vendas };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("cadastro")]
        public ActionResult Cadastro(Venda venda)
        {
            int id = repository.Inserir(venda);
            return RedirectToAction("Editar", new { id = id });
        }

        [HttpPost, Route("update")]
        public JsonResult Update(Venda venda)
        {
            var alterou = repository.Alterar(venda);
            var resultado = new { status = alterou };
            return Json(resultado,
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("apagar")]
        public JsonResult Apagar(int id)
        {
            var apagou = repository.Apagar(id);
            var resultado = new { status = apagou };
            return Json(resultado,
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("obterpeloid")]
        public JsonResult ObterPeloId(int id)
        {
            return Json(repository.ObterPeloId(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            List<Vendedor> vendedores = repositoryVendedor.ObterTodos();
            ViewBag.Vendedores = vendedores;
            List<Cliente> clientes = repositoryCliente.ObterTodos();
            ViewBag.Clientes = clientes;
            return View();
        }

        public ActionResult Cadastro()
        {
            VendedorRepository vendedorRepository = new VendedorRepository();
            List<Vendedor> vendedores = vendedorRepository.ObterTodos();
            ViewBag.Vendedores = vendedores;

            ClienteRepository clienteRepository = new ClienteRepository();
            List<Cliente> clientes = clienteRepository.ObterTodos();
            ViewBag.Clientes = clientes;


            return View();
        }

        [HttpPost, Route("editar")]
        public JsonResult Editar(Venda venda)
        {
            var alterou = repository.Alterar(venda);
            var resultado = new { status = alterou };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            var venda = repository.ObterPeloId(id);
            if (venda == null)
                return RedirectToAction("Index");

            ViewBag.Venda = venda;
            return View();
        }
    }
}