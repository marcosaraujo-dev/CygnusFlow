using AutoMapper;
using CygnusFlow.Application.DTOs.Projeto;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using System.Threading.Tasks;

namespace CygnusFlow.Application.UseCases.Projetos
{
    public class EditarProjetoUseCase
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IMapper _mapper;

        public EditarProjetoUseCase(IProjetoRepository projetoRepository, IMapper mapper)
        {
            _projetoRepository = projetoRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProjetoResponseDto>> ExecuteAsync(UpdateProjetoDto dto)
        {
            // 1. Buscar projeto existente
            var projetoExistenteResult = await _projetoRepository.GetByIdAsync(dto.Id);
            if (!projetoExistenteResult.IsSuccess)
                return Result<ProjetoResponseDto>.Failure(projetoExistenteResult.Notifications);

            var projeto = projetoExistenteResult.Data;

            // 2. Atualizar campos
            _mapper.Map(dto, projeto);

            // 3. Validar entidade
            var validationResult = projeto.Validate();
            if (!validationResult.IsValid)
                return Result<ProjetoResponseDto>.Failure(validationResult);

            // 4. Salvar alterações
            var saveResult = await _projetoRepository.UpdateAsync(projeto);
            if (!saveResult.IsSuccess)
                return Result<ProjetoResponseDto>.Failure(saveResult.Notifications);

            // 5. Retornar DTO de resposta
            var responseDto = _mapper.Map<ProjetoResponseDto>(saveResult.Data);
            return Result<ProjetoResponseDto>.Success(responseDto);
        }
    }
}
