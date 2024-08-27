using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using PsyAssistPlatform.Application.Dto.PsychologistProfile;
using PsyAssistPlatform.Application.Dto.PsyRequest;
using PsyAssistPlatform.Application.Dto.Questionnaire;
using PsyAssistPlatform.Application.Dto.Status;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Extensions;
using PsyAssistPlatform.Application.Interfaces.Dto.PsyRequest;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Application.Services;

public class PsyRequestInfoService : IPsyRequestInfoService
{
    private readonly IPsyRequestInfoRepository _psyRequestInfoRepository;
    private readonly IRepository<Status> _statusRepository;
    private readonly IRepository<PsyRequest> _psyRequestRepository;
    private readonly IRepository<PsychologistProfile> _psychologistProfileRepository;
    private readonly IPsyRequestStatusRepository _psyRequestStatusRepository;
    private readonly IMapper _applicationMapper;
    private readonly IMemoryCache _memoryCache;
    
    private const string PsyRequestInfoCacheName = "PsyRequestInfo_{0}";
    private const string PsychologistRequestNotFoundMessage = "Psychological request with Id [{0}] not found";
    private const string StatusNotFoundMessage = "Status with Id [{0}] not found";
    
    public PsyRequestInfoService(
        IPsyRequestInfoRepository psyRequestInfoRepository,
        IRepository<Status> statusRepository,
        IRepository<PsyRequest> psyRequestRepository,
        IRepository<PsychologistProfile> psychologistProfileRepository,
        IPsyRequestStatusRepository psyRequestStatusRepository,
        IMapper applicationMapper,
        IMemoryCache memoryCache)
    {
        _psyRequestInfoRepository = psyRequestInfoRepository;
        _statusRepository = statusRepository;
        _psyRequestRepository = psyRequestRepository;
        _psychologistProfileRepository = psychologistProfileRepository;
        _psyRequestStatusRepository = psyRequestStatusRepository;
        _applicationMapper = applicationMapper;
        _memoryCache = memoryCache;
    }
    
