using Microsoft.AspNetCore.Mvc;
using QuestRoom.BLL.Services;
using QuestRoom.PL.Models;
using QuestRoom.DAL.Entities;
using global::QuestRoom.BLL.Services;
using global::QuestRoom.DAL.Entities;
using global::QuestRoom.PL.Models;

namespace QuestRoom.PL.Controllers
{
  
        [ApiController]
        [Route("api/[controller]")]
        public class QuestController : ControllerBase
        {
            private readonly IQuestService _questService;

            public QuestController(IQuestService questService)
            {
                _questService = questService;
            }

            /// <summary>
            /// Отримати всі квести
            /// </summary>
            /// <returns>Список квестів</returns>
            [HttpGet]
            public ActionResult<ApiResponse<List<QuestDto>>> GetAllQuests()
            {
                try
                {
                    var quests = _questService.GetAllQuests();
                    var questDtos = quests.Select(q => new QuestDto
                    {
                        Id = q.Id,
                        Name = q.Name,
                        Description = q.Description,
                        MaxParticipants = q.MaxParticipants,
                        DurationMinutes = q.DurationMinutes,
                        Price = q.Price,
                        DifficultyLevel = q.DifficultyLevel,
                        ImageUrl = q.ImageUrl,
                        IsActive = q.IsActive
                    }).ToList();

                    return Ok(ApiResponse<List<QuestDto>>.SuccessResult(questDtos, "Квести успішно отримані"));
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ApiResponse<List<QuestDto>>.ErrorResult($"Помилка сервера: {ex.Message}"));
                }
            }

            /// <summary>
            /// Отримати квест за ID
            /// </summary>
            /// <param name="id">ID квесту</param>
            /// <returns>Квест</returns>
            [HttpGet("{id}")]
            public ActionResult<ApiResponse<QuestDto>> GetQuestById(int id)
            {
                try
                {
                    var quest = _questService.GetQuestById(id);
                    if (quest == null)
                    {
                        return NotFound(ApiResponse<QuestDto>.ErrorResult("Квест не знайдено"));
                    }

                    var questDto = new QuestDto
                    {
                        Id = quest.Id,
                        Name = quest.Name,
                        Description = quest.Description,
                        MaxParticipants = quest.MaxParticipants,
                        DurationMinutes = quest.DurationMinutes,
                        Price = quest.Price,
                        DifficultyLevel = quest.DifficultyLevel,
                        ImageUrl = quest.ImageUrl,
                        IsActive = quest.IsActive
                    };

                    return Ok(ApiResponse<QuestDto>.SuccessResult(questDto, "Квест успішно отриманий"));
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ApiResponse<QuestDto>.ErrorResult($"Помилка сервера: {ex.Message}"));
                }
            }

            /// <summary>
            /// Створити новий квест
            /// </summary>
            /// <param name="createQuestDto">Дані для створення квесту</param>
            /// <returns>Створений квест</returns>
            [HttpPost]
            public ActionResult<ApiResponse<QuestDto>> CreateQuest([FromBody] CreateQuestDto createQuestDto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ApiResponse<QuestDto>.ErrorResult("Невалідні дані"));
                    }

                    var quest = new Quest
                    {
                        Name = createQuestDto.Name,
                        Description = createQuestDto.Description,
                        MaxParticipants = createQuestDto.MaxParticipants,
                        DurationMinutes = createQuestDto.DurationMinutes,
                        Price = createQuestDto.Price,
                        DifficultyLevel = createQuestDto.DifficultyLevel ?? "Середній",
                        ImageUrl = createQuestDto.ImageUrl ?? "placeholder.jpg",
                        IsActive = true
                    };

                    _questService.AddQuest(quest);

                    var questDto = new QuestDto
                    {
                        Id = quest.Id,
                        Name = quest.Name,
                        Description = quest.Description,
                        MaxParticipants = quest.MaxParticipants,
                        DurationMinutes = quest.DurationMinutes,
                        Price = quest.Price,
                        DifficultyLevel = quest.DifficultyLevel,
                        ImageUrl = quest.ImageUrl,
                        IsActive = quest.IsActive
                    };

                    return CreatedAtAction(nameof(GetQuestById), new { id = quest.Id },
                        ApiResponse<QuestDto>.SuccessResult(questDto, "Квест успішно створено"));
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ApiResponse<QuestDto>.ErrorResult($"Помилка сервера: {ex.Message}"));
                }
            }

            /// <summary>
            /// Оновити квест
            /// </summary>
            /// <param name="id">ID квесту</param>
            /// <param name="createQuestDto">Оновлені дані квесту</param>
            /// <returns>Оновлений квест</returns>
            [HttpPut("{id}")]
            public ActionResult<ApiResponse<QuestDto>> UpdateQuest(int id, [FromBody] CreateQuestDto createQuestDto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ApiResponse<QuestDto>.ErrorResult("Невалідні дані"));
                    }

                    var existingQuest = _questService.GetQuestById(id);
                    if (existingQuest == null)
                    {
                        return NotFound(ApiResponse<QuestDto>.ErrorResult("Квест не знайдено"));
                    }

                    existingQuest.Name = createQuestDto.Name;
                    existingQuest.Description = createQuestDto.Description;
                    existingQuest.MaxParticipants = createQuestDto.MaxParticipants;
                    existingQuest.DurationMinutes = createQuestDto.DurationMinutes;
                    existingQuest.Price = createQuestDto.Price;
                    existingQuest.DifficultyLevel = createQuestDto.DifficultyLevel ?? existingQuest.DifficultyLevel;
                    existingQuest.ImageUrl = createQuestDto.ImageUrl ?? existingQuest.ImageUrl;

                    _questService.UpdateQuest(existingQuest);

                    var questDto = new QuestDto
                    {
                        Id = existingQuest.Id,
                        Name = existingQuest.Name,
                        Description = existingQuest.Description,
                        MaxParticipants = existingQuest.MaxParticipants,
                        DurationMinutes = existingQuest.DurationMinutes,
                        Price = existingQuest.Price,
                        DifficultyLevel = existingQuest.DifficultyLevel,
                        ImageUrl = existingQuest.ImageUrl,
                        IsActive = existingQuest.IsActive
                    };

                    return Ok(ApiResponse<QuestDto>.SuccessResult(questDto, "Квест успішно оновлено"));
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ApiResponse<QuestDto>.ErrorResult($"Помилка сервера: {ex.Message}"));
                }
            }

            /// <summary>
            /// Видалити квест
            /// </summary>
            /// <param name="id">ID квесту</param>
            /// <returns>Результат операції</returns>
            [HttpDelete("{id}")]
            public ActionResult<ApiResponse<object>> DeleteQuest(int id)
            {
                try
                {
                    var quest = _questService.GetQuestById(id);
                    if (quest == null)
                    {
                        return NotFound(ApiResponse<object>.ErrorResult("Квест не знайдено"));
                    }

                    _questService.DeleteQuest(id);

                    return Ok(ApiResponse<object>.SuccessResult(null, "Квест успішно видалено"));
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ApiResponse<object>.ErrorResult($"Помилка сервера: {ex.Message}"));
                }
            }
        }
    }

