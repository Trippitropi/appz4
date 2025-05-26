using Microsoft.AspNetCore.Mvc;
using QuestRoom.BLL.Services;
using QuestRoom.PL.Models;
using QuestRoom.DAL.Entities;

namespace QuestRoom.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiftCertificateController : ControllerBase
    {
        private readonly IGiftCertificateService _giftCertificateService;
        private readonly IClientService _clientService;
        private readonly IQuestService _questService;

        public GiftCertificateController(
            IGiftCertificateService giftCertificateService,
            IClientService clientService,
            IQuestService questService)
        {
            _giftCertificateService = giftCertificateService;
            _clientService = clientService;
            _questService = questService;
        }

        /// <summary>
        /// Отримати активні сертифікати для клієнта
        /// </summary>
        /// <param name="clientId">ID клієнта</param>
        /// <returns>Список активних сертифікатів</returns>
        [HttpGet("client/{clientId}")]
        public ActionResult<ApiResponse<List<GiftCertificateDto>>> GetActiveCertificatesForClient(int clientId)
        {
            try
            {
                var certificates = _giftCertificateService.GetActiveCertificatesForClient(clientId);
                var certificateDtos = certificates.Select(c => new GiftCertificateDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    IssueDate = c.IssueDate,
                    ExpiryDate = c.ExpiryDate,
                    IsUsed = c.IsUsed,
                    ClientId = c.ClientId,
                    ClientName = c.Owner?.Name,
                    QuestId = c.QuestId,
                    QuestName = c.Quest?.Name
                }).ToList();

                return Ok(ApiResponse<List<GiftCertificateDto>>.SuccessResult(certificateDtos, "Сертифікати успішно отримані"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<GiftCertificateDto>>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }

        /// <summary>
        /// Перевірити валідність сертифіката
        /// </summary>
        /// <param name="code">Код сертифіката</param>
        /// <returns>Результат перевірки</returns>
        [HttpGet("validate/{code}")]
        public ActionResult<ApiResponse<bool>> ValidateCertificate(string code)
        {
            try
            {
                var isValid = _giftCertificateService.IsValidCertificate(code);
                var message = isValid ? "Сертифікат валідний" : "Сертифікат невалідний або прострочений";

                return Ok(ApiResponse<bool>.SuccessResult(isValid, message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }

        /// <summary>
        /// Створити подарунковий сертифікат
        /// </summary>
        /// <param name="createCertificateDto">Дані для створення сертифіката</param>
        /// <returns>Створений сертифікат</returns>
        [HttpPost]
        public ActionResult<ApiResponse<GiftCertificateDto>> CreateGiftCertificate([FromBody] CreateGiftCertificateDto createCertificateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<GiftCertificateDto>.ErrorResult("Невалідні дані"));
                }

                // Перевіряємо чи існує клієнт (якщо вказаний)
                if (createCertificateDto.ClientId.HasValue)
                {
                    var client = _clientService.GetClientById(createCertificateDto.ClientId.Value);
                    if (client == null)
                    {
                        return BadRequest(ApiResponse<GiftCertificateDto>.ErrorResult("Клієнт не знайдений"));
                    }
                }

                // Перевіряємо чи існує квест (якщо вказаний)
                if (createCertificateDto.QuestId.HasValue)
                {
                    var quest = _questService.GetQuestById(createCertificateDto.QuestId.Value);
                    if (quest == null)
                    {
                        return BadRequest(ApiResponse<GiftCertificateDto>.ErrorResult("Квест не знайдений"));
                    }
                }

                var certificate = _giftCertificateService.CreateCertificate(
                    createCertificateDto.ClientId,
                    createCertificateDto.QuestId,
                    createCertificateDto.ValidityDays);

                var certificateDto = new GiftCertificateDto
                {
                    Id = certificate.Id,
                    Code = certificate.Code,
                    IssueDate = certificate.IssueDate,
                    ExpiryDate = certificate.ExpiryDate,
                    IsUsed = certificate.IsUsed,
                    ClientId = certificate.ClientId,
                    QuestId = certificate.QuestId
                };

                return CreatedAtAction(nameof(ValidateCertificate), new { code = certificate.Code },
                    ApiResponse<GiftCertificateDto>.SuccessResult(certificateDto, "Сертифікат успішно створений"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<GiftCertificateDto>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }
    }
}