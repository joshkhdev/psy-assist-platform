using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Contact;

namespace PsyAssistPlatform.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IRepository<Contact> _contactRepository;
    private readonly IMapper _mapper;

    public ContactsController(
        IRepository<Contact> contactRepository, 
        IMapper mapper)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получение списка всех контактов
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(_mapper.Map<IEnumerable<ContactResponse>>(
            await _contactRepository.GetAllAsync(cancellationToken)));

    /// <summary>
    /// Получение контакта по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        => Ok(_mapper.Map<ContactResponse>(
            await _contactRepository.GetByIdAsync(id, cancellationToken)));

    /// <summary>
    /// Создание нового контакта
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddAsync(
        CreateContactRequest request,
        CancellationToken cancellationToken)
    {
        var contact = _mapper.Map<Contact>(request);

        await _contactRepository.AddAsync(contact, cancellationToken);

        return Ok(new { id = contact.Id });
    }

    /// <summary>
    /// Обновление данных в контакте
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(
        int id,
        UpdateContactRequest request,
        CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetByIdAsync(id, cancellationToken);

        if (contact is null)
            return NotFound();

        var contactModel = _mapper.Map<Contact>(request);
        contactModel.Id = contact.Id;

        await _contactRepository.UpdateAsync(contactModel, cancellationToken);

        return Ok(new { id = contact.Id });
    }

    /// <summary>
    /// Удаление контакта
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetByIdAsync(id, cancellationToken);

        if (contact is null)
            return NotFound();

        await _contactRepository.DeleteAsync(id, cancellationToken);

        return Ok();
    }
}