    public async Task<IEnumerable<IPsyRequestInfo>?> GetAllPsyRequestsInfoAsync(CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(PsyRequestInfoCacheName, "All");
        var psyRequestsInfo = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
            
            var allPsyRequestsInfo = await _psyRequestInfoRepository.GetAllPsyRequestsAsync(cancellationToken);
            return _applicationMapper.Map<IEnumerable<PsyRequestInfoDto>>(allPsyRequestsInfo);
        });

        return psyRequestsInfo;
    }

    public async Task<IEnumerable<IPsyRequestInfo>?> GetPsyRequestsInfoByStatusIdAsync(int statusId, CancellationToken cancellationToken)
    {
        var status = await _statusRepository.GetByIdAsync(statusId, cancellationToken);
        if (status is null)
            throw new NotFoundException(string.Format(StatusNotFoundMessage, statusId));
        
        return await _psyRequestInfoRepository.GetPsyRequestsByStatusIdAsync(statusId, cancellationToken);
    }

    public async Task<IEnumerable<IPsyRequestInfo>?> GetPsyRequestInfoByIdAsync(int id, CancellationToken cancellationToken)
    {
        var psyRequest = await _psyRequestRepository.GetByIdAsync(id, cancellationToken);
        if (psyRequest is null)
            throw new NotFoundException(string.Format(PsychologistRequestNotFoundMessage, id));

        return await _psyRequestInfoRepository.GetPsyRequestInfoByIdAsync(id, cancellationToken);
    }

    public async Task<IPsyRequestInfo?> ChangePsyRequestStatusAsync(
        IChangePsyRequestStatus changePsyRequestStatusData, 
        CancellationToken cancellationToken)
    {
        switch (changePsyRequestStatusData.NewStatusId)
        {
            case (int)StatusType.New:
                throw new IncorrectDataException($"Invalid value new status Id [{changePsyRequestStatusData.NewStatusId}]");
            case (int)StatusType.Rejected:
                throw new IncorrectDataException(
                    $"Only administrator can change status to [{((StatusType)changePsyRequestStatusData.NewStatusId).ToDatabaseString()}]");
        }

        var psyRequest = await _psyRequestRepository.GetByIdAsync(changePsyRequestStatusData.PsyRequestId, cancellationToken);
        if (psyRequest is null)
        {
            throw new NotFoundException(string.Format(PsychologistRequestNotFoundMessage,
                changePsyRequestStatusData.PsyRequestId));
        }
        
        var status = await _statusRepository.GetByIdAsync(changePsyRequestStatusData.NewStatusId, cancellationToken);
        if (status is null)
        {
            throw new IncorrectDataException(string.Format(StatusNotFoundMessage,
                changePsyRequestStatusData.NewStatusId));
        }

        var psychologistProfile =
            await _psychologistProfileRepository.GetByIdAsync(changePsyRequestStatusData.PsychologistProfileId, cancellationToken);
        if (psychologistProfile is null)
        {
            throw new IncorrectDataException(
                $"Psychologist profile with Id [{changePsyRequestStatusData.PsychologistProfileId}] not found");
        }

        if (psyRequest.PsychologistProfile != null 
            && psyRequest.PsychologistProfile.Id != changePsyRequestStatusData.PsychologistProfileId)
        {
            throw new BusinessLogicException(
                $"Psychological request with Id [{psyRequest.Id}] is occupied by a psychologist with Id [{psyRequest.PsychologistProfile.Id}]");
        }

        var lastRequestStatus =
            await _psyRequestStatusRepository.GetLastStatusByPsyRequestIdAsync(psyRequest.Id, cancellationToken);
        switch (lastRequestStatus.StatusId)
        {
            case (int)StatusType.Completed 
                or (int)StatusType.Canceled
                or (int)StatusType.Rejected:
                throw new BusinessLogicException(
                    "Psychological request has a final status. Please contact the administrator");
            case (int)StatusType.New 
            when changePsyRequestStatusData.NewStatusId == (int)StatusType.Completed:
                throw new BusinessLogicException(
                    "To change psychological request to Completed it must have the status In progress");
        }

        if (lastRequestStatus.StatusId == changePsyRequestStatusData.NewStatusId)
        {
            throw new IncorrectDataException(
                $"Psychological request is already in status with Id [{changePsyRequestStatusData.NewStatusId}]");
        }

        _memoryCache.Remove(string.Format(PsyRequestInfoCacheName, "All"));
        
        var psyRequestStatus = new PsyRequestStatus
        {
            StatusId = changePsyRequestStatusData.NewStatusId,
            PsyRequestId = changePsyRequestStatusData.PsyRequestId,
            StatusUpdateDate = DateTime.Now.ToUniversalTime(),
            Comment = changePsyRequestStatusData.NewStatusId switch
            {
                2 =>
                    $"В работе. ID анкеты [{psyRequest.Questionnaire.Id}]. ID психолога [{changePsyRequestStatusData.PsychologistProfileId}]. {changePsyRequestStatusData.Comment}",
                3 =>
                    $"Завершено. ID анкеты [{psyRequest.Questionnaire.Id}]. ID психолога [{changePsyRequestStatusData.PsychologistProfileId}]. {changePsyRequestStatusData.Comment}",
                4 =>
                    $"Отменено. ID анкеты [{psyRequest.Questionnaire.Id}]. ID психолога [{changePsyRequestStatusData.PsychologistProfileId}]. {changePsyRequestStatusData.Comment}",
                _ => throw new ArgumentOutOfRangeException(nameof(IChangePsyRequestStatus.NewStatusId),
                    changePsyRequestStatusData.NewStatusId, null)
            }
        };
        await _psyRequestStatusRepository.AddPsyRequestStatusAsync(psyRequestStatus, cancellationToken);
        
        if ((StatusType)changePsyRequestStatusData.NewStatusId == StatusType.InProcessing)
        {
            await _psyRequestRepository.UpdateAsync(
                new PsyRequest
                {
                    Id = psyRequest.Id,
                    QuestionnaireId = psyRequest.Questionnaire.Id,
                    PsychologistProfileId = changePsyRequestStatusData.PsychologistProfileId
                }, cancellationToken);
        }

        return new PsyRequestInfoDto
        {
            Id = changePsyRequestStatusData.PsyRequestId,
            Questionnaire = _applicationMapper.Map<QuestionnaireDto>(psyRequest.Questionnaire),
            PsychologistProfile = _applicationMapper.Map<PsychologistProfileDto>(psychologistProfile),
            PsyRequestStatus = new PsyRequestStatusDto
            {
                Status = new StatusDto
                {
                    Id = changePsyRequestStatusData.NewStatusId,
                    Name = ((StatusType)changePsyRequestStatusData.NewStatusId).ToDatabaseString()
                },
                StatusUpdateDate = psyRequestStatus.StatusUpdateDate,
                Comment = psyRequestStatus.Comment
            }
        };
    }

    public async Task<IPsyRequestInfo?> RejectPsyRequestAsync(
        int psyRequestId, 
        int userId, 
        string comment,
        CancellationToken cancellationToken)
    {
        _memoryCache.Remove(string.Format(PsyRequestInfoCacheName, "All"));
        
        // TODO: Реализовать после внедрения авторизации
        throw new NotImplementedException();
    }
}