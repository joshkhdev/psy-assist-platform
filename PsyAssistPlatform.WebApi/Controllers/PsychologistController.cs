using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Psychologist;

namespace PsyAssistPlatform.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PsychologistController(
        IRepository<Psychologist> psychologistRepository,
        IRepository<User> userRepository,
        IMapper mapper) : ControllerBase
    {
        /// <summary>
        /// Получение списка всех психологов
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<PsychologistShortResponse>> GetAllAsync()
            => mapper.Map<IEnumerable<PsychologistShortResponse>>(
                await psychologistRepository.GetAllAsync(CancellationToken.None));

        /// <summary>
        /// Получение психолога по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
            => Ok(mapper.Map<PsychologistResponse>(
                await psychologistRepository.GetByIdAsync(id, CancellationToken.None)));

        /// <summary>
        /// Создание нового психолога
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddAsync(CreatePsychologistRequest request)
        {
            // TODO: ? Handle User Creation ?
            var user = await userRepository
                .GetByIdAsync(request.UserId, CancellationToken.None);

            if (user == null)
                return NotFound();

            var psychologist = mapper.Map<Psychologist>(request);
            psychologist.User = user;

            await psychologistRepository.AddAsync(psychologist, CancellationToken.None);

            return Ok(new { id = psychologist.Id });
        }

        /// <summary>
        /// Обновление данных о психологе
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdatePsychologistRequest request)
        {
            // TODO: ? Handle User Creation ?
            var user = await userRepository
                .GetByIdAsync(request.UserId, CancellationToken.None);
            var psychologist = 
                await psychologistRepository.GetByIdAsync(id, CancellationToken.None);

            if (psychologist == null || user == null)
                return NotFound();

            var psychologistModel = mapper.Map<Psychologist>(request);
            psychologistModel.Id = psychologist.Id;
            psychologistModel.User = user;

            await psychologistRepository.UpdateAsync(psychologistModel, CancellationToken.None);

            return Ok(new { id = psychologistModel.Id });
        }

        /// <summary>
        /// Удаление данных о психологе
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var psychologist = 
                await psychologistRepository.GetByIdAsync(id, CancellationToken.None);

            if (psychologist == null)
                return NotFound();

            await psychologistRepository.DeleteAsync(id, CancellationToken.None);

            return Ok();
        }
    }
}
