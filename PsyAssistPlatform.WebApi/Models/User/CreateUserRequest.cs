﻿using PsyAssistPlatform.Application.Interfaces.Dto.User;

namespace PsyAssistPlatform.WebApi.Models.User;

public record CreateUserRequest : ICreateUser
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public required int RoleId { get; set; }
}
