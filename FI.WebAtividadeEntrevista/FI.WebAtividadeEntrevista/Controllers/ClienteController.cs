using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Text.RegularExpressions;
using FI.WebAtividadeEntrevista.Common;
using FI.WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            List<string> errors = new List<string>();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            model.CPF = Regex.Replace(model.CPF,@"[^\d]", "");

            if (Validator.VerifyExistence(model))
            {
                errors.Add("O CPF informado já está cadastrado.");
            }
            else if (!this.ModelState.IsValid)
            {
                errors = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();
            }
            else
            {
                
                model.Id = bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                if (model.Beneficiarios != null)
                {
                    foreach (BeneficiarioModel beneficiario in model.Beneficiarios)
                    {
                        boBeneficiario.Incluir(new Beneficiario()
                        {
                            CPF = beneficiario.CPF,
                            Nome = beneficiario.Nome,
                            IdCliente = model.Id
                        });
                    }
                }

                return Json("Cadastro efetuado com sucesso");
            }

            Response.StatusCode = 400;
            return Json(string.Join(Environment.NewLine, errors));
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            model.CPF = Regex.Replace(model.CPF, @"[^\d]", "");
            
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                if (model.Beneficiarios != null)
                {
                    foreach (BeneficiarioModel beneficiario in model.Beneficiarios)
                    {
                        beneficiario.CPF = Regex.Replace(beneficiario.CPF, @"[^\d]", "");
                        if (beneficiario.Id == 0)
                        {
                            boBeneficiario.Incluir(new Beneficiario()
                            {
                                CPF = beneficiario.CPF,
                                Nome = beneficiario.Nome,
                                IdCliente = model.Id
                            });
                        }
                        else if (beneficiario.Id != null && beneficiario.Action == 0)
                        {
                            boBeneficiario.Alterar(new Beneficiario()
                            {
                                Id = beneficiario.Id,
                                CPF = beneficiario.CPF,
                                Nome = beneficiario.Nome,
                                IdCliente = model.Id
                            });
                        }
                        else if (beneficiario.Id != null && beneficiario.Action == 1)
                            boBeneficiario.Excluir(beneficiario.Id);
                    }
                }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();
            Cliente cliente = boCliente.Consultar(id);
            cliente.Beneficiarios = boBeneficiario.Consultar(id);
            
            Models.ClienteModel model = null;

            List<BeneficiarioModel> beneficiarioList = new List<BeneficiarioModel>();

            if (cliente.Beneficiarios.Count > 0)
            {
                foreach (Beneficiario beneficiarioItem in cliente.Beneficiarios)
                {
                    beneficiarioList.Add(new BeneficiarioModel
                    {
                        Id = beneficiarioItem.Id,
                        ClienteId = beneficiarioItem.IdCliente,
                        CPF = beneficiarioItem.CPF,
                        Nome = beneficiarioItem.Nome
                    });
                }
            }

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = beneficiarioList
                };

            
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}