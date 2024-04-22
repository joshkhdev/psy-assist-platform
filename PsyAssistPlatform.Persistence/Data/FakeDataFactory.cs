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
                },
                new()
                {
                    Id = 2,
                    Name = "Сurator"
                },
                new()
                {
                    Id = 3,
                    Name = "Admin"
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
                    Name = "Hyponosis"
                }
        };

    public static IEnumerable<Status> Statuses
        => new List<Status>()
        {
                new()
                {
                    Id = 1,
                    Name = "New"
                },
                new()
                {
                    Id = 2,
                    Name = "InProcessing"
                },
                new()
                {
                    Id = 3,
                    Name = "Completed"
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

    public static IEnumerable<Contact> Contacts
        => new List<Contact>()
        {
            new()
            {
                Id = 1,
                Telegram = "@ivanoff_fake",
                Email = "ivanov@fakemail.com",
                Phone = "+79991234567"
            }
        };
}
