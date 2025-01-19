namespace ExemploApi.Models
{
    //public record Endereco(int Id, string? Cep, string? Logradouro, int? Numero, string? Bairro, string? Complemento, string? Cidade, string? Estado, string? Referencia);
    //public record Contato(int Id, string? Email, int? Ddd, string? Telefone);

    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Email { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public List<Contato>? Contato { get; set; }
        public List<Endereco>? Endereco { get; set; }
    }

    public class Endereco
    {
        public Endereco(string? tipoEndereco, string cep, string logradouro, int? numero, string bairro, string complemento, string cidade, string estado, string referencia)
        {
            //Id = idGerado;
            TipoEndereco = tipoEndereco;
            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Bairro = bairro;
            Complemento = complemento;
            Cidade = cidade;
            Estado = estado;
            Referencia = referencia;
        }

        public int Id { get; set; }
        public string? TipoEndereco { get; set; }
        public string Cep { get; set; }
        public string? Logradouro { get; set; }
        public int? Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Complemento { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Referencia { get; set; }
    }

    public class Contato
    {
        public Contato(string? tipoContato, int ddd, string telefone)
        {
            TipoContato = tipoContato;
            Ddd = ddd;
            Telefone = telefone;
        }

        public int Id { get; set; }
        public string? TipoContato { get; set; }
        public int Ddd { get; set; }
        public string Telefone { get; set; }
    }
}
