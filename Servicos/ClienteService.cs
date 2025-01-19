using ExemploApi.Models;
using ExemploApi.Persistencia;

namespace ExemploApi.Servicos
{
    public class ClienteService
    {
        private readonly ClienteRepository _repository;

        public ClienteService(ClienteRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Cliente> GetAll(string nome = null, string cpf = null) =>
            _repository.GetAll(nome, cpf);

        public Endereco GetEnderecoById(int id) => _repository.GetEnderecoById(id);
        public Contato GetContatoById(int id) => _repository.GetContatoById(id);

        #region Cliente

        public Cliente GetClienteById(int id) => _repository.GetClienteById(id);
        public Cliente GetClienteByCpf(string cpf) => _repository.GetClienteByCpf(cpf);
        public void CreateCliente(Cliente cliente) => _repository.CreateCliente(cliente);
        public void UpdateCliente(Cliente cliente) => _repository.UpdateCliente(cliente);
        public void DeleteCliente(int id) => _repository.DeleteCliente(id);

        #endregion

        #region Endereco

        public void CreateEndereco(string cpf, Endereco endereco) => _repository.CreateEndereco(cpf, endereco);

        public void DeleteEndereco(int id) => _repository.DeleteEndereco(id);
        public void UpdateEndereco(Endereco endereco) => _repository.UpdateEndereco(endereco);

        #endregion

        #region Contato

        public void CreateContato(string cpf, Contato contato) => _repository.CreateContato(cpf, contato);
        public void DeleteContato(int id) => _repository.DeleteContato(id);
        public void UpdateContato(Contato contato) => _repository.UpdateContato(contato);

        #endregion
    }
}
