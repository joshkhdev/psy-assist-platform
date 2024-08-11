using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Messages;

namespace PsyAssistPlatform.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FeedbacksController : ControllerBase
{
    private readonly IRequestClient<FeedbacksMessage> _requestClient;

    public FeedbacksController(
        IRequestClient<FeedbacksMessage> requestClient)
    {
        _requestClient = requestClient;
    }

    /// <summary>
    /// Получить список всех отзывов
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<FeedbackMessage>> GetAllFeedbacksAsync(CancellationToken cancellationToken)
    {
        var response = await _requestClient.GetResponse<FeedbacksMessage>(new List<FeedbackMessage>(), cancellationToken);
        var feedbacks = new List<FeedbackMessage>(response.Message.Items);

        return feedbacks;
    }
}
