﻿namespace PsyAssistPlatform.WebApi.Models.Contact;

public record UpdateContactRequest
{
    public string? Telegram { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
}
