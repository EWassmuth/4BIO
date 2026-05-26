namespace ExemploApi.Models.DTO
{
    public record EnderecoDto(string? TipoEndereco, string Cep, string Logradouro, int? Numero, string Bairro, string Complemento, string Cidade, string Estado, string Referencia);
    public record ContatoDto(string? TipoContato, int Ddd, string Telefone);
    public record AtualizarClienteDto (string Nome, string? Email, string Cpf, string Rg);

    public class ClienteDto
    {
        public string Nome { get; set; }
        public string? Email { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public List<ContatoDto>? Contato { get; set; }
        public List<EnderecoDto>? Endereco { get; set; }
    }
}
