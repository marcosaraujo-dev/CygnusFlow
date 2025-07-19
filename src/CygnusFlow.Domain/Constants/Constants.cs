namespace CygnusFlow.Domain.Constants
{
    public static class ErrorCodes
    {
        // Códigos gerais
        public const string REQUIRED = "REQUIRED";
        public const string INVALID_FORMAT = "INVALID_FORMAT";
        public const string MIN_LENGTH = "MIN_LENGTH";
        public const string MAX_LENGTH = "MAX_LENGTH";
        public const string NOT_FOUND = "NOT_FOUND";
        public const string ALREADY_EXISTS = "ALREADY_EXISTS";
        public const string UNAUTHORIZED = "UNAUTHORIZED";
        public const string FORBIDDEN = "FORBIDDEN";
        public const string INTERNAL_ERROR = "INTERNAL_ERROR";
        public const string DATABASE_ERROR = "DATABASE_ERROR";

        // Códigos específicos de Projeto
        public const string PROJETO_CODIGO_DUPLICADO = "PROJETO_CODIGO_DUPLICADO";
        public const string PROJETO_DATAS_INVALIDAS = "PROJETO_DATAS_INVALIDAS";
        public const string PROJETO_DATA_FIM_ANTERIOR_INICIO = "PROJETO_DATA_FIM_ANTERIOR_INICIO";

        // Códigos específicos de Atividade
        public const string ATIVIDADE_FORA_PERIODO_PROJETO = "ATIVIDADE_FORA_PERIODO_PROJETO";
        public const string ATIVIDADE_CODIGO_DUPLICADO = "ATIVIDADE_CODIGO_DUPLICADO";

        // Códigos específicos de Usuário
        public const string USUARIO_INATIVO = "USUARIO_INATIVO";
        public const string USUARIO_EMAIL_DUPLICADO = "USUARIO_EMAIL_DUPLICADO";
        public const string USUARIO_SENHA_INVALIDA = "USUARIO_SENHA_INVALIDA";

        // Códigos de autenticação
        public const string LOGIN_CREDENCIAIS_INVALIDAS = "LOGIN_CREDENCIAIS_INVALIDAS";
        public const string TOKEN_EXPIRADO = "TOKEN_EXPIRADO";
        public const string TOKEN_INVALIDO = "TOKEN_INVALIDO";
    }
}
