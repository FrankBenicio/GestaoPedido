namespace GestaoPedido.Domain.Models;

public class Cliente : EntityBase
{
    public string Nome { get; private set; } = string.Empty;
    public string Documento { get; private set; } = string.Empty;
    public bool Ativo { get; private set; } = true;

    protected Cliente() { }

    private Cliente(string nome, string documento)
    {
        Nome = nome;
        Documento = documento;
        Ativo = true;
    }

    public static Cliente Criar(string nome, string documento)
    {
        return new Cliente(nome, documento);
    }

}
