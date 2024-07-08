
using FI.AtividadeEntrevista.BLL;
using FI.WebAtividadeEntrevista.Models;
using WebAtividadeEntrevista.Models;

namespace FI.WebAtividadeEntrevista.Common
{
    public sealed class Validator
    {
        public static bool VerifyExistence(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            return bo.VerificarExistencia(model.CPF);
        }
        
        public static bool VerifyExistenceBeneficiary(BeneficiarioModel model)
        {
            BoBeneficiario bo = new BoBeneficiario();

            return bo.VerificarExistencia(model.CPF);
        }
    }
}