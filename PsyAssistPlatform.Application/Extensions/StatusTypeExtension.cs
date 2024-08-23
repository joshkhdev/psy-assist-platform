namespace PsyAssistPlatform.Application.Extensions;

public static class StatusTypeExtension
{
    public static string ToDatabaseString(this StatusType status)
    {
        return status switch
        {
            StatusType.New => "New",
            StatusType.InProcessing => "InProcessing",
            StatusType.Completed => "Completed",
            StatusType.Canceled => "Canceled",
            StatusType.Rejected => "Rejected",
            _ => throw new ArgumentException("Status is not defined in the database")
        };
    }
    
    public static StatusType ToStatusTypeEnum(this string status)
    {
        return status switch
        {
            "New" => StatusType.New,
            "InProcessing" => StatusType.InProcessing,
            "Completed" => StatusType.Completed,
            "Canceled" => StatusType.Canceled,
            "Rejected" => StatusType.Rejected,
            _ => throw new ArgumentException("Status is not defined in the StatusType enum")
        };
    }
}