using ExemploApi.Models;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ExemploApi.Persistencia
{
    public class Validacao()
    {
        public bool ValidarEmail(string? email)
        {
            var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

            if (regex.IsMatch(email) || email == string.Empty) {
                return true;
            }
            else throw new Exception("Erro ao validar o e-mail");
        }

        public bool ValidarCpf(string? cpf)
        {
            var regex = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$");

            if (regex.IsMatch(cpf))
                return true;
            else throw new Exception("Erro ao validar o CPF");
        }

        public bool ValidarRg(string rg)
        {
            var regex = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$");

            if (regex.IsMatch(rg))
                return true;
            else throw new Exception("Erro ao validar o Rg");
        }

        public bool ValidarCep(string cep)
        {
            var regex = new Regex(@"^\d{5}-\d{3}$");
            if (regex.IsMatch(cep))
            {
                return true;
            }
            else 
                throw new Exception("Erro ao validar o cep");
        }

        public bool ValidarTelefone(string telefone)
        {
            var regex = new Regex(@"^\d{5}-\d{4}$");
            if (regex.IsMatch(telefone))
            {
                return true;
            }
            else
                throw new Exception("Erro ao validar o telefone");
        }

        public bool ValidarDadosCliente(Cliente cliente)
        {
            bool emailValido = ValidarEmail(cliente.Email);

            bool cpfValido = ValidarCpf(cliente.Cpf);
            bool rgValido = ValidarRg(cliente.Rg);

            bool enderecosValidos = cliente.Endereco != null && cliente.Endereco.All(e => ValidarCep(e.Cep));

            return emailValido && cpfValido && rgValido && enderecosValidos;
        }
    }
}
