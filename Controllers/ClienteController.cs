using ExemploApi.Models;
using ExemploApi.Models.DTO;
using ExemploApi.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace ExemploApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _service;

        public ClienteController(ClienteService service)
        {
            _service = service;
        }

        #region Cliente

        [HttpGet("listar")]
        public IActionResult ListarClientes([FromQuery] string nome = null, [FromQuery] string cpf = null)
        {
            var clientes = _service.GetAll(nome, cpf);
            if(!clientes.Any())
                return NotFound();

            return Ok(clientes);
        }

        [HttpPost("criar")]
        public IActionResult CriarCliente([FromBody] ClienteDto clienteDto)
        {
            
            var cliente = new Cliente
            {
                Nome = clienteDto.Nome,
                Email = clienteDto.Email,
                Cpf = clienteDto.Cpf,
                Rg = clienteDto.Rg,
                Contato = clienteDto.Contato?.Select(c => new Contato(c.TipoContato, c.Ddd, c.Telefone)).ToList(),
                Endereco = clienteDto.Endereco?.Select(e => new Endereco(e.TipoEndereco, e.Cep, e.Logradouro, e.Numero, e.Bairro, e.Complemento, e.Cidade, e.Estado, e.Referencia)).ToList(),
            };

            try
            { 
                _service.CreateCliente(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, new { error = ex.Message });
            }

            return CreatedAtAction(nameof(ListarClientes), new { id = cliente.Id }, cliente);
        }

        [HttpPut("atualizar/cliente/{id}")]
        public IActionResult AtualizarCliente(int id, [FromBody] AtualizarClienteDto AtualizarClienteDto)
        {
            if (_service.GetClienteById(id) == null) return NotFound();
            var cliente = new Cliente
            {
                Nome = AtualizarClienteDto.Nome,
                Email = AtualizarClienteDto.Email,
                Cpf = AtualizarClienteDto.Cpf,
                Rg = AtualizarClienteDto.Rg,
            };
            
            cliente.Id = id;

            try
            {
                _service.UpdateCliente(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, new { error = ex.Message });
            }
            
            return NoContent();
        }

        [HttpDelete("remover/cliente/{id}")]
        public IActionResult RemoverCliente(int id)
        {
            var cliente = _service.GetClienteById(id);

            if (cliente == null)
                return StatusCode(StatusCodes.Status404NotFound, new { error = "Cliente não encontrado" });

            _service.DeleteCliente(id);
            return NoContent();
        }

        #endregion

        #region Endereco

        [HttpPost("criar/endereco")]
        public IActionResult AdicionarEndereco([FromQuery] string cpf, [FromBody] EnderecoDto enderecoDto)
        {
            var cliente = _service.GetClienteByCpf(cpf);
            
            if (cliente == null)
                return StatusCode(StatusCodes.Status404NotFound, new { error = "Cliente não encontrado" });

            var endereco = new Endereco(
                tipoEndereco: enderecoDto.TipoEndereco,
                cep: enderecoDto.Cep,
                logradouro: enderecoDto.Logradouro,
                numero: enderecoDto.Numero,
                bairro: enderecoDto.Bairro,
                complemento: enderecoDto.Complemento,
                cidade: enderecoDto.Cidade,
                estado: enderecoDto.Estado,
                referencia: enderecoDto.Referencia
            );

            try
            {
                _service.CreateEndereco(cpf, endereco);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, new { error = ex.Message });
            }

            return Ok(endereco);
        }


        [HttpPut("atualizar/endereco/{id}")]
        public IActionResult AtualizarEndereco(int id, [FromBody] EnderecoDto enderecoDto)

        {
            if (_service.GetEnderecoById(id) == null) return NotFound();

            var endereco = new Endereco(
                enderecoDto.TipoEndereco,
                enderecoDto.Cep,
                enderecoDto.Logradouro,
                enderecoDto.Numero,
                enderecoDto.Bairro,
                enderecoDto.Complemento,
                enderecoDto.Cidade,
                enderecoDto.Estado,
                enderecoDto.Referencia
            );

            endereco.Id = id;

            try
            {
                _service.UpdateEndereco(endereco);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, new { error = ex.Message });
            }

            return NoContent();
        }

        [HttpDelete("remover/endereco/{id}")]
        public IActionResult RemoverEndereco(int id)
        {
            if (_service.GetEnderecoById(id) == null)
                return NotFound();

            _service.DeleteEndereco(id);
            return NoContent();
        }

        #endregion

        #region Contato
        

        [HttpPost("criar/contato")]
        public IActionResult AdicionarContato([FromQuery] string cpf, [FromBody] ContatoDto contatoDto)
        {
            var cliente = _service.GetClienteByCpf(cpf);

            if (cliente == null)
                return StatusCode(StatusCodes.Status404NotFound, new { error = "Cliente não encontrado" });

            var contato = new Contato(
                contatoDto.TipoContato,
                contatoDto.Ddd,
                contatoDto.Telefone
            );

            try
            {
                _service.CreateContato(cpf, contato);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, new { error = ex.Message });
            }

            return Ok(contato);
        }

        [HttpPut("atualizar/contato/{id}")]
        public IActionResult AtualizarContato(int id, [FromBody] ContatoDto contatoDto)
        {
            if (_service.GetContatoById(id) == null) return NotFound();

            var contato = new Contato(
                contatoDto.TipoContato,
                contatoDto.Ddd,
                contatoDto.Telefone
            );

            contato.Id = id;

            try
            {
                _service.UpdateContato(contato);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, new { error = ex.Message });
            }

            return NoContent();
        }

        [HttpDelete("remover/contato/{id}")]
        public IActionResult RemoverContato(int id)
        {
            if (_service.GetContatoById(id) == null)
                return NotFound();

            _service.DeleteContato(id);
            return NoContent();
        }
        #endregion
    }
}