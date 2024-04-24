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
    public async Task<IActionResult> GetAllContactsAsync(CancellationToken cancellationToken)
    {
        var contacts = await _contactRepository.GetAllAsync(cancellationToken);

        var contactResponses = _mapper.Map<IEnumerable<ContactResponse>>(contacts);

        return Ok(contactResponses);
    }

    /// <summary>
    /// Получение контакта по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactByIdAsync(int id, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetByIdAsync(id, cancellationToken);

        var contactResponse = _mapper.Map<ContactResponse>(contact);

        return Ok(contactResponse);
    }

    /// <summary>
    /// Создание нового контакта
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddContactAsync(
        CreateContactRequest request,
        CancellationToken cancellationToken)
    {
        var contact = _mapper.Map<Contact>(request);

        await _contactRepository.AddAsync(contact, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Обновление данных в контакте
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateContactAsync(
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

        return Ok();
    }

    /// <summary>
    /// Удаление контакта
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteContactAsync(int id, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetByIdAsync(id, cancellationToken);

        if (contact is null)
            return NotFound();

        await _contactRepository.DeleteAsync(id, cancellationToken);

        return Ok();
    }
}
