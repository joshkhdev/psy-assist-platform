using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Messages;
using PsyAssistPlatform.WebApi.Models.Feedback;

namespace PsyAssistPlatform.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FeedbacksController : ControllerBase
{
    private readonly IRequestClient<FeedbacksMessage> _requestClient;

    public FeedbacksController(IRequestClient<FeedbacksMessage> requestClient)
    {
        _requestClient = requestClient;
    }

    /// <summary>
    /// Получить список всех отзывов
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbacksAsync(CancellationToken cancellationToken)
    {
        var response = await _requestClient.GetResponse<FeedbacksMessage>(new List<FeedbackResponse>(), cancellationToken);

        return response.Message.Items;
    }
}
