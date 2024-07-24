using Microsoft.AspNetCore.Http; // Biblioteca necessária para manipular arquivos e dados de requisições HTTP.
using System.IO; // Biblioteca necessária para manipular operações de entrada e saída de arquivos.
using System; // Biblioteca necessária para gerar identificadores únicos (GUIDs).
using System.Threading.Tasks; // Biblioteca necessária para trabalhar com operações assíncronas.

namespace SuperShop.Helpers 
{
    // Implementa a interface IImageHelper, ou seja, fornece a implementação dos métodos definidos na interface.
    public class ImageHelper : IImageHelper
    {
        // Implementação do método assíncrono para fazer o upload de uma imagem.
        public async Task<string> UploadImageAsync(IFormFile imageFile, string folder)
        {
            // Gera uma chave única aleatória (GUID) e converte para string.
            // Isto garante que o nome do arquivo será único.
            string guid = Guid.NewGuid().ToString();
            // Define o nome do arquivo como o GUID seguido pela extensão .jpg.
            string file = $"{guid}.jpg";

            // Combina o caminho atual do diretório, a pasta wwwroot\images e a pasta fornecida com o nome do arquivo.
            string path = Path.Combine(
                   Directory.GetCurrentDirectory(), // Obtém o diretório atual da aplicação.
                   $"wwwroot\\images\\{folder}", // Combina com a pasta wwwroot\images e a pasta específica fornecida.
                   file); // Combina com o nome do arquivo gerado.

            // Cria um FileStream para escrever o arquivo no disco.
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                // Copia o conteúdo do arquivo enviado para o FileStream.
                await imageFile.CopyToAsync(stream);
            }

            // Retorna o caminho relativo do arquivo que pode ser armazenado na base de dados.
            return $"~/images/{folder}/{file}";
        }
    }
}
