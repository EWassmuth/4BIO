using ExemploApi.Interfaces;
using ExemploApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Text.Json;

namespace ExemploApi.Persistencia
{
    public class ClienteRepository  :   IClienteRepository
    {
        private readonly string _filePath = "clientes.json";
        private List<Cliente> _clientes;

        Validacao validacao = new Validacao();

        public ClienteRepository()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true
            };

            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                _clientes = JsonSerializer.Deserialize<List<Cliente>>(jsonData, options) ?? new List<Cliente>();
            }
            else
                _clientes = new List<Cliente>();
        }

        #region Cliente

        public IEnumerable<Cliente> GetAll(string nome = null, string cpf = null)
        {
            return _clientes.Where(c =>
                (string.IsNullOrEmpty(nome) || c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(cpf) || c.Cpf == cpf));
        }
        public Cliente GetClienteById(int id) => _clientes.FirstOrDefault(c => c.Id == id);
        public Cliente GetClienteByCpf(string cpf) => _clientes.FirstOrDefault(c => c.Cpf == cpf);
        public Cliente GetClienteByNome(string nome) => _clientes.FirstOrDefault(c => c.Nome == nome);
        public void CreateCliente(Cliente cliente)
        {            
            if (cliente == null || !validacao.ValidarDadosCliente(cliente))
                return;

            cliente.Id = _clientes.Any() ? _clientes.Max(c => c.Id) + 1 : 1;

            if (cliente?.Endereco != null)
            {
                foreach (var endereco in cliente.Endereco)
                {
                    //endereco.Id = _clientes.Where(e => e.Endereco != null).SelectMany(e => e.Endereco).Select(e => e.Id).DefaultIfEmpty(1).Max() + 1;
                    endereco.Id = GetEnderecoId();
                }
            }

            if (cliente?.Contato != null)
            {
                foreach (var contato in cliente.Contato)
                {
                    //contato.Id = _clientes.Where(c => c.Contato != null).SelectMany(c => c.Contato).Select(c => c.Id).DefaultIfEmpty(1).Max() + 1;
                    contato.Id = GetContatoId();
                }
            }

            _clientes.Add(cliente);
            SaveToFile();
        }

        public void UpdateCliente(Cliente cliente)
        {
            if (cliente == null || !validacao.ValidarEmail(cliente.Email) || !validacao.ValidarCpf(cliente.Cpf) || !validacao.ValidarRg(cliente.Rg))
                return;

            var clienteExistente = _clientes.FirstOrDefault(c => c.Id == cliente.Id);

            if (clienteExistente == null)
                return;

            cliente.Endereco ??= new List<Endereco>();
            cliente.Contato ??= new List<Contato>();

            cliente.Endereco = clienteExistente.Endereco.Where(e => cliente.Endereco.All(ne => ne.Id != e.Id)).Concat(cliente.Endereco).ToList();

            cliente.Contato = clienteExistente.Contato.Where(co => cliente.Contato.All(nc => nc.Id != co.Id)).Concat(cliente.Contato).ToList();

            var index = _clientes.FindIndex(c => c.Id == cliente.Id);

            if (index != -1)
            {
                _clientes[index] = cliente;
                SaveToFile();
            }
        }

        public void DeleteCliente(int id)
        {
            var cliente = GetClienteById(id);
            if (cliente != null)
            {
                _clientes.Remove(cliente);
                SaveToFile();
            }
            else
                return;
        }

        #endregion

        #region Endereco

        public Endereco GetEnderecoById(int id) => _clientes.SelectMany(c => c.Endereco).FirstOrDefault(e => e.Id == id);
        public void CreateEndereco(string cpf, Endereco endereco)
        {
            if (endereco == null || !validacao.ValidarCep(endereco.Cep) || !validacao.ValidarCpf(cpf))
                return;

            var cliente = GetClienteByCpf(cpf);
            
            if (cliente == null)
                return;

            //endereco.Id = _clientes.Where(e => e.Endereco != null).SelectMany(e => e.Endereco).Select(e => e.Id).DefaultIfEmpty(1).Max() + 1;
            endereco.Id = GetContatoId();

            cliente.Endereco.Add(endereco);

            SaveToFile();
        }

        public void UpdateEndereco(Endereco endereco)
        {
            if (endereco == null || !validacao.ValidarCep(endereco.Cep))
                return;

            var cliente = _clientes.FirstOrDefault(c => c.Id == FindClienteId(endereco.Id, null));
            
            if (cliente == null)
                return;

            var index = cliente.Endereco.ToList().FindIndex(e => e.Id == endereco.Id);

            if (index != -1)
            {
                cliente.Endereco[index] = endereco;
                SaveToFile();
            }
        }

        public void DeleteEndereco(int id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == FindClienteId(id, null));
            if (cliente == null)
                return;

            var endereco = cliente.Endereco.FirstOrDefault(e => e.Id == id);
            if (endereco == null)
                return;

            cliente.Endereco.Remove(endereco);
            SaveToFile();
        }
        #endregion

        #region Contato

        public Contato GetContatoById(int id) => _clientes.SelectMany(c => c.Contato).FirstOrDefault(e => e.Id == id);
        public void CreateContato(string cpf, Contato contato)
        {
            if (contato == null || !validacao.ValidarCpf(cpf) || !validacao.ValidarTelefone(contato.Telefone))
                return;

            var cliente = GetClienteByCpf(cpf);

            if (cliente == null)
                return;

            //contato.Id = _clientes.Where(c => c.Contato != null).SelectMany(c => c.Contato).Select(c => c.Id).DefaultIfEmpty(1).Max() + 1;
            contato.Id = GetContatoId();

            cliente.Contato.Add(contato);

            SaveToFile();
        }

        public void UpdateContato(Contato contato)
        {
            if (contato == null || !validacao.ValidarTelefone(contato.Telefone))
                return;

            var cliente = _clientes.FirstOrDefault(c => c.Id == FindClienteId(null, contato.Id));
            
            if (cliente == null)
                return;

            var index = cliente.Contato.ToList().FindIndex(c => c.Id == contato.Id);

            if (index != -1)
            {
                cliente.Contato[index] = contato;
                SaveToFile();
            }
        }

        public void DeleteContato(int id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == FindClienteId(null, id));
            if (cliente == null)
                return;

            var contato = cliente.Contato.FirstOrDefault(e => e.Id == id);
            if (contato == null)
                return;

            cliente.Contato.Remove(contato);
            SaveToFile();
        }

        #endregion

        private int GetEnderecoId() => _clientes.Where(e => e.Endereco != null).SelectMany(e => e.Endereco).Select(e => e.Id).DefaultIfEmpty(1).Max() + 1;
        private int GetContatoId() => _clientes.Where(c => c.Contato != null).SelectMany(c => c.Contato).Select(c => c.Id).DefaultIfEmpty(1).Max() + 1;

        public int FindClienteId(int? enderecoId, int? contatoId)
        {
            object clienteConsulta;
            foreach (var cliente in _clientes)
            {
                if(contatoId == null)
                    clienteConsulta = cliente.Endereco.FirstOrDefault(e => e.Id == enderecoId);       
                else
                    clienteConsulta = cliente.Contato?.FirstOrDefault(e => e.Id == contatoId);

                if (clienteConsulta != null)
                    return cliente.Id;
            }

            return -1;
        }
        private void SaveToFile()
        {
            var jsonData = JsonSerializer.Serialize(_clientes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}