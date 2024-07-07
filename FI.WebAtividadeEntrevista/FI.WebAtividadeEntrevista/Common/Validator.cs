
using FI.AtividadeEntrevista.BLL;
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
    }
}