using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IBlobHelper
        //Primeira forma de fazer o Upload da imagem, recebe um ficheiro da Web atraves de um formulário
    {   //Metodo Task que devolve uma Guid chamado UploadBlobAsync, que recebe um ficheiro
        //e tem um parametro que é uma string com o nome do contentor onde vamos guardar
        Task<Guid> UploadBlobAsync(IFormFile file, string containerName);

        //Segunda forma de fazer o Upload da imagem, recebe um array de bytes
        //Isto será usado quando se envia imagens atraves do telemovel
        //para dentro do contentor o telemovel so passa o array de imagens nao passa o ficheiro
        Task<Guid> UploadBlobAsync(byte[] file, string containerName);

        //Terceira forma de fazer o Upload da imagem, recebe uma string
        //Isto é quando se recebe uma imagem atraves de um endereço
        Task<Guid> UploadBlobAsync(string image, string containerName);

    }
}
