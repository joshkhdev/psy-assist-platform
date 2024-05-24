using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.WebApi.Models.Contact;

namespace PsyAssistPlatform.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;

    public ContactsController(IContactService contactService, IMapper mapper)
    {
        _contactService = contactService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список всех контактов
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<ContactResponse>> GetAllContactsAsync(CancellationToken cancellationToken)
    {
        var contacts = await _contactService.GetContactsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ContactResponse>>(contacts);
    }

    /// <summary>
    /// Получить контакт по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContactResponse>> GetContactByIdAsync(int id, CancellationToken cancellationToken)
    {
        var contact = await _contactService.GetContactByIdAsync(id, cancellationToken);
        return _mapper.Map<ContactResponse>(contact);
    }

    /// <summary>
    /// Обновить данные контакта
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateContactAsync(
        int id,
        UpdateContactRequest request,
        CancellationToken cancellationToken)
    {
        await _contactService.UpdateContactAsync(id, request, cancellationToken);
        return Ok();
    }
}
