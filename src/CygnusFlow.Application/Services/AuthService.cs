using CygnusFlow.Application.DTOs.Auth;
using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using System;
using System.Threading.Tasks;

namespace CygnusFlow.Application.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<LoginResponseDto>> ValidarLoginAsync(LoginDto loginDto)
        {
            var validationResult = ValidarDadosLogin(loginDto);
            if (!validationResult.IsValid)
                return Result<LoginResponseDto>.Failure(validationResult);

            var usuarioResult = await _usuarioRepository.GetByEmailAsync(loginDto.Email);
            if (!usuarioResult.IsSuccess)
            {
                var notification = new NotificationResult();
                notification.AddError("Login", ErrorMessages.GetMessage(ErrorCodes.LOGIN_CREDENCIAIS_INVALIDAS),
                    ErrorCodes.LOGIN_CREDENCIAIS_INVALIDAS);
                return Result<LoginResponseDto>.Failure(notification);
            }

            var usuario = usuarioResult.Data;

            // Verificar se usuário está ativo
            if (usuario.StatusUsuario.Id != (int)Domain.Enums.StatusUsuario.Ativo)
            {
                var notification = new NotificationResult();
                notification.AddError("Login", ErrorMessages.GetMessage(ErrorCodes.USUARIO_INATIVO),
                    ErrorCodes.USUARIO_INATIVO);
                return Result<LoginResponseDto>.Failure(notification);
            }

            // Verificar senha
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.SenhaHash))
            {
                var notification = new NotificationResult();
                notification.AddError("Login", ErrorMessages.GetMessage(ErrorCodes.LOGIN_CREDENCIAIS_INVALIDAS),
                    ErrorCodes.LOGIN_CREDENCIAIS_INVALIDAS);
                return Result<LoginResponseDto>.Failure(notification);
            }

            // Gerar token (simplificado - em produção usar JWT)
            var token = Guid.NewGuid().ToString();

            var response = new LoginResponseDto
            {
                UsuarioId = usuario.Id,
                NomeUsuario = usuario.Nome,
                Email = usuario.Email,
                TipoUsuario = usuario.TipoUsuarioId.ToString(),
                Token = token,
                ValidoAte = DateTime.Now.AddHours(8)
            };

            return Result<LoginResponseDto>.Success(response);
        }

        private NotificationResult ValidarDadosLogin(LoginDto loginDto)
        {
            var result = new NotificationResult();

            if (string.IsNullOrWhiteSpace(loginDto.Email))
                result.AddError(nameof(loginDto.Email), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "E-mail"), ErrorCodes.REQUIRED);

            if (string.IsNullOrWhiteSpace(loginDto.Senha))
                result.AddError(nameof(loginDto.Senha), ErrorMessages.GetMessage(ErrorCodes.REQUIRED, "Senha"), ErrorCodes.REQUIRED);

            return result;
        }
    }
}
