using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Approach;

namespace PsyAssistPlatform.WebApi.Controllers
{
    /// <summary>
    /// Подходы психологов
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ApproachesController : ControllerBase
    {
        private readonly IRepository<Approach> _approachRepository;
        private readonly IMapper _mapper;
        public ApproachesController(IRepository<Approach> approachRepository, IMapper mapper)
        {
            _approachRepository = approachRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить список подходов
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<ApproachShortResponse>> GetApproachesAsync(CancellationToken cancellationToken)
        {
            var approaches = await _approachRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ApproachShortResponse>>(approaches);
        }

        /// <summary>
        /// Получить подход по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApproachResponse>> GetApproachAsync(int id, CancellationToken cancellationToken)
        {
            var approach = await _approachRepository.GetByIdAsync(id, cancellationToken);

            if (approach == null)
                return NotFound($"Approach {id} doesn't found");

            return Ok(_mapper.Map<ApproachResponse>(approach));
        }

        /// <summary>
        /// Добавить подход
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateApproachAsync(CreateApproachRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest("No data to fill out");

            var approach = _mapper.Map<Approach>(request);

            await _approachRepository.AddAsync(approach, cancellationToken);

            return Ok(_mapper.Map<ApproachShortResponse>(approach));
        }

        /// <summary>
        /// Обновить подход
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApproachAsync(int id, UpdateApproachRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest("No data to fill out");

            var approach = await _approachRepository.GetByIdAsync(id, cancellationToken);

            if (approach == null)
                return NotFound($"Approach {id} doesn't found");

            var approachUpdate = _mapper.Map<Approach>(request);
            approachUpdate.Id = approach.Id;

            await _approachRepository.UpdateAsync(approachUpdate, cancellationToken);

            return Ok(_mapper.Map<ApproachShortResponse>(approachUpdate));
        }

        /// <summary>
        /// Удалить подход
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(int id, CancellationToken cancellationToken)
        {
            var approach = await _approachRepository.GetByIdAsync(id, cancellationToken);

            if (approach == null)
                return NotFound($"Approach {id} doesn't found");

            await _approachRepository.DeleteAsync(id, cancellationToken);

            return Ok();
        }
    }
}
