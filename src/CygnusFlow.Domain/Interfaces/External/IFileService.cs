using CygnusFlow.Domain.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.External
{
    public interface IFileService
    {
        Task<Result<string>> SalvarArquivoAsync(byte[] conteudo, string nomeArquivo, string pasta);
        Task<Result<byte[]>> LerArquivoAsync(string caminhoArquivo);
        Task<Result<bool>> ExcluirArquivoAsync(string caminhoArquivo);
        Task<Result<string>> GerarPdfAsync(object dados, string template);
        Task<Result<string>> GerarExcelAsync(object dados, string nomeArquivo);
        Task<Result<List<string>>> ListarArquivosPastaAsync(string pasta);
    }
}
