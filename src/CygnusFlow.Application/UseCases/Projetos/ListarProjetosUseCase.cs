using AutoMapper;
using CygnusFlow.Application.DTOs.Projeto;
using CygnusFlow.Application.DTOs.Specifications;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CygnusFlow.Application.UseCases.Projetos
{
    public class ListarProjetosUseCase
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IMapper _mapper;

        public ListarProjetosUseCase(IProjetoRepository projetoRepository, IMapper mapper)
        {
            _projetoRepository = projetoRepository;
            _mapper = mapper;
        }

        public async Task<ResultList<ProjetoListDto>> ExecuteAsync(ProjetoFiltroDto filtroDto)
        {
            var filtro = _mapper.Map<ProjetoFiltro>(filtroDto);

            var result = await _projetoRepository.GetByFiltrosAsync(filtro);
            if (!result.IsSuccess)
                return ResultList<ProjetoListDto>.Failure(result.Notifications);

            var dtos = _mapper.Map<List<ProjetoListDto>>(result.Items);
            return ResultList<ProjetoListDto>.Success(dtos, result.TotalCount);
        }
    }

}
