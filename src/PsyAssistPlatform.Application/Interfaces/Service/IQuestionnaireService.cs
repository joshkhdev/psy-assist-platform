using PsyAssistPlatform.Application.Interfaces.Dto.Questionnaire;

namespace PsyAssistPlatform.Application.Interfaces.Service;

public interface IQuestionnaireService
{
    Task<IEnumerable<IQuestionnaire>?> GetQuestionnairesAsync(CancellationToken cancellationToken);

    Task<IQuestionnaire?> GetQuestionnaireByIdAsync(int id, CancellationToken cancellationToken);

    Task<IQuestionnaire> CreateQuestionnaireAsync(
        ICreateQuestionnaire questionnaireData, 
        CancellationToken cancellationToken);
}