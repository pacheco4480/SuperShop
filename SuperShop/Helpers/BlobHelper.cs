using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    //Ctrl  + . em cima do IBlobHelper - Implement interface
    public class BlobHelper : IBlobHelper
    {
        //Isto vai ser o que nos vai ligar ao container
        private readonly CloudBlobClient _blobClient;

        //Construtor
        //Isto é só para ligar e fazer a ligaçao à conta
        public BlobHelper(IConfiguration configuration)
        {
            //Isto é a forma que usamos quando queremos ir buscar dados diretamente ao appsettings.json
            string keys = configuration["Blob:ConnectionStrings"];
            //Ctrl  + . em cima do CloudStorageAccount e clicar em "Install package "WindowsAzure.Storage"
            //É aqui que é feita a ligaçao ao storage que foi criado no Azure
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            //Cria um Stream vai buscar o nosso ficheiro agarra o ficheiro e manda para dentro do contentor
            Stream stream = file.OpenReadStream();
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(byte[] file, string containerName)
        {
            MemoryStream stream = new MemoryStream(file);
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(string image, string containerName)
        {
            Stream stream = File.OpenRead(image);
            return await UploadStreamAsync(stream, containerName);
        }

        //Recebe o stream, recebe o nome do contentor onde quer publicar
        private async Task<Guid> UploadStreamAsync(Stream stream, string containerName)
        {   //Cria o Guid
            Guid name = Guid.NewGuid();
            //Faz a ligaçao ao Blob e vai buscar o nome do contentor
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            //Aqui é passado o nome do contentor
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{name}");
            //Aqui faz o upload para a stream
            await blockBlob.UploadFromStreamAsync(stream);
            //O nome que é retornado aqui é o Guid que vai estar na imagem
            return name;
        }
    }
}
