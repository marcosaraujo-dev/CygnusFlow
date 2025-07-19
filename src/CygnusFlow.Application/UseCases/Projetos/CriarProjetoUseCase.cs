using AutoMapper;
using CygnusFlow.Application.DTOs.Projeto;
using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.ValueObjects;
using System.Threading.Tasks;

namespace CygnusFlow.Application.UseCases.Projetos
{
    public class CriarProjetoUseCase
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IMapper _mapper;

        public CriarProjetoUseCase(IProjetoRepository projetoRepository, IMapper mapper)
        {
            _projetoRepository = projetoRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProjetoResponseDto>> ExecuteAsync(CreateProjetoDto dto)
        {
            // 1. Mapear DTO para Entity
            var projeto = _mapper.Map<Projeto>(dto);

            // 2. Gerar próximo código
            var proximoNumeroResult = await _projetoRepository.GetProximoNumeroAsync();
            if (!proximoNumeroResult.IsSuccess)
                return Result<ProjetoResponseDto>.Failure(proximoNumeroResult.Notifications);

            var codigoResult = CodigoProjeto.GerarProximoCodigo(proximoNumeroResult.Data);
            if (!codigoResult.IsSuccess)
                return Result<ProjetoResponseDto>.Failure(codigoResult.Notifications);

            projeto.Codigo = codigoResult.Data.Value;

            // 3. Validar entidade
            var validationResult = projeto.Validate();
            if (!validationResult.IsValid)
                return Result<ProjetoResponseDto>.Failure(validationResult);

            // 4. Verificar se código já existe
            var existeCodigoResult = await _projetoRepository.ExisteCodigoAsync(projeto.Codigo);
            if (!existeCodigoResult.IsSuccess)
                return Result<ProjetoResponseDto>.Failure(existeCodigoResult.Notifications);

            if (existeCodigoResult.Data)
            {
                var notification = new NotificationResult();
                notification.AddError(nameof(projeto.Codigo), "Código já existe", "CODIGO_DUPLICADO");
                return Result<ProjetoResponseDto>.Failure(notification);
            }

            // 5. Salvar no repositório
            var saveResult = await _projetoRepository.CreateAsync(projeto);
            if (!saveResult.IsSuccess)
                return Result<ProjetoResponseDto>.Failure(saveResult.Notifications);

            // 6. Retornar DTO de resposta
            var responseDto = _mapper.Map<ProjetoResponseDto>(saveResult.Data);
            return Result<ProjetoResponseDto>.Success(responseDto);
        }
    }
}
