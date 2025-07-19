using System.Collections.Generic;

namespace CygnusFlow.Domain.Constants
{
    public static class ErrorMessages
    {
        private static readonly Dictionary<string, string> Messages = new()
        {
            { ErrorCodes.REQUIRED, "O campo {0} é obrigatório." },
            { ErrorCodes.INVALID_FORMAT, "O campo {0} possui formato inválido." },
            { ErrorCodes.MIN_LENGTH, "O campo {0} deve ter pelo menos {1} caracteres." },
            { ErrorCodes.MAX_LENGTH, "O campo {0} deve ter no máximo {1} caracteres." },
            { ErrorCodes.NOT_FOUND, "{0} não foi encontrado(a)." },
            { ErrorCodes.ALREADY_EXISTS, "{0} já existe." },
            { ErrorCodes.UNAUTHORIZED, "Acesso não autorizado." },
            { ErrorCodes.FORBIDDEN, "Operação não permitida." },

            { ErrorCodes.PROJETO_CODIGO_DUPLICADO, "Já existe um projeto com este código." },
            { ErrorCodes.PROJETO_DATAS_INVALIDAS, "As datas do projeto são inválidas." },
            { ErrorCodes.PROJETO_DATA_FIM_ANTERIOR_INICIO, "A data de fim deve ser posterior à data de início." },

            { ErrorCodes.ATIVIDADE_FORA_PERIODO_PROJETO, "A atividade está fora do período do projeto." },
            { ErrorCodes.ATIVIDADE_CODIGO_DUPLICADO, "Já existe uma atividade com este código." },

            { ErrorCodes.USUARIO_INATIVO, "Usuário inativo." },
            { ErrorCodes.USUARIO_EMAIL_DUPLICADO, "Já existe um usuário com este e-mail." },
            { ErrorCodes.USUARIO_SENHA_INVALIDA, "Senha inválida." },

            { ErrorCodes.LOGIN_CREDENCIAIS_INVALIDAS, "E-mail ou senha inválidos." },
            { ErrorCodes.TOKEN_EXPIRADO, "Token expirado." },
            { ErrorCodes.TOKEN_INVALIDO, "Token inválido." }
        };

        public static string GetMessage(string code, params object[] args)
        {
            if (Messages.TryGetValue(code, out var message))
            {
                return string.Format(message, args);
            }
            return $"Erro desconhecido: {code}";
        }
    }
}
