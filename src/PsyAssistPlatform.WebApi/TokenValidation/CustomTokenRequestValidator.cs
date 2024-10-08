﻿using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Security.Claims;

namespace PsyAssistPlatform.WebApi.TokenValidation;

public class CustomTokenRequestValidator: ICustomTokenRequestValidator
{
    public Task ValidateAsync(CustomTokenRequestValidationContext context)
    {
        var request = context.Result.ValidatedRequest;

        if (request.GrantType == GrantType.ClientCredentials)
        {
            var roleClaim = request.Raw.Get("role");
            if (!string.IsNullOrWhiteSpace(roleClaim))
            {
                var clientClaims = new List<Claim>
                {
                    new Claim("role", roleClaim)
                };

                clientClaims.ForEach(request.ClientClaims.Add);
            }
        }

        return Task.CompletedTask;
    }
}
