﻿namespace PsyAssistPlatform.WebApi.Models.User;

public record CreateUserRequest
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public int RoleId { get; set; }
}
