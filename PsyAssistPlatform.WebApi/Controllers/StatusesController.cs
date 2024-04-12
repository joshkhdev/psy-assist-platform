using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Status;

namespace PsyAssistPlatform.WebApi.Controllers
{
    /// <summary>
    /// Статусы заявок
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StatusesController : ControllerBase
    {
        private readonly IRepository<Status> _statusRepository;
        private readonly IMapper _mapper;
        public StatusesController(IRepository<Status> statusRepository, IMapper mapper)
        {
            _statusRepository = statusRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить список статусов
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<StatusShortResponse>> GetStatusesAsync(CancellationToken cancellationToken)
        {
            var statuses = await _statusRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<StatusShortResponse>>(statuses);
        }

        /// <summary>
        /// Получить статус по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusResponse>> GetStatusAsync(int id, CancellationToken cancellationToken)
        {
            var status = await _statusRepository.GetByIdAsync(id, cancellationToken);

            if (status == null)
                return NotFound($"Status {id} doesn't found");

            return Ok(_mapper.Map<StatusResponse>(status));
        }

        /// <summary>
        /// Добавить статус
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateStatusAsync(CreateStatusRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest("No data to fill out");

            var status = _mapper.Map<Status>(request);

            await _statusRepository.AddAsync(status, cancellationToken);

            return Ok(_mapper.Map<StatusShortResponse>(status));
        }

        /// <summary>
        /// Обновить статус
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatusAsync(int id, UpdateStatusRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest("No data to fill out");

            var status = await _statusRepository.GetByIdAsync(id, cancellationToken);

            if (status == null)
                return NotFound($"Status {id} doesn't found");

            var statusUpdate = _mapper.Map<Status>(request);
            statusUpdate.Id = status.Id;

            await _statusRepository.UpdateAsync(statusUpdate, cancellationToken);

            return Ok(_mapper.Map<StatusShortResponse>(statusUpdate));
        }

        /// <summary>
        /// Удалить статус
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteStatusAsync(int id, CancellationToken cancellationToken)
        {
            var status = await _statusRepository.GetByIdAsync(id, cancellationToken);

            if (status == null)
                return NotFound($"Status {id} doesn't found");

            await _statusRepository.DeleteAsync(id, cancellationToken);

            return Ok();
        }
    }
}
