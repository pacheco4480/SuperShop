namespace SuperShop.Data.Entities
{
    public interface IEntity
    {   //Isto será o que vai ser comum em todas as Entidades
        int Id { get; set; }

        //Isto serve para "apagar" um registo, por defeito o valor do boolenao é false, quando
        //quiser apagar o registo muda-se o valor para true, o registo continua a existir mas é considerado apagado
        //Podemos utilizar isto para o utilizador apagar registos sem apagar da base de dados
        //Neste exercicio nao vamos utilizar esta propriedade
        //bool WasDeleted { get; set; }

    }
}
