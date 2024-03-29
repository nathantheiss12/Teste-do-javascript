﻿using Model;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace View.Controllers
{
    [Route("usuario/")]
    public class UsuarioController : BaseController
    {
        public ActionResult Change(String lang)
        {
            if (lang != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                HttpCookie cookie = new HttpCookie("Language");
                cookie.Value = lang;
                Response.Cookies.Add(cookie);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        private UsuarioRepository repository;
        private PaisRepository repositoryPais;
        private EstadoRepository repositoryEstado;
        private CidadeRepository repositoryCidade;

        public UsuarioController()
        {
            repository = new UsuarioRepository();
            repositoryPais = new PaisRepository();
            repositoryEstado = new EstadoRepository();
            repositoryCidade = new CidadeRepository();
        }

        [HttpGet]
        public ActionResult Perfil()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Config()
        {
            var usuario =(Usuario)  Session["Usuario"];
            ViewBag.Usuario = usuario;

            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("obtertodos")]
        public JsonResult ObterTodos()
        {
            var usuarios = repository.ObterTodos();
            var resultado = new { data = usuarios };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("inserir")]
        public JsonResult Inserir(Usuario usuario)
        {
            usuario.RegistroAtivo = true;
            var id = repository.Inserir(usuario);
            var resultado = new { id = id };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("apagar")]
        public JsonResult Apagar(int id)
        {
            var apagou = repository.Apagar(id);
            var resultado = new { status = apagou };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("update")]
        public JsonResult Update(Usuario usuario)
        {
            var alterou = repository.Alterar(usuario);
            var resultado = new { status = alterou };
            return Json(resultado);
        }

        [HttpPost, Route("updatepass")]
        public ActionResult UpdatePass(Usuario usuario)
        {
            var alterou = repository.AlterarSenha(usuario);

            if (usuario.Senha == null)
            {
                return null;
            }

            return RedirectToAction("Config");
        }

        [HttpGet, Route("obterpeloid")]
        public JsonResult ObterPeloId(int id)
        {
            return Json(repository.ObterPeloId(id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("obtertodosselect2")]
        public JsonResult ObterTodosSelect2(string term)
        {
            var usuarios = repository.ObterTodos();

            List<object> usuariosSelect2 = new List<object>();
            foreach (Usuario usuario in usuarios)
            {
                usuariosSelect2.Add(new
                {
                    id = usuario.Id,
                    nome = usuario.Nome,
                    senha = usuario.Senha,
                    registro = usuario.RegistroAtivo

                });
            }
            var resultado = new { results = usuariosSelect2 };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Upload()
        {

            HttpPostedFileBase arquivo = Request.Files[0];

            //Suas validações ......

            //Salva o arquivo
            if (arquivo.ContentLength > 0)
            {
                var uploadPath = Server.MapPath("~/Content/uploads");
                var nomeImagem = Path.GetFileName(arquivo.FileName);

                string caminhoArquivo = Path.Combine(@uploadPath, nomeImagem);

                arquivo.SaveAs(caminhoArquivo);

                var usuarioLogado = (Usuario)Session["Usuario"];

                Usuario usuario = repository.ObterPeloId(usuarioLogado.Id);
                usuario.UrlImagem = nomeImagem;
                repository.Alterar(usuario);
                Session["Usuario"] = usuario;
            }

            ViewData["Message"] = String.Format(" arquivo(s) salvo(s) com sucesso.");
            return RedirectToAction("config");
        }
    }
}