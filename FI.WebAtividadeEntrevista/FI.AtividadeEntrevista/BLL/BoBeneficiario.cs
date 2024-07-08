using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.Beneficiarios.DaoBeneficiario dao = new DAL.Beneficiarios.DaoBeneficiario();
            return dao.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.Beneficiarios.DaoBeneficiario dao = new DAL.Beneficiarios.DaoBeneficiario();
            dao.Alterar(beneficiario);
        }

        /// <summary>
        /// Excluir o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.Beneficiarios.DaoBeneficiario dao = new DAL.Beneficiarios.DaoBeneficiario();
            dao.Excluir(id);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.Beneficiarios.DaoBeneficiario dao = new DAL.Beneficiarios.DaoBeneficiario();
            return dao.VerificarExistencia(CPF);
        }
        
        /// <summary>
        /// Consulta beneficiários
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public List<Beneficiario> Consultar(long IdCliente)
        {
            DAL.Beneficiarios.DaoBeneficiario dao = new DAL.Beneficiarios.DaoBeneficiario();
            return dao.Listar(IdCliente);
        }
    }
}
