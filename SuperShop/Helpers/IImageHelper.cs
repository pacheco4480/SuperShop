using Microsoft.AspNetCore.Http; // Usado para trabalhar com arquivos e requisições HTTP no ASP.NET Core.
using System.Threading.Tasks; // Necessário para trabalhar com operações assíncronas.

namespace SuperShop.Helpers 
{
    // Define uma interface chamada IImageHelper.
    // Uma interface é como um contrato que especifica métodos que uma classe deve implementar.
    public interface IImageHelper
    {
        // Declara um método assíncrono que será usado para fazer upload de uma imagem.
        // IFormFile representa o arquivo de imagem enviado pelo cliente.
        // A string folder indica a pasta onde a imagem deve ser guardada.
        // O método retorna uma Task<string>, o que significa que é assíncrono e retornará uma string.
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
