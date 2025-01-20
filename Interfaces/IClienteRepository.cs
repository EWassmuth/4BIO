using ExemploApi.Models;
using System.Text.Json;

namespace ExemploApi.Interfaces
{
    public interface IClienteRepository
    {
        public IEnumerable<Cliente> GetAll(string nome = null, string cpf = null);

        #region Cliente

        public Cliente GetClienteById(int id);
        public Cliente GetClienteByNome(string nome);
        public Cliente GetClienteByCpf(string nome);

        public void CreateCliente(Cliente cliente);
        public void UpdateCliente(Cliente cliente);
        public void DeleteCliente(int id);

        #endregion

        #region Endereco

        public void CreateEndereco(string cpf, Endereco endereco);
        public void UpdateEndereco(Endereco endereco);
        public void DeleteEndereco(int id);

        #endregion

        #region Contato

        public void CreateContato(string cpf, Contato contato);
        public void UpdateContato(Contato contato);
        public void DeleteContato(int id);

        #endregion
    }
}
