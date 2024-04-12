using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.Data;
public static class FakeDataFactory
{
    public static IEnumerable<Role> Roles
        => new List<Role>()
        {
                new()
                {
                    Id = 1,
                    Name = "Psychologist"
                }
        };

    public static IEnumerable<Approach> Approaches
        => new List<Approach>()
        {
                new()
                {
                    Id = 1,
                    Name = "Psychoanalysis"
                },
                new()
                {
                    Id = 2,
                    Name = "hyponosis"
                }
        };

    public static IEnumerable<User> Users
        => new List<User>()
        {
                new()
                {
                    Id = 1,
                    Name = "Ivan Ivanov",
                    Email = "ivanov@goooooooogle.org",
                    IsBlocked = false,
                    Password = "qwerty",
                    RoleId = 1
                }
        };

    public static IEnumerable<Psychologist> Psychologists
        => new List<Psychologist>()
    {
            new()
            {
                Id = 1,
                Description = "No Description",
                IsActive = true,
                Name = "Ivan Ivanov",
                RequestsInclude = "List 1",
                RequestsExclude = "List 2",
                TimeZone =  TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
                UserId = 1
            }
    };
}
