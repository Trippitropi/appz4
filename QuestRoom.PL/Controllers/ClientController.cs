using Microsoft.AspNetCore.Mvc;
using QuestRoom.BLL.Services;
using QuestRoom.PL.Models;
using QuestRoom.DAL.Entities;

namespace QuestRoom.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Отримати всіх клієнтів
        /// </summary>
        /// <returns>Список клієнтів</returns>
        [HttpGet]
        public ActionResult<ApiResponse<List<ClientDto>>> GetAllClients()
        {
            try
            {
                var clients = _clientService.GetAllClients();
                var clientDtos = clients.Select(c => new ClientDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone
                }).ToList();

                return Ok(ApiResponse<List<ClientDto>>.SuccessResult(clientDtos, "Клієнти успішно отримані"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ClientDto>>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }

        /// <summary>
        /// Отримати клієнта за ID
        /// </summary>
        /// <param name="id">ID клієнта</param>
        /// <returns>Клієнт</returns>
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<ClientDto>> GetClientById(int id)
        {
            try
            {
                var client = _clientService.GetClientById(id);
                if (client == null)
                {
                    return NotFound(ApiResponse<ClientDto>.ErrorResult("Клієнт не знайдений"));
                }

                var clientDto = new ClientDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    Phone = client.Phone
                };

                return Ok(ApiResponse<ClientDto>.SuccessResult(clientDto, "Клієнт успішно отриманий"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ClientDto>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }

        /// <summary>
        /// Створити нового клієнта
        /// </summary>
        /// <param name="createClientDto">Дані для створення клієнта</param>
        /// <returns>Створений клієнт</returns>
        [HttpPost]
        public ActionResult<ApiResponse<ClientDto>> CreateClient([FromBody] CreateClientDto createClientDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<ClientDto>.ErrorResult("Невалідні дані"));
                }

                var client = new Client
                {
                    Name = createClientDto.Name,
                    Email = createClientDto.Email,
                    Phone = createClientDto.Phone
                };

                _clientService.AddClient(client);

                var clientDto = new ClientDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    Phone = client.Phone
                };

                return CreatedAtAction(nameof(GetClientById), new { id = client.Id },
                    ApiResponse<ClientDto>.SuccessResult(clientDto, "Клієнт успішно створений"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ClientDto>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }

        /// <summary>
        /// Оновити клієнта
        /// </summary>
        /// <param name="id">ID клієнта</param>
        /// <param name="createClientDto">Оновлені дані клієнта</param>
        /// <returns>Оновлений клієнт</returns>
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<ClientDto>> UpdateClient(int id, [FromBody] CreateClientDto createClientDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<ClientDto>.ErrorResult("Невалідні дані"));
                }

                var existingClient = _clientService.GetClientById(id);
                if (existingClient == null)
                {
                    return NotFound(ApiResponse<ClientDto>.ErrorResult("Клієнт не знайдений"));
                }

                existingClient.Name = createClientDto.Name;
                existingClient.Email = createClientDto.Email;
                existingClient.Phone = createClientDto.Phone;

                _clientService.UpdateClient(existingClient);

                var clientDto = new ClientDto
                {
                    Id = existingClient.Id,
                    Name = existingClient.Name,
                    Email = existingClient.Email,
                    Phone = existingClient.Phone
                };

                return Ok(ApiResponse<ClientDto>.SuccessResult(clientDto, "Клієнт успішно оновлений"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ClientDto>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }
    }
}