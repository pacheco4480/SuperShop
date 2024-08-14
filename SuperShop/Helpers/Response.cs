namespace SuperShop.Helpers
{
    // Classe que representa a resposta de uma operação.
    public class Response
    {
        // Indica se a operação foi bem-sucedida ou não.
        public bool IsSuccess { get; set; }

        // Contém uma mensagem descritiva sobre o resultado da operação.
        public string Message { get; set; }

        // Contém os resultados da operação, se houver algum.
        public object Results;
    }
}
