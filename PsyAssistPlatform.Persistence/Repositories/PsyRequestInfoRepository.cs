using Microsoft.EntityFrameworkCore;
using PsyAssistPlatform.Application.Dto.PsychologistProfile;
using PsyAssistPlatform.Application.Dto.PsyRequest;
using PsyAssistPlatform.Application.Dto.Questionnaire;
using PsyAssistPlatform.Application.Dto.Status;
using PsyAssistPlatform.Application.Interfaces.Dto.PsyRequest;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.Repositories;

public class PsyRequestInfoRepository : IPsyRequestInfoRepository
{
    private readonly PsyAssistContext _context;

    public PsyRequestInfoRepository(PsyAssistContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<IPsyRequestInfo>?> GetAllPsyRequestsAsync(CancellationToken cancellationToken)
    {
        var latestPsyRequestStatuses = await _context.PsyRequestStatuses
            .GroupBy(psyRequestStatus => psyRequestStatus.PsyRequestId)
            .Select(grouping => grouping.OrderByDescending(psyRequestStatus => psyRequestStatus.StatusUpdateDate)
                .First())
            .Take(500)
            .ToListAsync(cancellationToken);

        var psyRequestsData = await _context.PsyRequests
            .Where(psyRequest => latestPsyRequestStatuses.Select(psyRequestStatus => psyRequestStatus.PsyRequestId)
                .Contains(psyRequest.Id)).Include(psyRequest => psyRequest.Questionnaire)
            .Include(psyRequest => psyRequest.PsychologistProfile)
            .OrderBy(psyRequest => psyRequest.Id)
            .ToListAsync(cancellationToken);

        var psyRequestsInfo = psyRequestsData.Select(psyRequest =>
            new PsyRequestInfoDto
            {
                Id = psyRequest.Id,
                Questionnaire = GetQuestionnaireDto(psyRequest.Questionnaire),
                PsychologistProfile = GetPsychologistProfileDto(psyRequest.PsychologistProfile),
                PsyRequestStatus =
                    GetPsyRequestStatusDto(latestPsyRequestStatuses.First(psyRequestStatus =>
                        psyRequestStatus.PsyRequestId == psyRequest.Id))
            }).ToList();

        return psyRequestsInfo;
    }

    public async Task<IEnumerable<IPsyRequestInfo>?> GetPsyRequestsByStatusIdAsync(
        int statusId,
        CancellationToken cancellationToken)
    {
        var psyRequestsData = await _context.PsyRequests
            .Where(psyRequest => psyRequest.PsyRequestStatuses
                .OrderByDescending(psyRequestStatus => psyRequestStatus.StatusUpdateDate)
                .First().StatusId == statusId)
            .Include(psyRequest => psyRequest.Questionnaire)
            .Include(psyRequest => psyRequest.PsychologistProfile)
            .Include(psyRequest => psyRequest.PsyRequestStatuses)
            .ToListAsync(cancellationToken);

        var psyRequestsInfo = psyRequestsData.Select(psyRequest =>
            new PsyRequestInfoDto
            {
                Id = psyRequest.Id,
                Questionnaire = GetQuestionnaireDto(psyRequest.Questionnaire),
                PsychologistProfile = GetPsychologistProfileDto(psyRequest.PsychologistProfile),
                PsyRequestStatus =
                    GetPsyRequestStatusDto(psyRequest.PsyRequestStatuses
                        .OrderByDescending(psyRequestStatus => psyRequestStatus.StatusUpdateDate)
                        .First())
            }).ToList();

        return psyRequestsInfo;
    }

    public async Task<IEnumerable<IPsyRequestInfo>?> GetPsyRequestInfoByIdAsync(int id, CancellationToken cancellationToken)
    {
        var psyRequestInfoData = await _context.PsyRequestStatuses
            .Include(psyRequestStatus => psyRequestStatus.Status)
            .Include(psyRequestStatus => psyRequestStatus.PsyRequest)
            .ThenInclude(psyRequest => psyRequest.Questionnaire)
            .Include(psyRequestStatus => psyRequestStatus.PsyRequest)
            .ThenInclude(psyRequest => psyRequest.PsychologistProfile)
            .Where(psyRequestStatus => psyRequestStatus.PsyRequestId == id)
            .ToListAsync(cancellationToken);
        
        var psyRequestsInfo = psyRequestInfoData.Select(psyRequestStatus => 
            new PsyRequestInfoDto
            {
                Id = psyRequestStatus.PsyRequest.Id,
                Questionnaire = GetQuestionnaireDto(psyRequestStatus.PsyRequest.Questionnaire),
                PsychologistProfile = GetPsychologistProfileDto(psyRequestStatus.PsyRequest.PsychologistProfile),
                PsyRequestStatus = new PsyRequestStatusDto()
                {
                    Comment = psyRequestStatus.Comment,
                    StatusUpdateDate = psyRequestStatus.StatusUpdateDate,
                    Status = new StatusDto
                    {
                        Id = psyRequestStatus.Status.Id,
                        Name = psyRequestStatus.Status.Name
                    }
                }
            }).ToList();

        return psyRequestsInfo;
    }

    public async Task<IPsyRequestInfo?> GetPsyRequestByIdAsync(int id, CancellationToken cancellationToken)
    {
        var latestPsyRequestStatuses = await _context.PsyRequestStatuses
            .GroupBy(psyRequestStatus => psyRequestStatus.PsyRequestId)
            .Select(grouping => grouping.OrderByDescending(psyRequestStatus => psyRequestStatus.StatusUpdateDate)
                .First())
            .Take(500)
            .ToListAsync(cancellationToken);

        var psyRequests = await _context.PsyRequests
            .Where(psyRequest => latestPsyRequestStatuses.Select(psyRequestStatus => psyRequestStatus.PsyRequestId)
                .Contains(psyRequest.Id)).Include(psyRequest => psyRequest.Questionnaire)
            .Include(psyRequest => psyRequest.PsychologistProfile)
            .SingleOrDefaultAsync(psyRequest => psyRequest.Id == id, cancellationToken);

        return psyRequests is not null
            ? new PsyRequestInfoDto
            {
                Id = psyRequests.Id,
                Questionnaire = GetQuestionnaireDto(psyRequests.Questionnaire),
                PsychologistProfile = GetPsychologistProfileDto(psyRequests.PsychologistProfile),
                PsyRequestStatus = GetPsyRequestStatusDto(latestPsyRequestStatuses.First(psyRequestStatus =>
                    psyRequestStatus.PsyRequestId == id))
            }
            : null;
    }

    private static QuestionnaireDto GetQuestionnaireDto(Questionnaire questionnaire)
    {
        return new QuestionnaireDto
        {
            Id = questionnaire.Id,
            Name = questionnaire.Name,
            Pronouns = questionnaire.Pronouns,
            Age = questionnaire.Age,
            TimeZone = questionnaire.TimeZone,
            Telegram = questionnaire.Contact.Telegram,
            Email = questionnaire.Contact.Email,
            Phone = questionnaire.Contact.Phone,
            NeuroDifferences = questionnaire.NeuroDifferences,
            MentalSpecifics = questionnaire.MentalSpecifics,
            PsyWishes = questionnaire.PsyWishes,
            PsyQuery = questionnaire.PsyQuery,
            TherapyExperience = questionnaire.TherapyExperience,
            IsForPay = questionnaire.IsForPay,
            RegistrationDate = questionnaire.RegistrationDate,
        };
    }

    private static PsychologistProfileDto? GetPsychologistProfileDto(PsychologistProfile? psychologistProfile)
    {
        if (psychologistProfile is null)
            return null;

        return new PsychologistProfileDto
        {
            Name = psychologistProfile.Name,
            Description = psychologistProfile.Description,
            TimeZone = psychologistProfile.TimeZone,
            IncludingQueries = psychologistProfile.IncludingQueries,
            ExcludingQueries = psychologistProfile.ExcludingQueries,
            UserId = psychologistProfile.UserId,
            IsActive = psychologistProfile.IsActive
        };
    }

    private static PsyRequestStatusDto GetPsyRequestStatusDto(PsyRequestStatus psyRequestStatus)
    {
        return new PsyRequestStatusDto
        {
            Status = new StatusDto()
            {
                Id = psyRequestStatus.Status.Id,
                Name = psyRequestStatus.Status.Name
            },
            StatusUpdateDate = psyRequestStatus.StatusUpdateDate,
            Comment = psyRequestStatus.Comment
        };
    }
}